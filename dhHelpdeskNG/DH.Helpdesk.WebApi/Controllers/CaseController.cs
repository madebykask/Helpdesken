using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.User.Input;
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
using DH.Helpdesk.Services.utils;
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
        private readonly IDepartmentService _departmentService;
        private readonly IPriorityService _priorityService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;

        #region ctor()

        public CaseController(ICaseService caseService, ICaseFileService caseFileService,
            ICaseFieldSettingService caseFieldSettingService, ICaseFieldSettingsHelper caseFieldSettingsHelper,
            IBaseCaseSolutionService caseSolutionService, ICustomerUserService customerUserService,
            IUserService userService, IWorkingGroupService workingGroupService,
            ISupplierService supplierService, ISettingService customerSettingsService,
            IMapper mapper, ICaseEditModeCalcStrategy caseEditModeCalcStrategy, ICaseTranslationService caseTranslationService,
            IDepartmentService departmentService, IPriorityService priorityService, IWatchDateCalendarService watchDateCalendarService)
        {
            _caseService = caseService;
            _caseFileService = caseFileService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _caseSolutionService = caseSolutionService;
            _customerUserService = customerUserService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _supplierService = supplierService;
            _customerSettingsService = customerSettingsService;
            _mapper = mapper;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
            _caseTranslationService = caseTranslationService;
            _departmentService = departmentService;
            _priorityService = priorityService;
            _watchDateCalendarService = watchDateCalendarService;
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

            CreateInitiatorSection(cid, customerUserSetting, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model);

            #endregion

            #region Regarding

            CreateRegardingSection(cid, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model);

            #endregion 

            #region ComputerInfo

            CreateComputerInfoSection(cid, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model);

            #endregion

            #region CaseInfo

            CreateCaseInfoSection(cid, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting);

            #endregion

            #region CaseManagement

            CreateCaseManagementSection(cid, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting, customerSettings);

            #endregion

            #region Communication Management

            CreateCommunicationSection(cid, userOverview, caseFieldSettings, currentCase, null, languageId, caseFieldTranslations, model);

            #endregion

            //calc case edit mode
            model.EditMode = _caseEditModeCalcStrategy.CalcEditMode(cid, UserId, currentCase); // remember to apply isCaseLocked check on client

            _caseService.MarkAsRead(caseId);
            return model;
        }

        [HttpGet]
        [Route("template/{templateId:int}")]
        public async Task<IHttpActionResult> New([FromUri]int langId, [FromUri]int cid, [FromUri]int? templateId)
        {
            var customerUserSetting = _customerUserService.GetCustomerUserSettings(cid, UserId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{cid}' and user '{UserId}'");

            var userOverview = await _userService.GetUserOverviewAsync(UserId);// TODO: use cached version!
            if (!userOverview.CreateCasePermission.ToBool())
                SendResponse($"User {UserName} is not allowed to create case.", HttpStatusCode.Forbidden);

            var customerSettings = _customerSettingsService.GetCustomerSettings(cid);
            if (!templateId.HasValue && customerSettings.DefaultCaseTemplateId != 0)
                templateId = customerSettings.DefaultCaseTemplateId;
            if(!templateId.HasValue)
                throw new Exception("No template id found. Template id should be in parameter or set as default for customer.");

            var caseTemplate = _caseSolutionService.GetCaseSolution(templateId.Value);
            if (caseTemplate == null)
                throw new Exception($"Template '{templateId.Value}' can't be found.");

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(cid);

            var model = new CaseEditOutputModel()
            {
                Fields = new List<IBaseCaseField>()
            };
            model.CaseSolution = _mapper.Map<CaseSolutionInfo>(caseTemplate);

            CreateInitiatorSection(cid, customerUserSetting, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model);
            CreateRegardingSection(cid, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model);
            CreateComputerInfoSection(cid, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model);
            CreateCaseInfoSection(cid, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model, customerUserSetting);
            CreateCaseManagementSection(cid, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model, customerUserSetting, customerSettings);
            CreateCommunicationSection(cid, userOverview, caseFieldSettings, null, caseTemplate, langId, caseFieldTranslations, model);

            model.EditMode = Web.Common.Enums.Case.AccessMode.FullAccess;
            return Ok(model);
        }

        private void CreateInitiatorSection(int cid, CustomerUser customerUserSetting, IList<CaseFieldSetting> caseFieldSettings,
            Case currentCase, CaseSolution template, int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            // Initiator
            //displayUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayUserInfoHtml
            IBaseCaseField field;
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                //if (Model.ComputerUserCategories.Any())
                //GlobalEnums.TranslationCaseFields.UserSearchCategory_Id//TODO:add UserSearchCategory_Id
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReportedBy))
                {
                    field = GetField(currentCase != null ? currentCase.ReportedBy : template.ReportedBy, cid, languageId,
                        CaseFieldsNamesApi.ReportedBy, GlobalEnums.TranslationCaseFields.ReportedBy, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
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
                    field = GetField(currentCase != null ? currentCase.PersonsName : template.PersonsName, cid, languageId,
                        CaseFieldsNamesApi.PersonName, GlobalEnums.TranslationCaseFields.Persons_Name, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_EMail))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsEmail : template.PersonsEmail, cid, languageId,
                        CaseFieldsNamesApi.PersonEmail, GlobalEnums.TranslationCaseFields.Persons_EMail, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Phone))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsPhone : template.PersonsPhone, cid, languageId,
                        CaseFieldsNamesApi.PersonPhone, GlobalEnums.TranslationCaseFields.Persons_Phone, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_CellPhone))
                {
                    field = GetField(currentCase != null ? currentCase.PersonsCellphone : template.PersonsCellPhone, cid, languageId,
                        CaseFieldsNamesApi.PersonCellPhone, GlobalEnums.TranslationCaseFields.Persons_CellPhone, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Region_Id))
                {
                    field = GetField(currentCase != null ? currentCase.Region_Id : template.Region_Id, cid, languageId,
                        CaseFieldsNamesApi.RegionId, GlobalEnums.TranslationCaseFields.Region_Id, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Department_Id))
                {
                    field = GetField(currentCase != null ? currentCase.Department_Id : template.Department_Id, cid, languageId,
                        CaseFieldsNamesApi.DepartmentId, GlobalEnums.TranslationCaseFields.Department_Id, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.OU_Id))
                {
                    field = GetField(currentCase != null ? currentCase.OU_Id : template.OU_Id, cid, languageId,
                        CaseFieldsNamesApi.OrganizationUnitId, GlobalEnums.TranslationCaseFields.OU_Id, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CostCentre))
                {

                    field = GetField(currentCase != null ? currentCase.CostCentre : template.CostCentre, cid, languageId,
                        CaseFieldsNamesApi.CostCentre, GlobalEnums.TranslationCaseFields.CostCentre, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Place))
                {
                    field = GetField(currentCase != null ? currentCase.Place : template.Place, cid, languageId,
                        CaseFieldsNamesApi.Place, GlobalEnums.TranslationCaseFields.Place, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.UserCode))
                {
                    field = GetField(currentCase != null ? currentCase.UserCode : template.UserCode, cid, languageId,
                        CaseFieldsNamesApi.UserCode, GlobalEnums.TranslationCaseFields.UserCode, CaseSectionType.Initiator,
                        caseFieldSettings, caseFieldTranslations);
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
        }

        private void CreateRegardingSection(int cid, IList<CaseFieldSetting> caseFieldSettings, Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            IBaseCaseField field;
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
                field = GetField(currentCase != null ? currentCase.IsAbout?.ReportedBy : template.IsAbout_ReportedBy, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_ReportedBy, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Name : template.IsAbout_PersonsName, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonName, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Email : template.IsAbout_PersonsEmail, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonEmail, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Phone : template.IsAbout_PersonsPhone, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonPhone, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings,
                GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Person_Cellphone : template.IsAbout_PersonsCellPhone, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_PersonCellPhone, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "30"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Region_Id : template.IsAbout_Region_Id, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_RegionId, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Department_Id : template.IsAbout_Department_Id, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_DepartmentId, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.OU_Id : template.IsAbout_OU_Id, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_OrganizationUnitId, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.CostCentre : template.IsAbout_CostCentre, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_CostCentre, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Place))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.Place : template.IsAbout_Place, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_Place, GlobalEnums.TranslationCaseFields.IsAbout_Place, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_UserCode))
            {
                field = GetField(currentCase != null ? currentCase.IsAbout?.UserCode : template.IsAbout_UserCode, cid, languageId,
                    CaseFieldsNamesApi.IsAbout_UserCode, GlobalEnums.TranslationCaseFields.IsAbout_UserCode, CaseSectionType.Regarding,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }
        }

        private void CreateComputerInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings, Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            IBaseCaseField field;
            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryNumber))
            {
                field = GetField(currentCase != null ? currentCase.InventoryNumber : template.InventoryNumber, cid, languageId,
                    CaseFieldsNamesApi.InventoryNumber, GlobalEnums.TranslationCaseFields.InventoryNumber, CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "60"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ComputerType_Id))
            {
                field = GetField(currentCase != null ? currentCase.InventoryType : template.InventoryType, cid, languageId,
                    CaseFieldsNamesApi.ComputerTypeId, GlobalEnums.TranslationCaseFields.ComputerType_Id, CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryLocation))
            {
                field = GetField(currentCase != null ? currentCase.InventoryLocation : template.InventoryLocation, cid, languageId,
                    CaseFieldsNamesApi.InventoryLocation, GlobalEnums.TranslationCaseFields.InventoryLocation, CaseSectionType.ComputerInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }
        }

        private void CreateCaseInfoSection(int cid, IList<CaseFieldSetting> caseFieldSettings, Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model, CustomerUser customerUserSetting)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            IBaseCaseField field;
            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseNumber))
            {
                field = GetField(currentCase != null ? currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture) : "", cid, languageId,
                    CaseFieldsNamesApi.CaseNumber, GlobalEnums.TranslationCaseFields.CaseNumber, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegTime))
            {
                field = GetField(currentCase != null ? DateTime.SpecifyKind(currentCase.RegTime, DateTimeKind.Utc) : new DateTime?(), cid, languageId,
                    CaseFieldsNamesApi.RegTime, GlobalEnums.TranslationCaseFields.RegTime, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ChangeTime))
            {
                field = GetField(currentCase != null ? DateTime.SpecifyKind(currentCase.ChangeTime, DateTimeKind.Utc) : new DateTime?(), cid, languageId,
                    CaseFieldsNamesApi.ChangeTime, GlobalEnums.TranslationCaseFields.ChangeTime, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.User_Id))
            {
                var userIdValue = "";
                if (currentCase?.User_Id != null)
                {
                    var user = _userService.GetUser(currentCase.User_Id.Value);
                    WorkingGroupEntity caseOwnerDefaultWorkingGroup = null;
                    if (currentCase.DefaultOwnerWG_Id.HasValue && currentCase.DefaultOwnerWG_Id.Value > 0)
                    {
                        caseOwnerDefaultWorkingGroup =
                            _workingGroupService.GetWorkingGroup(currentCase.DefaultOwnerWG_Id.Value);
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
                
                field = GetField(userIdValue, cid, languageId,
                    CaseFieldsNamesApi.UserId, GlobalEnums.TranslationCaseFields.User_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings,
                GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer))
            {
                field = GetField(currentCase != null ? currentCase.RegistrationSourceCustomer_Id : new int?(), cid, languageId,
                    CaseFieldsNamesApi.RegistrationSourceCustomer, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                field = GetField(currentCase?.CaseType_Id ?? template.CaseType_Id, cid, languageId,
                    CaseFieldsNamesApi.CaseTypeId, GlobalEnums.TranslationCaseFields.CaseType_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                field = GetField(currentCase != null ? currentCase.ProductArea_Id : template.ProductArea_Id, cid, languageId,
                    CaseFieldsNamesApi.ProductAreaId, GlobalEnums.TranslationCaseFields.ProductArea_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.System_Id))
            {
                field = GetField(currentCase != null ? currentCase.System_Id : template.System_Id, cid, languageId,
                    CaseFieldsNamesApi.SystemId, GlobalEnums.TranslationCaseFields.System_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Urgency_Id))
            {
                field = GetField(currentCase != null ? currentCase.Urgency_Id : template.Urgency_Id, cid, languageId,
                    CaseFieldsNamesApi.UrgencyId, GlobalEnums.TranslationCaseFields.Urgency_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Impact_Id))
            {
                field = GetField(currentCase != null ? currentCase.Impact_Id : template.Impact_Id, cid, languageId,
                    CaseFieldsNamesApi.ImpactId, GlobalEnums.TranslationCaseFields.Impact_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Category_Id))
            {
                field = GetField(currentCase != null ? currentCase.Category_Id : template.Category_Id, cid, languageId,
                    CaseFieldsNamesApi.CategoryId, GlobalEnums.TranslationCaseFields.Category_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Supplier_Id))
            {
                var supplierId = currentCase != null ? currentCase.Supplier_Id : template.Supplier_Id;
                //if show Supplier_Id
                field = GetField(supplierId, cid, languageId,
                    CaseFieldsNamesApi.SupplierId, GlobalEnums.TranslationCaseFields.Supplier_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);

                var supplier = supplierId.HasValue
                    ? _supplierService.GetSupplier(supplierId.Value)
                    : null;
                field = GetField(supplier?.Country_Id, cid, languageId,
                    CaseFieldsNamesApi.SupplierCountryId, GlobalEnums.TranslationCaseFields.Supplier_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InvoiceNumber))
            {
                field = GetField(currentCase != null ? currentCase.InvoiceNumber : template.InvoiceNumber, cid, languageId,
                    CaseFieldsNamesApi.InvoiceNumber, GlobalEnums.TranslationCaseFields.InvoiceNumber, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReferenceNumber))
            {
                field = GetField(currentCase != null ? currentCase.ReferenceNumber : template.ReferenceNumber, cid, languageId,
                    CaseFieldsNamesApi.ReferenceNumber, GlobalEnums.TranslationCaseFields.ReferenceNumber, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Caption))
            {
                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Caption,
                    Value = currentCase!= null ? currentCase.Caption : template.Caption,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Caption,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Caption, currentCase?.Id ?? 0, customerUserSetting.CaptionPermission))
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Description))
            {
                var registrationSource = currentCase != null ? currentCase.RegistrationSource : template.RegistrationSource;
                var registrationSourceOptions = (currentCase != null && registrationSource == (int) CaseRegistrationSource.Email)
                    ? currentCase.Mail2Tickets.Where(x => x.Log_Id == null)
                        .GroupBy(x => x.Type)
                        .Select(gr =>
                            new KeyValuePair<string, string>(gr.Key, string.Join(";", gr.Select(x => x.EMailAddress)))).ToList()
                    : null;

                // Registration source: values 
                field = new BaseCaseField<int>()
                {
                    Name =
                        CaseFieldsNamesApi
                            .CaseRegistrationSource, //NOTE: should not be mistaken with another field - RegistrationSourceCustomer!
                    Value = registrationSource ?? 0,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Registration source"),
                    Section = CaseSectionType.CaseInfo,
                    Options = registrationSourceOptions
                };
                model.Fields.Add(field);

                field = GetField(currentCase != null ? currentCase.Description : template.Description, cid, languageId,
                    CaseFieldsNamesApi.Description, GlobalEnums.TranslationCaseFields.Description, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "10000"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Miscellaneous))
            {
                field = GetField(currentCase != null ? currentCase.Miscellaneous : template.Miscellaneous, cid, languageId,
                    CaseFieldsNamesApi.Miscellaneous, GlobalEnums.TranslationCaseFields.Miscellaneous, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "1000"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ContactBeforeAction))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = CaseFieldsNamesApi.ContactBeforeAction,
                    Value = currentCase?.ContactBeforeAction.ToBool() ?? template.ContactBeforeAction.ToBool(),
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings, 
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.ContactBeforeAction, currentCase?.Id ?? 0, customerUserSetting.ContactBeforeActionPermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SMS))
            {
                field = GetField(currentCase?.SMS.ToBool() ?? template.SMS.ToBool(), cid, languageId,
                    CaseFieldsNamesApi.Sms, GlobalEnums.TranslationCaseFields.SMS, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.AgreedDate))
            {
                field = GetField(currentCase != null ? currentCase.AgreedDate : template.AgreedDate, cid, languageId,
                    CaseFieldsNamesApi.AgreedDate, GlobalEnums.TranslationCaseFields.AgreedDate, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Available))
            {
                field = GetField(currentCase != null ? currentCase.Available : template.Available, cid, languageId,
                    CaseFieldsNamesApi.Available, GlobalEnums.TranslationCaseFields.Available, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Cost))
            {
                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost,
                    Value = currentCase?.Cost ?? template.Cost,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Artikelkostnad"), //Kostnad
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = CaseFieldsNamesApi.Cost_OtherCost,
                    Value = currentCase?.OtherCost ?? template.OtherCost,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = CaseFieldsNamesApi.Cost_Currency,
                    Value = currentCase != null ? currentCase.Currency : template.Currency,
                    Label = _caseTranslationService.TranslateFieldLabel(languageId, "Valuta"),
                    Section = CaseSectionType.CaseInfo,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if ( /*customerSettings.AttachmentPlacement == 1 &&*/
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Filename))
            {
                field = GetField(currentCase != null ? GetCaseFilesModel(currentCase.Id) : null, cid, languageId,
                    CaseFieldsNamesApi.Filename, GlobalEnums.TranslationCaseFields.Filename, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }
        }

        private void CreateCaseManagementSection(int cid, IList<CaseFieldSetting> caseFieldSettings, Case currentCase, CaseSolution template, int languageId,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model, CustomerUser customerUserSetting,
            CustomerSettings customerSettings)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            IBaseCaseField field;
            //CaseManagement
            //displayCaseManagementInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                field = GetField(currentCase != null ? currentCase.WorkingGroup_Id : template.WorkingGroup_Id, cid, languageId,
                    CaseFieldsNamesApi.WorkingGroupId, GlobalEnums.TranslationCaseFields.WorkingGroup_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
            {
                field = GetField(currentCase?.CaseResponsibleUser_Id, cid, languageId,
                    CaseFieldsNamesApi.CaseResponsibleUserId, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
            {
                field = GetField(currentCase != null ? currentCase.Performer_User_Id : template.PerformerUser_Id, cid, languageId,
                    CaseFieldsNamesApi.PerformerUserId, GlobalEnums.TranslationCaseFields.Performer_User_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            //customerUserSetting.PriorityPermission - if 0 - readonly, else 
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.PriorityId,
                    Value = currentCase != null ? currentCase.Priority_Id : template.Priority_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Priority_Id, languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.Priority_Id, currentCase?.Id ?? 0, customerUserSetting.PriorityPermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Status_Id))
            {
                field = GetField(currentCase != null ? currentCase.Status_Id : template.Status_Id, cid, languageId,
                    CaseFieldsNamesApi.StatusId, GlobalEnums.TranslationCaseFields.Status_Id, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.StateSecondaryId,
                    Value = currentCase != null ? currentCase.StateSecondary_Id : template.StateSecondary_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.StateSecondary_Id, currentCase?.Id ?? 0,
                            customerUserSetting.StateSecondaryPermission))
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Project))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = CaseFieldsNamesApi.Project,
                    Value = currentCase != null ? currentCase.Project_Id : template.Project_Id,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.Project, languageId, cid,
                        caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem &&
                _caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Problem))
            {
                field = GetField(currentCase != null ? currentCase.Problem_Id : template.Problem_Id, cid, languageId,
                    CaseFieldsNamesApi.Problem, GlobalEnums.TranslationCaseFields.Problem, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CausingPart))
            {
                field = GetField(currentCase != null ? currentCase.CausingPartId : template.CausingPartId, cid, languageId,
                    CaseFieldsNamesApi.CausingPart, GlobalEnums.TranslationCaseFields.CausingPart, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Change)
            ) //TODO: && Model.changes.Any()
            {
                field = GetField(currentCase != null ? currentCase.Change_Id : template.Change_Id, cid, languageId,
                    CaseFieldsNamesApi.Change, GlobalEnums.TranslationCaseFields.Change, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.PlanDate))
            {
                field = GetField(currentCase != null ?
                        currentCase.PlanDate.HasValue ? DateTime.SpecifyKind(currentCase.PlanDate.Value, DateTimeKind.Utc) : currentCase.PlanDate :
                        template.PlanDate.HasValue ? DateTime.SpecifyKind(template.PlanDate.Value, DateTimeKind.Utc) : template.PlanDate,
                    cid, languageId,
                    CaseFieldsNamesApi.PlanDate, GlobalEnums.TranslationCaseFields.PlanDate, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WatchDate))
            {
                var watchDate = currentCase != null ?
                        currentCase.WatchDate.HasValue ? DateTime.SpecifyKind(currentCase.WatchDate.Value, DateTimeKind.Utc) : currentCase.WatchDate :
                        template.WatchDate.HasValue ? DateTime.SpecifyKind(template.WatchDate.Value, DateTimeKind.Utc) : template.WatchDate;
                if (!watchDate.HasValue && template != null && template.Department_Id.HasValue && template.Priority_Id.HasValue)
                {
                    var dept = _departmentService.GetDepartment(template.Department_Id.Value);
                    var priority = _priorityService.GetPriority(template.Priority_Id.Value);
                    if (dept?.WatchDateCalendar_Id != null && priority != null && priority.IsActive.ToBool() && priority.SolutionTime == 0)
                    {
                        watchDate =
                            _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
                    }
                }

                field = new BaseCaseField<DateTime?>()
                {
                    Name = CaseFieldsNamesApi.WatchDate,
                    Value = watchDate,
                    Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.WatchDate,
                        languageId, cid, caseFieldTranslations),
                    Section = CaseSectionType.CaseManagement,
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings,
                        _caseFieldSettingsHelper.IsReadOnly(GlobalEnums.TranslationCaseFields.WatchDate, currentCase?.Id ?? 0,
                            customerUserSetting.WatchDatePermission))
                };
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Verified))
            {
                field = GetField(currentCase?.Verified.ToBool() ?? template.Verified.ToBool(), cid, languageId,
                    CaseFieldsNamesApi.Verified, GlobalEnums.TranslationCaseFields.Verified, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.VerifiedDescription))
            {
                field = GetField(currentCase != null ? currentCase.VerifiedDescription : template.VerifiedDescription, cid, languageId,
                        CaseFieldsNamesApi.VerifiedDescription, GlobalEnums.TranslationCaseFields.VerifiedDescription, CaseSectionType.CaseInfo,
                        caseFieldSettings, caseFieldTranslations);
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "200"));
                model.Fields.Add(field);
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SolutionRate))
            {
                field = GetField(currentCase != null ? currentCase.SolutionRate : template.SolutionRate, cid, languageId,
                    CaseFieldsNamesApi.SolutionRate, GlobalEnums.TranslationCaseFields.SolutionRate, CaseSectionType.CaseInfo,
                    caseFieldSettings, caseFieldTranslations);
                model.Fields.Add(field);
            }
        }

        private void CreateCommunicationSection(int cid, UserOverview userOverview, IList<CaseFieldSetting> caseFieldSettings, Case currentCase, CaseSolution template,
            int languageId, IList<CaseFieldSettingsForTranslation> caseFieldTranslations, CaseEditOutputModel model)
        {
            if (template == null && currentCase == null)
                throw new Exception("No case or template data provided.");

            IBaseCaseField field;
            if (userOverview.CloseCasePermission == 1)
            {
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings,
                    GlobalEnums.TranslationCaseFields.FinishingDescription))
                {
                    field = GetField(currentCase != null ? currentCase.FinishingDescription : template.FinishingDescription, cid, languageId,
                        CaseFieldsNamesApi.FinishingDescription, GlobalEnums.TranslationCaseFields.FinishingDescription, CaseSectionType.Communication,
                        caseFieldSettings, caseFieldTranslations);
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "200"));
                    model.Fields.Add(field);
                }

                int? finishingCause = null;
                var lastLog = currentCase?.Logs?.FirstOrDefault(); //todo: check if its correct - order
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
                        Value = currentCase != null ? currentCase.FinishingDate : template.FinishingDate,
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDate,
                            languageId, cid, caseFieldTranslations),
                        Section = CaseSectionType.Communication,
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDate, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings,
                    GlobalEnums.TranslationCaseFields.tblLog_Text_External))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.Log_ExternalText,
                        Value = "",
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_External,
                            languageId, cid, caseFieldTranslations),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_External, caseFieldSettings),
                        Section = CaseSectionType.Communication
                    };
                    model.Fields.Add(field);
                }

                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings,
                    GlobalEnums.TranslationCaseFields.tblLog_Text_Internal))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = CaseFieldsNamesApi.Log_InternalText,
                        Value = "",
                        Label = _caseTranslationService.GetFieldLabel(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal,
                            languageId, cid, caseFieldTranslations),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, caseFieldSettings),
                        Section = CaseSectionType.Communication
                    };
                    model.Fields.Add(field);
                }
            }
        }

        private BaseCaseField<T> GetField<T>(T value, int cid, int languageId,
            string caseFieldNameApi,
            GlobalEnums.TranslationCaseFields translationCaseFieldName,
            CaseSectionType sectionName,
            IList<CaseFieldSetting> caseFieldSettings,
            IList<CaseFieldSettingsForTranslation> caseFieldTranslations)
        {
            return new BaseCaseField<T>()
            {
                Name = caseFieldNameApi,
                Value = value,
                Label = _caseTranslationService.GetFieldLabel(translationCaseFieldName, languageId,
                    cid, caseFieldTranslations),
                Section = sectionName,
                Options = GetFieldOptions(translationCaseFieldName, caseFieldSettings)
            };
        }
        
        private IList<CaseFileModel> GetCaseFilesModel(int caseId)
        {
            return _caseFileService.GetCaseFiles(caseId, true);
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