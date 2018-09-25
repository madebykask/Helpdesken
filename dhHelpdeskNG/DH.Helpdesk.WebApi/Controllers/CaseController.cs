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

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            IMailTemplateService mailTemplateService, IUserService userSerivice,
            IComputerService computerService, ICustomerUserService customerUserService)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _mailTemplateService = mailTemplateService;
            _userSerivice = userSerivice;
            _computerService = computerService;
            _customerUserService = customerUserService;
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

            var userOverview = await Task.FromResult(_userSerivice.GetUserOverview(UserId));//TODO: use cahced version
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

                model.Fields.Add(field);
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),
                    Value = currentCase.PersonsName,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Name, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Name, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));

                model.Fields.Add(field);
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(),
                    Value = currentCase.PersonsEmail,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_EMail, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_EMail, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));

                model.Fields.Add(field);
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(),
                    Value = currentCase.PersonsPhone,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_Phone, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_Phone, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(),
                    Value = currentCase.PersonsCellphone,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Persons_CellPhone, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Persons_CellPhone, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

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

                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.Department_Id.ToString(),
                    Value = currentCase.Department_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);

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

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(),
                    Value = currentCase.IsAbout.ReportedBy,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "40"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(),
                    Value = currentCase.IsAbout.Person_Name,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString(),
                    Value = currentCase.IsAbout.Person_Email,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString(),
                    Value = currentCase.IsAbout.Person_Cellphone,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString(),
                    Value = currentCase.IsAbout.Region_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
                
                field = new BaseCaseField<int?>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString(),
                    Value = currentCase.IsAbout.Department_Id,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
                
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
                
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString(),
                    Value = currentCase.IsAbout.CostCentre,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre,
                        caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString(),
                    Value = currentCase.IsAbout.Place,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_Place, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_Place, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
                model.Fields.Add(field);

                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString(),
                    Value = currentCase.IsAbout.UserCode,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, caseFieldSettingsTranslated,
                        languageId, input.Cid),
                    Section = CaseSectionType.Regarding.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
                model.Fields.Add(field);
                
            }

            // ComputerInfo
            //displayComputerInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions.displayComputerInfoHtml
            field = new BaseCaseField<string>()
            {
                Name = GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(),
                Value = currentCase.InventoryNumber,
                Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryNumber, caseFieldSettingsTranslated,
                    languageId, input.Cid),
                Section = CaseSectionType.Regarding.ToString(),
                Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryNumber, caseFieldSettings,
                    caseFieldSettingsTranslated)
            };
            field.Options.Add(new KeyValuePair<string, string>("maxlength", "60"));
            model.Fields.Add(field);

            field = new BaseCaseField<string>()
            {
                Name = GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString(),
                Value = currentCase.InventoryType,
                Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ComputerType_Id, caseFieldSettingsTranslated,
                    languageId, input.Cid),
                Section = CaseSectionType.Regarding.ToString(),
                Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.ComputerType_Id, caseFieldSettings,
                    caseFieldSettingsTranslated)
            };
            field.Options.Add(new KeyValuePair<string, string>("maxlength", "50"));
            model.Fields.Add(field);

            field = new BaseCaseField<string>()
            {
                Name = GlobalEnums.TranslationCaseFields.InventoryLocation.ToString(),
                Value = currentCase.InventoryLocation,
                Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.InventoryLocation, caseFieldSettingsTranslated,
                    languageId, input.Cid),
                Section = CaseSectionType.Regarding.ToString(),
                Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.InventoryLocation, caseFieldSettings,
                    caseFieldSettingsTranslated)
            };
            field.Options.Add(new KeyValuePair<string, string>("maxlength", "100"));
            model.Fields.Add(field);

            // CaseInfo
            //displayCaseInfoHtml //TODO:see DH.Helpdesk.Web.Infrastructure.Extensions.ObjectExtensions
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

           
            var userHasInvoicePermission = false;
            //this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InvoicePermission); //TODO:

            //model.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);//TODO:




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

        private static CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName)
        {
            return caseFieldSettings.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        }


    }
}