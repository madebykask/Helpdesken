using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.Object;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;

namespace DH.Helpdesk.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/case")]
    public class CaseSaveController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseLockService _caseLockService;
        private readonly ICustomerService _customerService;
        private readonly ISettingService _customerSettingsService;
        private readonly ICaseEditModeCalcStrategy _caseEditModeCalcStrategy;
        private readonly IUserService _userService;
        private readonly ISettingsLogic _settingsLogic;
        private readonly ICaseFieldSettingsHelper _caseFieldSettingsHelper;

        public CaseSaveController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            ICaseLockService caseLockService, ICustomerService customerService,
            ISettingService customerSettingsService, ICaseEditModeCalcStrategy caseEditModeCalcStrategy,
            IUserService userService, ISettingsLogic settingsLogic, ICaseFieldSettingsHelper caseFieldSettingsHelper)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseLockService = caseLockService;
            _customerService = customerService;
            _customerSettingsService = customerSettingsService;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
            _userService = userService;
            _settingsLogic = settingsLogic;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
        }

        /// <summary>
        /// Save Case
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="cid"></param>
        /// <param name="langId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("save/{caseId:int}")]
        public async Task<IHttpActionResult> Post([FromUri] int? caseId, [FromUri]int cid, [FromUri] int langId, [FromBody]CaseEditInputModel model)
        {
            var isNew = !caseId.HasValue;
            if (isNew)
            {
                return BadRequest("CaseId is required. Creating new case is not supported.");
            }

            var lockData = await _caseLockService.GetCaseLockAsync(caseId.Value);
            if (lockData != null && lockData.UserId != UserId)
            {
                return BadRequest($"Case is locked by {lockData.User.UserID}.");
            }

            var oldCase = _caseService.GetDetachedCaseById(caseId.Value);
            var editMode = _caseEditModeCalcStrategy.CalcEditMode(oldCase.Customer_Id, UserId, oldCase);
            if (editMode != AccessMode.FullAccess)
            {
                return BadRequest($"No permission to edit case {caseId}.");
            }

            //TODO: validate input -- apply ui validation rules

            var utcNow = DateTime.UtcNow;
            var mailSenders = new MailSenders();
            var currentCase = _caseService.GetDetachedCaseById(caseId.Value); //TODO: try to Copy/Clone from oldCase instead

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
                currentCase.CaseResponsibleUser_Id = model.ResponsibleUserId;

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
                currentCase.Performer_User_Id = model.PerformerId;

            //if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            //{
            //    if (model.WorkingGroupId.HasValue)
            //    {
            //        var curWg = _workingGroupService.GetWorkingGroup(model.WorkingGroupId.Value);
            //        if (curWg != null)
            //            if (!string.IsNullOrWhiteSpace(curWg.EMail) && _emailService.IsValidEmail(curWg.EMail))
            //                mailSenders.WGEmail = curWg.EMail;
            //    }
            //}

            //if (case_.DefaultOwnerWG_Id.HasValue && case_.DefaultOwnerWG_Id.Value > 0)
            //{
            //var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(m.case_.User_Id.Value, m.case_.Customer_Id);
            //if (userDefaultWorkingGroupId.HasValue)
            //{
            //    m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
            //}
            //    var defaultWGEmail = _workingGroupService.GetWorkingGroup(case_.DefaultOwnerWG_Id.Value).EMail;
            //    mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
            //}

            CaseLog caseLog = null; // TODO: implement

            var leadTime = 0;// TODO: add calculation
            var actionLeadTime = 0;// TODO: add calculation
            var actionExternalTime = 0;// TODO: add calculation

            var extraInfo = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.WebApi,
                LeadTimeForNow = leadTime,
                ActionLeadTime = actionLeadTime,
                ActionExternalTime = actionExternalTime
            };

            IDictionary<string, string> errors;
            var caseHistoryId = this._caseService.SaveCase(
                currentCase,
                caseLog,
                UserId,
                UserName,
                extraInfo,
                out errors,
                null,  // TODO: Parentcase - if support will be added. Web api dont have Create child/parent functionality
                null); // TODO: FollowerUsers - if support added

            // TODO: caseNotifications?

            // TODO: LOG - if support added 

            var customer = _customerService.GetCustomer(oldCase.Customer_Id);
            var customerSettings = _customerSettingsService.GetCustomerSettings(oldCase.Customer_Id);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimezoneId());
            var basePath = _settingsLogic.GetFilePath(customerSettings);
            var allLogFiles = new List<CaseFileDto>();// TODO: Temparary no files. implement after files upload if needed.

            var caseMailSetting = new CaseMailSetting(
                customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                "",// TODO: get client url or helpdesk url?
                customerSettings.DontConnectUserToWorkingGroup);
            caseMailSetting.CustomeMailFromAddress = mailSenders;
            caseMailSetting.DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool();

            var currentUser = _userService.GetUser(UserId);

            // send emails
            _caseService.SendCaseEmail(oldCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, allLogFiles, currentUser); //TODO: async or move to scheduler

            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, currentCase, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, currentCase, caseLog, userTimeZone, caseHistoryId, basePath, langId,
                    caseMailSetting, allLogFiles); //TODO: async or move to scheduler

            // TODO: return errors
            return Ok(oldCase.Id);
        }
    }
}
