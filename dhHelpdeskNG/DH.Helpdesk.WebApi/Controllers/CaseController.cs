using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Web.Common.Constants.Case;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;
using DateTime = System.DateTime;

namespace DH.Helpdesk.WebApi.Controllers
{
    //[Route( "api/v{version:apiVersion}" )]
    [RoutePrefix("api/Case")]
    public class CaseController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFileService _caseFileService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseFieldSettingsHelper _caseFieldSettingsHelper;
        private readonly IBaseCaseSolutionService _caseSolutionService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ISupplierService _supplierService;
        private readonly ISettingService _customerSettingsService;
        private readonly IMapper _mapper;
        private readonly ICaseEditModeCalcStrategy _caseEditModeCalcStrategy;
        private readonly ICaseTranslationService _caseTranslationService;

        #region ctor()

        public CaseController(ICaseService caseService, ICaseFileService caseFileService, ICaseFieldSettingService caseFieldSettingService,
            ICaseFieldSettingsHelper caseFieldSettingsHelper, IBaseCaseSolutionService caseSolutionService,
            ICustomerUserService customerUserService, IUserService userService, IWorkingGroupService workingGroupService,
            ISupplierService supplierService, ISettingService customerSettingsService, ICaseTranslationService caseTranslationService,
            ICaseEditModeCalcStrategy caseEditModeCalcStrategy,
            IMapper mapper)
        {
            _caseService = caseService;
            _caseFileService = caseFileService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _customerUserService = customerUserService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _supplierService = supplierService;
            _customerSettingsService = customerSettingsService;
            _caseTranslationService = caseTranslationService;
            _caseSolutionService = caseSolutionService;
            _mapper = mapper;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
        }

        #endregion

        /// <summary>
        /// Get case data.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="langId"></param>
        /// <param name="caseId"></param>
        /// <param name="cid"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}")]
        public async Task<CaseEditOutputModel> Get([FromUri]int langId, [FromUri]int caseId, [FromUri]int cid, [FromUri]string sessionId = null)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var languageId = langId;
            var currentCase = _caseService.GetDetachedCaseById(caseId);
            var currentCid = cid;
            var userGroupId = User.Identity.GetGroupId();

            if (currentCase.Customer_Id != currentCid)
                throw new Exception($"Case customer({currentCase.Customer_Id}) and current customer({currentCid}) are different"); //TODO: how to react?

