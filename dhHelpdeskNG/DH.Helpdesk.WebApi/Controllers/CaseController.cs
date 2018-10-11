using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Config.Filters;
using DH.Helpdesk.WebApi.Infrastructure.Translate;
using DateTime = System.DateTime;

namespace DH.Helpdesk.WebApi.Controllers
{

    public class CaseController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IUserService _userSerivice;
        private readonly ICustomerUserService _customerUserService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ISupplierService _supplierService;
        private readonly ISettingService _customerSettingsService;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            IMailTemplateService mailTemplateService, IUserService userSerivice, IComputerService computerService,
            ICustomerUserService customerUserService, IUserService userService, IWorkingGroupService workingGroupService,
            ISupplierService supplierService, ISettingService customerSettingsService,
            ITranslateCacheService translateCacheService)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _mailTemplateService = mailTemplateService;
            _userSerivice = userSerivice;
            _customerUserService = customerUserService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _supplierService = supplierService;
            _customerSettingsService = customerSettingsService;
            _translateCacheService = translateCacheService;
        }

        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        public async Task<CaseEditOutputModel> Get([FromUri] GetCaseInputModel input)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var languageId = input.LangId;
            var currentCase = _caseService.GetCaseById(input.CaseId);
            var currentCid = input.Cid;
            var userGroupId = User.Identity.GetGroupId();
            if (currentCase.Customer_Id != currentCid)
                throw new Exception(
                    $"Case customer({currentCase.Customer_Id}) and current customer({currentCid}) are different"); //TODO: how to react?

            var customerUserSetting = _customerUserService.GetCustomerUserSettings(currentCid, userId);
            if (customerUserSetting == null)
            {
                throw new Exception(
                    $"No customer settings for this customer '{currentCid}' and user '{userId}'");
            }

            var customerSettings = _customerSettingsService.GetCustomerSettings(currentCid);
            var userOverview = await _userSerivice.GetUserOverviewAsync(UserId);//TODO: use cahced version
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(currentCid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(currentCid);

            //LockCase(id, userId, true, activeTab);//TODO: Mark as locked
            //model.CanGetRelatedCases = userGroupId > (int)UserGroup.User;//TODO: Move to helper extension
            //model.CurrentUserRole = userGroupId;//is really required?
            //model.CaseLock = caseLocked;//TODO: lock implementation
            //model.CaseUnlockAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseUnlockPermission);//TODO: lock implementation
            //model.MailTemplates = _mailTemplateService.GetCustomMailTemplatesList(currentCase.Customer_Id).ToList();//TODO:

            model.Fields = new List<IBaseCaseField>();

            //TODO: Move to mapper
            model.Id = currentCase.Id;
            model.CaseNumber = currentCase.CaseNumber;
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
                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReportedBy))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.ReportedBy.ToString(),
                        Value = currentCase.ReportedBy,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ReportedBy, languageId, input.Cid, caseFieldTranslations, "Användar ID"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReportedBy, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Name))
                {
                    model.Fields.Add(field);
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),
                        Value = currentCase.PersonsName,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Name, languageId, input.Cid, caseFieldTranslations, "Anmälare"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Name, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_EMail))
                {
                    model.Fields.Add(field);
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(),
                        Value = currentCase.PersonsEmail,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_EMail, languageId, input.Cid, caseFieldTranslations, "E-post"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_EMail, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_Phone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(),
                        Value = currentCase.PersonsPhone,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Phone, languageId, input.Cid, caseFieldTranslations, "Telefon"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Phone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Persons_CellPhone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(),
                        Value = currentCase.PersonsCellphone,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_CellPhone,
                            languageId, input.Cid, caseFieldTranslations, "Mobil"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_CellPhone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Region_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Region_Id.ToString(),
                        Value = currentCase.Region_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Region_Id,
                            languageId, input.Cid, caseFieldTranslations, "Område"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Region_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Department_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Department_Id.ToString(),
                        Value = currentCase.Department_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Department_Id,
                            languageId, input.Cid, caseFieldTranslations, "Avdelning"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.OU_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.OU_Id.ToString(),
                        Value = currentCase.OU_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.OU_Id,
                            languageId, input.Cid, caseFieldTranslations, "Enhet"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.OU_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CostCentre))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.CostCentre.ToString(),
                        Value = currentCase.CostCentre,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CostCentre,
                            languageId, input.Cid, caseFieldTranslations, "Kostnadsställe"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CostCentre, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Place))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.Place.ToString(),
                        Value = currentCase.Place,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Place,
                            languageId, input.Cid, caseFieldTranslations, "Placering"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Place, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.UserCode))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.UserCode.ToString(),
                        Value = currentCase.UserCode,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UserCode,
                            languageId, input.Cid, caseFieldTranslations, "Ansvarskod"),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UserCode, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                //field = new BaseCaseField<bool>()//TODO: for edit
                //{
                //    Name = GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString(),
                //    Value = true,
                //    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, 
                //        languageId, input.Cid, caseFieldTranslations, ""),
                //    Section = CaseSectionType.Initiator.ToString(),
                //    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings//        )
                //};
                //model.Fields.Add(field);



            }

            #endregion

            #region Regarding

            // Regarding
            //displayAboutUserInfoHtml:TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayAboutUserInfoHtml
            if (currentCase.IsAbout != null)
            {
                //if (Model.ComputerUserCategories.Any())
                //GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id //TODO: IsAbout_UserSearchCategory_Id

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(),
                        Value = currentCase.IsAbout.ReportedBy,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                            languageId, input.Cid, caseFieldTranslations, "Användar ID"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(),
                        Value = currentCase.IsAbout.Person_Name,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                            languageId, input.Cid, caseFieldTranslations, "Anmälare"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString(),
                        Value = currentCase.IsAbout.Person_Email,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail,
                            languageId, input.Cid, caseFieldTranslations, "E-post"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString(),
                        Value = currentCase.IsAbout.Person_Phone,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone,
                            languageId, input.Cid, caseFieldTranslations, "Telefon"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString(),
                        Value = currentCase.IsAbout.Person_Cellphone,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone,
                            languageId, input.Cid, caseFieldTranslations, "Mobil"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "30"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString(),
                        Value = currentCase.IsAbout.Region_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                            languageId, input.Cid, caseFieldTranslations, "Område"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString(),
                        Value = currentCase.IsAbout.Department_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                            languageId, input.Cid, caseFieldTranslations, "Avdelning"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString(),
                        Value = currentCase.IsAbout.OU_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id,
                            languageId, input.Cid, caseFieldTranslations, "Enhet"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString(),
                        Value = currentCase.IsAbout.CostCentre,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                            languageId, input.Cid, caseFieldTranslations, "Kostnadsställe"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Place))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString(),
                        Value = currentCase.IsAbout.Place,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Place,
                            languageId, input.Cid, caseFieldTranslations, "Placering"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Place, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_UserCode))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString(),
                        Value = currentCase.IsAbout.UserCode,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_UserCode,
                            languageId, input.Cid, caseFieldTranslations, "Ansvarskod"),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, caseFieldSettings)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

            }

            #endregion

            #region ComputerInfo

            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(),
                    Value = currentCase.InventoryNumber,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryNumber, languageId, input.Cid, caseFieldTranslations, "PC Nummer"),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "60"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ComputerType_Id))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString(),
                    Value = currentCase.InventoryType,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ComputerType_Id, languageId, input.Cid, caseFieldTranslations, "Datortyp beskrivning"),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ComputerType_Id, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryLocation))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.InventoryLocation.ToString(),
                    Value = currentCase.InventoryLocation,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryLocation, languageId, input.Cid, caseFieldTranslations, "Placering"),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryLocation, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            #endregion

            #region CaseInfo

            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseNumber.ToString(),
                    Value = currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseNumber,
                        languageId, input.Cid, caseFieldTranslations, "Ärende"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseNumber, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = GlobalEnums.TranslationCaseFields.RegTime.ToString(),
                    Value = currentCase.RegTime,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.RegTime,
                        languageId, input.Cid, caseFieldTranslations, "Registreringsdatum"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegTime, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ChangeTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                    Value = currentCase.ChangeTime,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ChangeTime,
                        languageId, input.Cid, caseFieldTranslations, "Ändringsdatum"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ChangeTime, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.User_Id))
            {
                var userIdValue = "";
                if (currentCase.User_Id.HasValue)
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

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.User_Id.ToString(),
                    Value = userIdValue,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.User_Id,
                        languageId, input.Cid, caseFieldTranslations, "Registrerad av"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.User_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString(),
                    Value = currentCase.RegistrationSourceCustomer_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer,

                        languageId, input.Cid, caseFieldTranslations, "Källa"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                    Value = currentCase.CaseType_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseType_Id,
                        languageId, input.Cid, caseFieldTranslations, "Ärendetyp"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseType_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                    Value = currentCase.ProductArea_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ProductArea_Id,
                        languageId, input.Cid, caseFieldTranslations, "Produktområde"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ProductArea_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.System_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.System_Id.ToString(),
                    Value = currentCase.System_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.System_Id,
                        languageId, input.Cid, caseFieldTranslations, "System"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.System_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Urgency_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Urgency_Id.ToString(),
                    Value = currentCase.Urgency_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Urgency_Id,
                        languageId, input.Cid, caseFieldTranslations, "Brådskandegrad"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Urgency_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Impact_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Impact_Id.ToString(),
                    Value = currentCase.Impact_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Impact_Id,
                        languageId, input.Cid, caseFieldTranslations, "Påverkan"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Impact_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Category_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                    Value = currentCase.Category_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Category_Id,
                        languageId, input.Cid, caseFieldTranslations, "Kategori"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Category_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Supplier_Id))
            {
                var supplier = currentCase.Supplier_Id.HasValue
                    ? _supplierService.GetSupplier(currentCase.Supplier_Id.Value)
                    : null;
                //if show Supplier_Id
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                    Value = currentCase.Supplier_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id,
                        languageId, input.Cid, caseFieldTranslations, "Leverantör"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings)
                };
                model.Fields.Add(field);

                field = new BaseCaseField<int?>()
                {
                    Name = "Supplier_Country_Id", //TODO: Naming
                    Value = supplier?.Country_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id,
                        languageId, input.Cid, caseFieldTranslations, "Leverantör"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InvoiceNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString(),
                    Value = currentCase.InvoiceNumber,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InvoiceNumber,
                        languageId, input.Cid, caseFieldTranslations, "Fakturanummer"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InvoiceNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ReferenceNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(),
                    Value = currentCase.ReferenceNumber,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ReferenceNumber,
                        languageId, input.Cid, caseFieldTranslations, "Referensnummer"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReferenceNumber, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Caption))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Caption.ToString(),
                    Value = currentCase.Caption,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Caption,
                        languageId, input.Cid, caseFieldTranslations, "Rubrik"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Description))
            {
                if (currentCase.RegistrationSource == 3)
                {
                    //TODO: Mail2ticket field data
                }

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Description.ToString(),
                    Value = currentCase.Description,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Description,
                        languageId, input.Cid, caseFieldTranslations, "Beskrivning"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Description, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "10000"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Miscellaneous))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Miscellaneous.ToString(),
                    Value = currentCase.Miscellaneous,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Miscellaneous,
                        languageId, input.Cid, caseFieldTranslations, "Övrigt"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Miscellaneous, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "1000"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ContactBeforeAction))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(),
                    Value = currentCase.ContactBeforeAction.ToBool(),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ContactBeforeAction,
                        languageId, input.Cid, caseFieldTranslations, "Telefonkontakt"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SMS))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = GlobalEnums.TranslationCaseFields.SMS.ToString(),
                    Value = currentCase.SMS.ToBool(),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.SMS,
                        languageId, input.Cid, caseFieldTranslations, "SMS"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SMS, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.AgreedDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.AgreedDate.ToString(),
                    Value = currentCase.AgreedDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.AgreedDate,
                        languageId, input.Cid, caseFieldTranslations, "Överenskommet datum"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.AgreedDate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Available))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Available.ToString(),
                    Value = currentCase.Available,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Available,
                        languageId, input.Cid, caseFieldTranslations, "Anträffbar"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Available, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Cost))
            {

                field = new BaseCaseField<int>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Cost.ToString(),
                    Value = currentCase.Cost,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Cost,
                        languageId, input.Cid, caseFieldTranslations, "Kostnad"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = "Cost_OtherCost",
                    Value = currentCase.OtherCost,
                    Label = Translation.Get("Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);


                field = new BaseCaseField<string>()
                {
                    Name = "Cost_Currency",
                    Value = currentCase.Currency,
                    Label = Translation.Get("Valuta"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            #endregion

            #region CaseManagement

            //CaseManagement
            //displayCaseManagementInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                    Value = currentCase.WorkingGroup_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.WorkingGroup_Id,
                        languageId, input.Cid, caseFieldTranslations, "Driftgrupp"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(),
                    Value = currentCase.CaseResponsibleUser_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id,
                        languageId, input.Cid, caseFieldTranslations, "Ansvarig"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(),
                    Value = currentCase.Performer_User_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Performer_User_Id,
                        languageId, input.Cid, caseFieldTranslations, "Handläggare"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Performer_User_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            //customerUserSetting.PriorityPermission - if 0 - readonly, else 
            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                    Value = currentCase.Priority_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Priority_Id,
                        languageId, input.Cid, caseFieldTranslations, "Prioritet"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings,
                        !IsCaseNew(currentCase.Id) && !customerUserSetting.PriorityPermission.ToBool())
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Status_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                    Value = currentCase.Status_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Status_Id,
                        languageId, input.Cid, caseFieldTranslations, "Status"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Status_Id, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                    Value = currentCase.StateSecondary_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                        languageId, input.Cid, caseFieldTranslations, "Understatus"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings,
                        !IsCaseNew(currentCase.Id) && !customerUserSetting.StateSecondaryPermission.ToBool())
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject &&
                IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Project))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Project.ToString(),
                    Value = currentCase.Project_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Project,
                        languageId, input.Cid, caseFieldTranslations, "Projekt"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem &&
                IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Problem))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Problem.ToString(),
                    Value = currentCase.Problem_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Problem,
                        languageId, input.Cid, caseFieldTranslations, "ITIL Problem"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Problem, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CausingPart))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CausingPart.ToString(),
                    Value = currentCase.CausingPartId,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CausingPart,
                        languageId, input.Cid, caseFieldTranslations, "Rotorsak"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CausingPart, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Change)) //TODO: && Model.changes.Any()
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Change.ToString(),
                    Value = currentCase.Change_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Change,
                        languageId, input.Cid, caseFieldTranslations, "Ändring"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Change, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.PlanDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.PlanDate.ToString(),
                    Value = currentCase.PlanDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.PlanDate,
                        languageId, input.Cid, caseFieldTranslations, "Planerad åtgärdsdatum"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.PlanDate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WatchDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.WatchDate.ToString(),
                    Value = currentCase.WatchDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.WatchDate,
                        languageId, input.Cid, caseFieldTranslations, "Bevakningsdatum"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Verified))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Verified.ToString(),
                    Value = currentCase.Verified.ToBool(),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Verified,
                        languageId, input.Cid, caseFieldTranslations, "Verifierad"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Verified, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.VerifiedDescription))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                    Value = currentCase.VerifiedDescription,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.VerifiedDescription,

                        languageId, input.Cid, caseFieldTranslations, "Verifierad beskrivning"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.VerifiedDescription, caseFieldSettings)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "200"));
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.SolutionRate))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.SolutionRate.ToString(),
                    Value = currentCase.SolutionRate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.SolutionRate,
                        languageId, input.Cid, caseFieldTranslations, "Lösningsgrad"),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SolutionRate, caseFieldSettings)
                };
                model.Fields.Add(field);
            }

            #endregion

            #region Communication Management

            if (userOverview.CloseCasePermission == 1)
            {

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.FinishingDescription))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.FinishingDescription.ToString(),
                        Value = currentCase.FinishingDescription,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDescription,

                            languageId, input.Cid, caseFieldTranslations, "Avslutsbeskrivning"),
                        Section = CaseSectionType.CaseManagement.ToString(),
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
                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ClosingReason))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.ClosingReason.ToString(),
                        Value = finishingCause,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ClosingReason,

                            languageId, input.Cid, caseFieldTranslations, "Avslutsorsak"),
                        Section = CaseSectionType.CaseManagement.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ClosingReason, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.FinishingDate))
                {
                    field = new BaseCaseField<DateTime?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.FinishingDate.ToString(),
                        Value = currentCase.FinishingDate,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.FinishingDate,
                            languageId, input.Cid, caseFieldTranslations, "Avslutsdatum"),
                        Section = CaseSectionType.CaseManagement.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.FinishingDate, caseFieldSettings)
                    };
                    model.Fields.Add(field);
                }

            }

            #endregion

            return await Task.FromResult(model);
        }

        private bool IsCaseNew(int currentCaseId)
        {
            return currentCaseId < 0;
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
            var setting = GetCaseFieldSetting(caseFieldSettings, fieldName);
            //var settingEx = caseFieldSettingsTranslated.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
            if (setting != null && setting.Required.ToBool())
            {
                options.Add(new KeyValuePair<string, string>("required", "true"));
            }

            if (readOnly.HasValue)
            {
                options.Add(new KeyValuePair<string, string>("readonly", readOnly.Value.ToJavaScriptBool()));
            }
            //if (settingEx != null && !string.IsNullOrWhiteSpace(settingEx.FieldHelp))
            //{
            //    options.Add(new KeyValuePair<string, string>("description", settingEx.FieldHelp));
            //}
            //TODO: check is readonly
            return options;
        }

        private string GetFieldLabel(GlobalEnums.TranslationCaseFields field, int languageId, int customerId, 
                                    IList<CaseFieldSettingsForTranslation> caseFieldSettingsForTranslations, 
                                    string defaultCaption = "")
        {
            var caption = defaultCaption;
            var fieldName = field.ToString();

            var settingEx = caseFieldSettingsForTranslations.FirstOrDefault(x => x.Name.ToLower() == fieldName.Replace("tblLog_", "tblLog.").ToLower() && x.Language_Id == languageId);
            if (!string.IsNullOrWhiteSpace(settingEx?.Label))
            {
                caption = settingEx.Label;
            }
            else 
            {
                var translateByText = string.Empty;
                //var instanceWord = Translation.GetInstanceWord(translate); // todo: check if required - see Translation.cs\CaseTranslation
                //if (!string.IsNullOrEmpty(instanceWord))
                var translation = _translateCacheService.GetTextTranslations().FirstOrDefault(x => string.Compare(x.TextToTranslate, caption, StringComparison.OrdinalIgnoreCase) == 0);
                if (translation != null)
                {
                    var trans = translation.TextTranslations.FirstOrDefault(x => x.Language_Id == languageId);
                    translateByText = trans?.TextTranslated;
                    if (!string.IsNullOrEmpty(translateByText))
                        caption = translateByText;
                }

                if (string.IsNullOrEmpty(translateByText))
                    caption = caption.GetDefaultValue(languageId);
            }

            return caption;
        }

        private static bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields field)
        {
            return GetCaseFieldSetting(caseFieldSettings, field.ToString())?.IsActive ?? false;
        }

        private static CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName)
        {
            return caseFieldSettings.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        }


    }
}