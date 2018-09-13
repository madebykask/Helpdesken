using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;

namespace DH.Helpdesk.WebApi.Controllers
{
    
    public class CaseController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IUserService _userSerivice;

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService, IMailTemplateService mailTemplateService, IUserService userSerivice)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _mailTemplateService = mailTemplateService;
            _userSerivice = userSerivice;
        }

        [HttpGet]
        [UserCasePermissions]
        public async Task<CaseEditOutputModel> Get([FromUri] GetCaseInputModel input)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var currentCase = await Task.FromResult(_caseService.GetCaseById(input.CaseId));
            var currentCustomerId = input.CustomerId;
            var userGroupId = User.Identity.GetGroupId();
            if(currentCase.Customer_Id != currentCustomerId) 
                throw new Exception($"Case customer({currentCase.Customer_Id}) and current customer({currentCustomerId}) are different");//TODO: how to react?

            var userOverview = await Task.FromResult(_userSerivice.GetUserOverview(UserId));//TODO: use cahced version
            var caseFieldSettings = await Task.FromResult(_caseFieldSettingService.GetCaseFieldSettings(input.CustomerId));
            //LockCase(id, userId, true, activeTab);//TODO: Mark as locked

            //TODO: Move to mapper
            model.BackUrl = "";//TODO
            //model.CanGetRelatedCases = userGroupId > (int)UserGroup.User;//TODO: Move to helper extension
            //model.CurrentUserRole = userGroupId;//is really required?
            //model.CaseLock = caseLocked;//TODO: lock implementation
            //model.CaseUnlockAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseUnlockPermission);//TODO: lock implementation
            //model.MailTemplates = _mailTemplateService.GetCustomMailTemplatesList(currentCase.Customer_Id).ToList();//TODO:

            model.Id = currentCase.Id;
            model.CustomerId = currentCase.Customer_Id;
            model.AgreedDate = currentCase.AgreedDate;
            model.ApprovedByUserId = currentCase.ApprovedBy_User_Id;
            model.ApprovedDate = currentCase.ApprovedDate;
            model.Available = currentCase.Available;
            model.Caption = currentCase.Caption;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userOverview.TimeZoneId);
            model.ChangeTime = TimeZoneInfo.ConvertTimeFromUtc(currentCase.ChangeTime, userTimeZone);
            model.CaseCleanUpId = currentCase.CaseCleanUp_Id;
            model.CaseGuid = currentCase.CaseGUID;
            model.CaseNumber = currentCase.CaseNumber;
            model.CaseResponsibleUserId = currentCase.CaseResponsibleUser_Id;
            model.ReportedBy = currentCase.ReportedBy;
            model.PersonsName = currentCase.PersonsName;
            model.PersonsEmail = currentCase.PersonsEmail;
            model.PersonsPhone = currentCase.PersonsPhone;
            model.PersonsCellphone = currentCase.PersonsCellphone;
            model.RegionId = currentCase.Region_Id;
            model.DepartmentId = currentCase.Department_Id;
            model.OuId = currentCase.OU_Id;
            model.CostCentre = currentCase.CostCentre;
            model.Place = currentCase.Place;
            model.UserCode = currentCase.UserCode;
            model.InventoryNumber = currentCase.InventoryNumber;
            model.ProductAreaId = currentCase.ProductArea_Id;
            model.InventoryLocation = currentCase.InventoryLocation;
            model.UserId = currentCase.User_Id;
            model.IpAddress = currentCase.IpAddress;
            model.CaseTypeId = currentCase.CaseType_Id;
            model.ProductAreaSetDate = currentCase.ProductAreaSetDate;
            model.SupplierId = currentCase.Supplier_Id;
            model.UrgencyId = currentCase.Urgency_Id;
            model.ImpactId = currentCase.Impact_Id;
            model.CategoryId = currentCase.Category_Id;
            model.InvoiceNumber = currentCase.InvoiceNumber;
            model.ReferenceNumber = currentCase.ReferenceNumber;
            model.Description = currentCase.Description;
            model.Miscellaneous = currentCase.Miscellaneous;
            model.ContactBeforeAction = currentCase.ContactBeforeAction;
            model.Sms = currentCase.SMS;
            model.Cost = currentCase.Cost;
            model.OtherCost = currentCase.OtherCost;
            model.Currency = currentCase.Currency;
            model.SystemId = currentCase.System_Id;
            model.PerformerUserId = currentCase.Performer_User_Id;
            model.PriorityId = currentCase.Priority_Id;
            model.StatusId = currentCase.Status_Id;
            model.StateSecondaryId = currentCase.StateSecondary_Id;
            model.ExternalTime = currentCase.ExternalTime;
            model.ProjectId = currentCase.Project_Id;
            model.Verified = currentCase.Verified;
            model.VerifiedDescription = currentCase.VerifiedDescription;
            model.SolutionRate = currentCase.SolutionRate;
            model.PlanDate = currentCase.PlanDate;
            model.WatchDate = currentCase.WatchDate;
            model.LockCaseToWorkingGroupId = currentCase.LockCaseToWorkingGroup_Id;
            model.WorkingGroupId = currentCase.WorkingGroup_Id;
            model.CaseSolutionId = currentCase.CaseSolution_Id;
            model.CurrentCaseSolutionId = currentCase.CurrentCaseSolution_Id;
            model.FinishingDate = currentCase.FinishingDate;
            model.FinishingDescription = currentCase.FinishingDescription;
            model.FollowUpDate = currentCase.FollowUpDate;
            model.RegistrationSource = currentCase.RegistrationSource;
            model.RegistrationSourceCustomerId = currentCase.RegistrationSourceCustomer_Id;
            model.InventoryType = currentCase.InventoryType;
            model.RelatedCaseNumber = currentCase.RelatedCaseNumber;
            model.ProblemId = currentCase.Problem_Id;
            //model.Deleted = currentCase.Deleted;
            model.RegLanguageId = currentCase.RegLanguage_Id;
            model.RegUserId = currentCase.RegUserId;
            model.RegUserName = currentCase.RegUserName;
            model.RegUserDomain = currentCase.RegUserDomain;
            model.ProductAreaQuestionVersionId = currentCase.ProductAreaQuestionVersion_Id;
            model.LeadTime = currentCase.LeadTime;
            model.RegTime = currentCase.RegTime;
            model.ChangeByUserId = currentCase.ChangeByUser_Id;
            model.DefaultOwnerWgId = currentCase.DefaultOwnerWG_Id;
            model.CausingPartId = currentCase.CausingPartId;
            model.ChangeId = currentCase.Change_Id;
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

        public async Task<CaseEditOutputModel> New()
        {
            var model = new CaseEditOutputModel();
            //TODO:
            return await Task.FromResult(model);
        }
    }
}