using System;
using System.Collections.Generic;
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
            var currentCustomerId = input.CustomerId;
            var userGroupId = User.Identity.GetGroupId();
            if(currentCase.Customer_Id != currentCustomerId) 
                throw new Exception($"Case customer({currentCase.Customer_Id}) and current customer({currentCustomerId}) are different");//TODO: how to react?

            var customerUserSetting = await Task.FromResult(_customerUserService.GetCustomerUserSettings(currentCustomerId, userId));
            if (customerUserSetting == null)
            {
                throw new Exception(
                    $"No customer settings for this customer '{currentCustomerId}' and user '{userId}'");
            }

            var userOverview = await Task.FromResult(_userSerivice.GetUserOverview(UserId));//TODO: use cahced version
            var caseFieldSettings = await Task.FromResult(_caseFieldSettingService.GetCaseFieldSettings(input.CustomerId));
            var caseFieldSettingsTranslated =
                await Task.FromResult(
                    _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(input.CustomerId, languageId));//TODO: merge with caseFieldSettings to reduce amount of requests;
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
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                field = new BaseCaseField<string>()
                {
                    Name = GlobalEnums.TranslationCaseFields.ReportedBy.ToString(),
                    Value = currentCase.ReportedBy,
                    Label = GetFieldLabel(GlobalEnums.TranslationCaseFields.ReportedBy, caseFieldSettingsTranslated,
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
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
                        languageId, input.CustomerId),
                    Section = CaseSectionType.Initiator.ToString(),
                    Options = GetFieldOptions(GlobalEnums.TranslationCaseFields.Department_Id, caseFieldSettings,
                        caseFieldSettingsTranslated)
                };
                model.Fields.Add(field);
                
            }

            //model.CustomerId = currentCase.Customer_Id;
            //model.AgreedDate = currentCase.AgreedDate;
            //model.ApprovedByUserId = currentCase.ApprovedBy_User_Id;
            //model.ApprovedDate = currentCase.ApprovedDate;
            //model.Available = currentCase.Available;
            //model.Caption = currentCase.Caption;
            //var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userOverview.TimeZoneId);
            //model.ChangeTime = TimeZoneInfo.ConvertTimeFromUtc(currentCase.ChangeTime, userTimeZone);
            //model.CaseCleanUpId = currentCase.CaseCleanUp_Id;
            //model.CaseGuid = currentCase.CaseGUID;
            
            //model.CaseResponsibleUserId = currentCase.CaseResponsibleUser_Id;
            ////model.ReportedBy = currentCase.ReportedBy;
            //model.PersonsName = currentCase.PersonsName;
            //model.PersonsEmail = currentCase.PersonsEmail;
            //model.PersonsPhone = currentCase.PersonsPhone;
            //model.PersonsCellphone = currentCase.PersonsCellphone;
            //model.RegionId = currentCase.Region_Id;
            //model.DepartmentId = currentCase.Department_Id;
            //model.OuId = currentCase.OU_Id;
            //model.CostCentre = currentCase.CostCentre;
            //model.Place = currentCase.Place;
            //model.UserCode = currentCase.UserCode;
            //model.InventoryNumber = currentCase.InventoryNumber;
            //model.ProductAreaId = currentCase.ProductArea_Id;
            //model.InventoryLocation = currentCase.InventoryLocation;
            //model.UserId = currentCase.User_Id;
            //model.IpAddress = currentCase.IpAddress;
            //model.CaseTypeId = currentCase.CaseType_Id;
            //model.ProductAreaSetDate = currentCase.ProductAreaSetDate;
            //model.SupplierId = currentCase.Supplier_Id;
            //model.UrgencyId = currentCase.Urgency_Id;
            //model.ImpactId = currentCase.Impact_Id;
            //model.CategoryId = currentCase.Category_Id;
            //model.InvoiceNumber = currentCase.InvoiceNumber;
            //model.ReferenceNumber = currentCase.ReferenceNumber;
            //model.Description = currentCase.Description;
            //model.Miscellaneous = currentCase.Miscellaneous;
            //model.ContactBeforeAction = currentCase.ContactBeforeAction;
            //model.Sms = currentCase.SMS;
            //model.Cost = currentCase.Cost;
            //model.OtherCost = currentCase.OtherCost;
            //model.Currency = currentCase.Currency;
            //model.SystemId = currentCase.System_Id;
            //model.PerformerUserId = currentCase.Performer_User_Id;
            //model.PriorityId = currentCase.Priority_Id;
            //model.StatusId = currentCase.Status_Id;
            //model.StateSecondaryId = currentCase.StateSecondary_Id;
            //model.ExternalTime = currentCase.ExternalTime;
            //model.ProjectId = currentCase.Project_Id;
            //model.Verified = currentCase.Verified;
            //model.VerifiedDescription = currentCase.VerifiedDescription;
            //model.SolutionRate = currentCase.SolutionRate;
            //model.PlanDate = currentCase.PlanDate;
            //model.WatchDate = currentCase.WatchDate;
            //model.LockCaseToWorkingGroupId = currentCase.LockCaseToWorkingGroup_Id;
            //model.WorkingGroupId = currentCase.WorkingGroup_Id;
            //model.CaseSolutionId = currentCase.CaseSolution_Id;
            //model.CurrentCaseSolutionId = currentCase.CurrentCaseSolution_Id;
            //model.FinishingDate = currentCase.FinishingDate;
            //model.FinishingDescription = currentCase.FinishingDescription;
            //model.FollowUpDate = currentCase.FollowUpDate;
            //model.RegistrationSource = currentCase.RegistrationSource;
            //model.RegistrationSourceCustomerId = currentCase.RegistrationSourceCustomer_Id;
            //model.InventoryType = currentCase.InventoryType;
            //model.RelatedCaseNumber = currentCase.RelatedCaseNumber;
            //model.ProblemId = currentCase.Problem_Id;
            ////model.Deleted = currentCase.Deleted;
            //model.RegLanguageId = currentCase.RegLanguage_Id;
            //model.RegUserId = currentCase.RegUserId;
            //model.RegUserName = currentCase.RegUserName;
            //model.RegUserDomain = currentCase.RegUserDomain;
            //model.ProductAreaQuestionVersionId = currentCase.ProductAreaQuestionVersion_Id;
            //model.LeadTime = currentCase.LeadTime;
            //model.RegTime = currentCase.RegTime;
            //model.ChangeByUserId = currentCase.ChangeByUser_Id;
            //model.DefaultOwnerWgId = currentCase.DefaultOwnerWG_Id;
            //model.CausingPartId = currentCase.CausingPartId;
            //model.ChangeId = currentCase.Change_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;
            //model.UserId = currentCase.User_Id;



            var userHasInvoicePermission = false;
            //this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InvoicePermission); //TODO:

            //model.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);//TODO:
            
            


            return await Task.FromResult(model);
        }

        private List<KeyValuePair<string, string>> GetFieldOptions(GlobalEnums.TranslationCaseFields field, IList<CaseFieldSetting> caseFieldSettings,
            IList<CaseFieldSettingsWithLanguage> caseFieldSettingsTranslated)
        {
            var options = new List<KeyValuePair<string, string>>();
            var fieldName = field.ToString();

            //TODO: Move Replace("tblLog_", "tblLog.") to extension
            var setting = caseFieldSettings.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)); 
            var settingEx = caseFieldSettingsTranslated.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
            if (setting != null && setting.Required.ToBool())
            {
                options.Add(new KeyValuePair<string, string>("required", "true"));
            }
            if(settingEx != null && !string.IsNullOrWhiteSpace(settingEx.FieldHelp))
            {
                options.Add(new KeyValuePair<string, string>("description", settingEx.FieldHelp));
            }
            //TODO: check is readonly
            return options;
        }

        private string GetFieldLabel(GlobalEnums.TranslationCaseFields field, IList<CaseFieldSettingsWithLanguage> caseFieldSettingsTranslated,
            int languageId, int customerId, string defaultCaption = "")
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
                    caption = Translation.Get(fieldName); //, Enums.TranslationSource.CaseTranslation, customerId);
                else
                    caption = Translation
                        .Get(fieldName); //, languageId, Enums.TranslationSource.CaseTranslation, customerId);

                if (string.IsNullOrEmpty(caption) && defaultCaption != "")
                    caption = Translation.Get(
                        defaultCaption); //, languageId, Enums.TranslationSource.TextTranslation, customerId);
            }

            return caption;
        }

        public static string getCaseFieldName(string value)
        {
            return value.Replace("tblLog_", "tblLog.");
        }

        public async Task<CaseEditOutputModel> New()
        {
            var model = new CaseEditOutputModel();
            //TODO:
            return await Task.FromResult(model);
        }
    }
}