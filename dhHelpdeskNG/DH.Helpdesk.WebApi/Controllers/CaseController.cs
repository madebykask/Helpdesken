using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;
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
        private readonly IComputerService _computerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ISupplierService _supplierService;
        private readonly ISettingService _customerSettingsService;

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            IMailTemplateService mailTemplateService, IUserService userSerivice, IComputerService computerService,
            ICustomerUserService customerUserService, IUserService userService, IWorkingGroupService workingGroupService,
            ISupplierService supplierService, ISettingService customerSettingsService)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _mailTemplateService = mailTemplateService;
            _userSerivice = userSerivice;
            _computerService = computerService;
            _customerUserService = customerUserService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _supplierService = supplierService;
            _customerSettingsService = customerSettingsService;
        }

        [HttpGet]
        [UserCasePermissions]
        public async Task<CaseEditOutputModel> Get([FromUri] GetCaseInputModel input)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var languageId = input.LanguageId;//TODO:
            var currentCase = await Task.FromResult(_caseService.GetCaseById(input.CaseId));
            var currentCid = input.Cid;
            var userGroupId = User.Identity.GetGroupId();
            if (currentCase.Customer_Id != currentCid)
                throw new Exception($"Case customer({currentCase.Customer_Id}) and current customer({currentCid}) are different");//TODO: how to react?

            var customerUserSetting = await Task.FromResult(_customerUserService.GetCustomerUserSettings(currentCid, userId));
            if (customerUserSetting == null)
            {
                throw new Exception(
                    $"No customer settings for this customer '{currentCid}' and user '{userId}'");
            }

            var customerSettings =  await Task.FromResult(_customerSettingsService.GetCustomerSettings(currentCid));
            //var userOverview = await Task.FromResult(_userSerivice.GetUserOverview(UserId));//TODO: use cahced version
            var caseFieldSettings = await Task.FromResult(_caseFieldSettingService.GetCaseFieldSettings(input.Cid));
            var caseFieldSettingsTranslated =
                await Task.FromResult(
                    _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(input.Cid, languageId));//TODO: merge with caseFieldSettings to reduce amount of requests;
            //LockCase(id, userId, true, activeTab);//TODO: Mark as locked

            //TODO: Move to mapper
            model.BackUrl = "";//TODO
            //model.CanGetRelatedCases = userGroupId > (int)UserGroup.User;//TODO: Move to helper extension
            //model.CurrentUserRole = userGroupId;//is really required?
            //model.CaseLock = caseLocked;//TODO: lock implementation
            //model.CaseUnlockAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseUnlockPermission);//TODO: lock implementation
            //model.MailTemplates = _mailTemplateService.GetCustomMailTemplatesList(currentCase.Customer_Id).ToList();//TODO:

            model.Fields = new List<IBaseCaseField>();

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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ReportedBy, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReportedBy, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Name,
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Name, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_EMail,
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_EMail, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Phone,
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Phone, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_CellPhone,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Region_Id, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Region_Id, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.OU_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.OU_Id.ToString(),
                        Value = currentCase.OU_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.OU_Id, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.OU_Id, caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CostCentre))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.CostCentre.ToString(),
                        Value = currentCase.CostCentre,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CostCentre, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CostCentre, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Place, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Place, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UserCode, caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Initiator.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UserCode, caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                //field = new BaseCaseField<bool>()//TODO: for edit
                //{
                //    Name = GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString(),
                //    Value = true,
                //    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettingsTranslated,
                //        languageId, input.Cid),
                //    Section = CaseSectionType.Initiator.ToString(),
                //    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.UpdateNotifierInformation, caseFieldSettings,
                //        caseFieldSettingsTranslated)
                //};
                //model.Fields.Add(field);



            }

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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
                {
                    field = new BaseCaseField<string>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString(),
                        Value = currentCase.IsAbout.Person_Cellphone,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone,
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

                if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
                {
                    field = new BaseCaseField<int?>()
                    {
                        Name = GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString(),
                        Value = currentCase.IsAbout.Region_Id,
                        Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                            caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Place, caseFieldSettings,
                            caseFieldSettingsTranslated)
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
                            caseFieldSettingsTranslated,
                            languageId, input.Cid),
                        Section = CaseSectionType.Regarding.ToString(),
                        Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, caseFieldSettings,
                            caseFieldSettingsTranslated)
                    };
                    field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                    model.Fields.Add(field);
                }

            }

            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.InventoryNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(),
                    Value = currentCase.InventoryNumber,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryNumber,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryNumber, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ComputerType_Id,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ComputerType_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryLocation,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryLocation, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);
            }

            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseNumber))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseNumber.ToString(),
                    Value = currentCase.CaseNumber.ToString(CultureInfo.InvariantCulture),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseNumber, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseNumber, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.RegTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = GlobalEnums.TranslationCaseFields.RegTime.ToString(),
                    Value = currentCase.RegTime,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.RegTime, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegTime, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ChangeTime))
            {
                field = new BaseCaseField<DateTime>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                    Value = currentCase.ChangeTime,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ChangeTime, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ChangeTime, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.User_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.User_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer,
                        caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                    Value = currentCase.CaseType_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseType_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseType_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                    Value = currentCase.CaseType_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ProductArea_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ProductArea_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.System_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.System_Id.ToString(),
                    Value = currentCase.System_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.System_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.System_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Urgency_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Urgency_Id.ToString(),
                    Value = currentCase.Urgency_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Urgency_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Urgency_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Impact_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Impact_Id.ToString(),
                    Value = currentCase.Impact_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Impact_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Impact_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Category_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                    Value = currentCase.Category_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Category_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Category_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Supplier_Id))
            {
                var supplier = currentCase.Supplier_Id.HasValue ? _supplierService.GetSupplier(currentCase.Supplier_Id.Value) : null;
                //if show Supplier_Id
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                    Value = currentCase.Supplier_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);

                field = new BaseCaseField<int?>()
                {
                    Name = "Supplier_Country_Id", //TODO: Naming
                    Value = supplier?.Country_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Supplier_Id,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Supplier_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InvoiceNumber, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ReferenceNumber, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Caption, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Description, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Miscellaneous, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ContactBeforeAction, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SMS, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.AgreedDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.AgreedDate.ToString(),
                    Value = currentCase.AgreedDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.AgreedDate, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.AgreedDate, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Available))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Available.ToString(),
                    Value = currentCase.Available,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Available, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Available, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);

                field = new BaseCaseField<int>()
                {
                    Name = "Cost_OtherCost",
                    Value = currentCase.OtherCost,
                    Label = Translation.Get("Övrig kostnad"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "7"));
                model.Fields.Add(field);


                field = new BaseCaseField<string>()
                {
                    Name = "Cost_Currency",
                    Value = currentCase.Currency,
                    Label = Translation.Get("Valuta"),
                    Section = CaseSectionType.CaseInfo.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Cost, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            //CaseManagement
            //displayCaseManagementInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                    Value = currentCase.WorkingGroup_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(),
                    Value = currentCase.CaseResponsibleUser_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(),
                    Value = currentCase.Performer_User_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Performer_User_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Performer_User_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }
            
            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                    Value = currentCase.Priority_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Priority_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Status_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                    Value = currentCase.Status_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Status_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Status_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                    Value = currentCase.StateSecondary_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.StateSecondary_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProject && IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Project))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Project.ToString(),
                    Value = currentCase.Project_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Project, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Project, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (customerSettings.ModuleProblem && IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Problem))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Problem.ToString(),
                    Value = currentCase.Problem_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Problem, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Problem, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CausingPart))
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.CausingPart.ToString(),
                    Value = currentCase.CausingPartId,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.CausingPart, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.CausingPart, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Change)) //TODO: && Model.changes.Any()
            {
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Change.ToString(),
                    Value = currentCase.Change_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Change, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Change, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.PlanDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.PlanDate.ToString(),
                    Value = currentCase.PlanDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.PlanDate, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.PlanDate, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WatchDate))
            {
                field = new BaseCaseField<DateTime?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.WatchDate.ToString(),
                    Value = currentCase.WatchDate,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.WatchDate, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Verified))
            {
                field = new BaseCaseField<bool>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Verified.ToString(),
                    Value = currentCase.Verified.ToBool(),
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Verified, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Verified, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            if (IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.VerifiedDescription))
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                    Value = currentCase.VerifiedDescription,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.VerifiedDescription, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.VerifiedDescription, caseFieldSettings,
                        caseFieldSettingsTranslated)
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
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.SolutionRate, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.CaseManagement.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.SolutionRate, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
            }

            return await Task.FromResult(model);
        }


        public async Task<CaseEditOutputModel> New()
        {
            var model = new CaseEditOutputModel();
            //TODO:
            return await Task.FromResult(model);
        }

        private List<KeyValuePair<string, string>> GetFieldOptions(GlobalEnums.TranslationCaseFields field, IList<CaseFieldSetting> caseFieldSettings,
            IList<CaseFieldSettingsWithLanguage> caseFieldSettingsTranslated)
        {
            var options = new List<KeyValuePair<string, string>>();
            var fieldName = field.ToString();

            //TODO: Move Replace("tblLog_", "tblLog.") to extension
            var setting = GetCaseFieldSetting(caseFieldSettings, fieldName);
            var settingEx = caseFieldSettingsTranslated.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
            if (setting != null && setting.Required.ToBool())
            {
                options.Add(new KeyValuePair<string, string>("required", "true"));
            }
            if (settingEx != null && !string.IsNullOrWhiteSpace(settingEx.FieldHelp))
            {
                options.Add(new KeyValuePair<string, string>("description", settingEx.FieldHelp));
            }
            //TODO: check is readonly
            return options;
        }

        private string GetFieldLabel(GlobalEnums.TranslationCaseFields field, IList<CaseFieldSettingsWithLanguage> caseFieldSettingsTranslated,
            int languageId, int Cid, string defaultCaption = "")
        {
            var caption = "";
            var fieldName = field.ToString();

            var settingEx = caseFieldSettingsTranslated.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
            if (settingEx != null && !string.IsNullOrWhiteSpace(settingEx.Label))
            {
                caption = settingEx.Label;
            }
            else
            {
                if (languageId == 0) //TODO: translation
                    caption = Translation.Get(fieldName); //, Enums.TranslationSource.CaseTranslation, Cid);
                else
                    caption = Translation
                        .Get(fieldName); //, languageId, Enums.TranslationSource.CaseTranslation, Cid);

                if (string.IsNullOrEmpty(caption) && defaultCaption != "")
                    caption = Translation.Get(
                        defaultCaption); //, languageId, Enums.TranslationSource.TextTranslation, Cid);
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