            var customerUserSetting = _customerUserService.GetCustomerUserSettings(currentCid, userId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{currentCid}' and user '{userId}'");

            var customerSettings = _customerSettingsService.GetCustomerSettings(currentCid);
            var userOverview = await _userService.GetUserOverviewAsync(UserId);// TODO: use cached version!
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(currentCid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(currentCid);

            model.Fields = new List<IBaseCaseField>();

            //TODO: Move to mapper
            model.Id = currentCase.Id;
            model.CaseNumber = currentCase.CaseNumber;
            model.CaseGuid = currentCase.CaseGUID;

            //case solution
            if (currentCase.CurrentCaseSolution_Id.HasValue)
            {
                var caseSolutionId = currentCase.CurrentCaseSolution_Id.Value;
                var caseSolution = _caseSolutionService.GetCaseSolution(caseSolutionId);
                //_caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseSolutionId);//TODO: Case solution settings participate in visibility check
                if (caseSolution != null)
                {
                    model.CaseSolution = _mapper.Map<CaseSolutionInfo>(caseSolution);
                }
            }

            //if (!string.IsNullOrWhiteSpace(currentCase.ReportedBy))//TODO:
            //{
            //    var reportedByUser = _computerService.GetComputerUserByUserID(currentCase.ReportedBy);
            //    if (reportedByUser?.ComputerUsersCategoryID != null)
            //    {
            //        var initiatorComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
            //        if (initiatorComputerUserCategory != null)
            //        {
            //            model.InitiatorReadOnly = initiatorComputerUserCategory.IsReadOnly;
            //        }
            //    }
            //}
            IBaseCaseField field = null;

            #region Initiator

            // Initiator
            //displayUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayUserInfoHtml
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                //if (Model.ComputerUserCategories.Any())
                //GlobalEnums.TranslationCaseFields.UserSearchCategory_Id//TODO:add UserSearchCategory_Id
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReportedBy))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.ReportedBy,
                        Value = currentCase.ReportedBy,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ReportedBy, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReportedBy, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                    model.Fields.Add(field);
                }

                //initiator category Id
                //if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id))
                //{
                //    field = new BaseCaseField<string>()
                //    {
                //        Name = CaseFieldsNamesApi.UserSearchCategory_Id.ToString(),
                //        Value = //todo:  set default value from field settings?
                //        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id, languageId, cid, caseFieldTranslations),
                //        Section = CaseSectionType.Initiator,
                //        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id, caseFieldSettings)
                //    };
                //    model.Fields.Add(field);
                //}

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Name))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.PersonName,
                        Value = currentCase.PersonsName,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Name, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Name, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_EMail))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.PersonEmail,
                        Value = currentCase.PersonsEmail,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_EMail, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_EMail, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Phone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.PersonPhone,
                        Value = currentCase.PersonsPhone,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Phone, languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Phone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_CellPhone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.PersonCellPhone,
                        Value = currentCase.PersonsCellphone,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_CellPhone,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_CellPhone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Region_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.RegionId,
                        Value = currentCase.Region_Id,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Region_Id,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Region_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Department_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.DepartmentId,
                        Value = currentCase.Department_Id,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Department_Id,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.OU_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.OrganizationUnitId,
                        Value = currentCase.OU_Id,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.OU_Id,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.OU_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CostCentre))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.CostCentre,
                        Value = currentCase.CostCentre,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.CostCentre,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CostCentre, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Place))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.Place,
                        Value = currentCase.Place,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Place,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Place, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.UserCode))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.UserCode,
                        Value = currentCase.UserCode,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.UserCode,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Initiator,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UserCode, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                //field = new BaseCaseField<bool>()//TODO: for edit
                //{
                //    Name = CaseFieldsNamesApi.UpdateNotifierInformation.ToString(),
                //    Value = true,
                //    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, 
                //        languageId, cid, caseFieldTranslations, ""),
                //    Section = CaseSectionType.Initiator,
                //    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings//        )
                //};
                //model.Fields.Add(field);



            }

            #endregion

            #region Regarding

            // Regarding
            //displayAboutUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayAboutUserInfoHtml
            
            //regarding category Id
            //if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id))
            //{
            //    field = new BaseCaseField<string>()
            //    {
            //        Name = CaseFieldsNamesApi.IsAbout_UserSearchCategory_Id.ToString(),
            //        Value = //todo:  set default value from field settings?
            //        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id, languageId, cid, caseFieldTranslations),
            //        Section = CaseSectionType.Regarding,
            //        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id, caseFieldSettings)
            //    };
            //    model.Fields.Add(field);
            //}

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_ReportedBy,
                    Value = currentCase.IsAbout?.ReportedBy,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_PersonName,
                    Value = currentCase.IsAbout?.Person_Name,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_PersonEmail,
                    Value = currentCase.IsAbout?.Person_Email,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_PersonPhone,
                    Value = currentCase.IsAbout?.Person_Phone,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_PersonCellPhone,
                    Value = currentCase.IsAbout?.Person_Cellphone,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "30"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_RegionId,
                    Value = currentCase.IsAbout?.Region_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_DepartmentId,
                    Value = currentCase.IsAbout?.Department_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_OrganizationUnitId,
                    Value = currentCase.IsAbout?.OU_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_CostCentre,
                    Value = currentCase.IsAbout?.CostCentre,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Place))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_Place,
                    Value = currentCase.IsAbout?.Place,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Place,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Place, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_UserCode))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.IsAbout_UserCode,
                    Value = currentCase.IsAbout?.UserCode,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_UserCode,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.Regarding,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            #endregion 

            #region ComputerInfo

            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.InventoryNumber,
                    Value = currentCase.InventoryNumber,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryNumber, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.ComputerInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "60"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ComputerType_Id))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.ComputerTypeId,
                    Value = currentCase.InventoryType,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ComputerType_Id, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.ComputerInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ComputerType_Id, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryLocation))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.InventoryLocation,
                    Value = currentCase.InventoryLocation,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryLocation, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.ComputerInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryLocation, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            #endregion

            #region CaseInfo

            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.CaseNumber,
                    Value = currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseNumber,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseNumber, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = CaseFieldsNamesApi.RegTime,
                    Value = currentCase.RegTime,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.RegTime,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegTime, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ChangeTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = CaseFieldsNamesApi.ChangeTime,
                    Value = currentCase.ChangeTime,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ChangeTime,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ChangeTime, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.User_Id))
            {
                var userIdValue = "";
                if (currentCase.User_Id.HasValue)
                {
                    var user = _userService.GetUser(currentCase.User_Id.Value);
                    WorkingGroupEntity caseOwnerDefaultWorkingGroup = null;
                    if (currentCase.DefaultOwnerWG_Id.HasValue && currentCase.DefaultOwnerWG_Id.Value > 0)
                    {
                        caseOwnerDefaultWorkingGroup = _workingGroupService.GetWorkingGroup(currentCase.DefaultOwnerWG_Id.Value);
                    }

                    if (user != null)
                    {
                        userIdValue = $"{user.FirstName} {user.SurName}";
                        if (caseOwnerDefaultWorkingGroup != null)
                            userIdValue += $" {caseOwnerDefaultWorkingGroup.WorkingGroupName}";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(currentCase.RegUserName))
                            userIdValue = currentCase.RegUserName;
                        if (!string.IsNullOrWhiteSpace(currentCase.RegUserId))
                            userIdValue += $" {currentCase.RegUserId}";
                    }
                }

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.UserId,
                    Value = userIdValue,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.User_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.User_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.RegistrationSourceCustomer,
                    Value = currentCase.RegistrationSourceCustomer_Id, //todo: check RegistrationSource
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer,

                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.CaseTypeId,
                    Value = currentCase.CaseType_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseType_Id, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseType_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.ProductAreaId,
                    Value = currentCase.ProductArea_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ProductArea_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ProductArea_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.System_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.SystemId,
                    Value = currentCase.System_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.System_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.System_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Urgency_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.UrgencyId,
                    Value = currentCase.Urgency_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Urgency_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Urgency_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Impact_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.ImpactId,
                    Value = currentCase.Impact_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Impact_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Impact_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Category_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.CategoryId,
                    Value = currentCase.Category_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Category_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Category_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Supplier_Id))
            {
                var supplier = currentCase.Supplier_Id.HasValue
                    ? _supplierService.GetSupplier(currentCase.Supplier_Id.Value)
                    : null;
                //if show Supplier_Id
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.SupplierId,
                    Value = currentCase.Supplier_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings)
                };
                model.Fields.Add(field);

                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.SupplierCountryId,
                    Value = supplier?.Country_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InvoiceNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.InvoiceNumber,
                    Value = currentCase.InvoiceNumber,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.InvoiceNumber,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InvoiceNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReferenceNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.ReferenceNumber,
                    Value = currentCase.ReferenceNumber,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ReferenceNumber,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReferenceNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Caption))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Caption,
                    Value = currentCase.Caption,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Caption,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Caption, currentCase.Id, customerUserSetting.CaptionPermission))
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Description))
            {
                var registrationSourceOptions = currentCase.RegistrationSource == (int) CaseRegistrationSource.Email
                    ? currentCase.Mail2Tickets.Where(x => x.Log_Id == null)
                        .GroupBy(x => x.Type)
                        .Select(gr => new KeyValuePair<string, string>(gr.Key, string.Join(";", gr.Select(x => x.EMailAddress)))).ToList()
                    : null;
              

                // Registration source: values 
                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.CaseRegistrationSource, //NOTE: should not be mistaken with another field - RegistrationSourceCustomer!
                    Value = currentCase.RegistrationSource,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Registration source"),
                    Section = CaseSectionType.CaseInfo,
                    Options = registrationSourceOptions
                };
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Description,
                    Value = currentCase.Description,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Description, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Description, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "10000"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Miscellaneous))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Miscellaneous,
                    Value = currentCase.Miscellaneous,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Miscellaneous,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Miscellaneous, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "1000"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ContactBeforeAction))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.ContactBeforeAction,
                    Value = currentCase.ContactBeforeAction.ToBool(),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings, 
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ContactBeforeAction, currentCase.Id, customerUserSetting.ContactBeforeActionPermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SMS))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.Sms,
                    Value = currentCase.SMS.ToBool(),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.SMS,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SMS, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.AgreedDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = CaseFieldsNamesApi.AgreedDate,
                    Value = currentCase.AgreedDate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.AgreedDate,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.AgreedDate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Available))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Available,
                    Value = currentCase.Available,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Available,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Available, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Cost))
            {

                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost,
                    Value = currentCase.Cost, 
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Artikelkostnad"), //Kostnad
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost_OtherCost,
                    Value = currentCase.OtherCost,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Cost_Currency,
                    Value = currentCase.Currency,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Valuta"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (/*customerSettings.AttachmentPlacement == 1 &&*/ _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Filename))
            {
                field = new BaseCaseField<IList<CaseFileModel>>()
                {
                    Name = CaseFieldsNamesApi.Filename,
                    Value = GetCaseFilesModel(currentCase.Id),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Filename, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Filename, caseFieldSettings)
                };

                model.Fields.Add(field);
            }

            #endregion

            #region CaseManagement

            //CaseManagement
            //displayCaseManagementInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.WorkingGroupId,
                    Value = currentCase.WorkingGroup_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.CaseResponsibleUserId,
                    Value = currentCase.CaseResponsibleUser_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.PerformerUserId,
                    Value = currentCase.Performer_User_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Performer_User_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Performer_User_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            //customerUserSetting.PriorityPermission - if 0 - readonly, else 
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.PriorityId,
                    Value = currentCase.Priority_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Priority_Id, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Priority_Id, currentCase.Id, customerUserSetting.PriorityPermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Status_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.StatusId,
                    Value = currentCase.Status_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Status_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Status_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.StateSecondaryId,
                    Value = currentCase.StateSecondary_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.StateSecondary_Id, currentCase.Id, customerUserSetting.StateSecondaryPermission))
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Project))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Project,
                    Value = currentCase.Project_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Project, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Problem))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Problem,
                    Value = currentCase.Problem_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Problem, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Problem, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CausingPart))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.CausingPart,
                    Value = currentCase.CausingPartId,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.CausingPart, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CausingPart, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Change)) //TODO: && Model.changes.Any()
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Change,
                    Value = currentCase.Change_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Change, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Change, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.PlanDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = CaseFieldsNamesApi.PlanDate,
                    Value = currentCase.PlanDate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.PlanDate, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.PlanDate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WatchDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = CaseFieldsNamesApi.WatchDate,
                    Value = currentCase.WatchDate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.WatchDate,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.WatchDate, currentCase.Id, customerUserSetting.WatchDatePermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Verified))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.Verified,
                    Value = currentCase.Verified.ToBool(),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Verified,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Verified, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.VerifiedDescription))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.VerifiedDescription,
                    Value = currentCase.VerifiedDescription,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.VerifiedDescription,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.VerifiedDescription, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "200"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SolutionRate))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.SolutionRate,
                    Value = currentCase.SolutionRate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.SolutionRate,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SolutionRate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            #endregion

            #region Communication Management

            if (userOverview.CloseCasePermission == 1)
            {

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.FinishingDescription))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.FinishingDescription,
                        Value = currentCase.FinishingDescription,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDescription,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDescription, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "200"));
                    model.Fields.Add(field);
                }

                int? finishingCause = null;
                var lastLog = currentCase.Logs?.FirstOrDefault(); //todo: check if its correct - order
                if (lastLog != null)
                {
                    finishingCause = lastLog.FinishingType;
                }
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ClosingReason))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = CaseFieldsNamesApi.ClosingReason,
                        Value = finishingCause,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ClosingReason,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ClosingReason, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.FinishingDate))
                {
                    field = new BaseCaseField<DateTime?>()
                    {
                        Name = CaseFieldsNamesApi.FinishingDate,
                        Value = currentCase.FinishingDate,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDate,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDate, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

            }

            #endregion

            //calc case edit mode
            model.EditMode = _caseEditModeCalcStrategy.CalcEditMode(cid, UserId, currentCase); // remember to apply isCaseLocked check on client

            _caseService.MarkAsRead(caseId);
            return model;
        }

        private IList<CaseFileModel> GetCaseFilesModel(int caseId)
        {
            return _caseFileService.GetCaseFiles(caseId, true);
        }

        public async Task<CaseEditOutputModel> New()
        {
            var model = new CaseEditOutputModel();
            //TODO:
            return await Task.FromResult(model);
        }

        private List<KeyValuePair<string, string>> GetFieldOptions(GlobalEnums.TranslationCaseFields field, IList<CaseFieldSetting> caseFieldSettings, bool? readOnly = null)
        //IList<CaseFieldSettingsWithLanguage> caseFieldSettingsTranslated)
        {
            var options = new List<KeyValuePair<string, string>>();
            var fieldName = field.ToString();

            //TODO: Move Replace("tblLog_", "tblLog.") to extension
            var setting = _caseFieldSettingsHelper.GetCaseFieldSetting(caseFieldSettings, fieldName);
            //var settingEx = caseFieldSettingsTranslated.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
            if (setting != null && setting.Required.ToBool())
            {
                options.Add(new KeyValuePair<string, string>("required", ""));
            }

            if (readOnly.HasValue && readOnly.Value)
            {
                options.Add(new KeyValuePair<string, string>("readonly", ""));
            }
            //if (settingEx != null && !string.IsNullOrWhiteSpace(settingEx.FieldHelp))
            //{
            //    options.Add(new KeyValuePair<string, string>("description", settingEx.FieldHelp));
            //}
            //TODO: check is readonly
            return options;
        }
    }
}