using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
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
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IEmailService _emailService;
        private readonly ICaseFileService _caseFileService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ILogFileService _logFileService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ILogService _logService;
        private ITemporaryFilesCache _userTempFilesStorage;

        #region ctor()

        public CaseSaveController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            ICaseLockService caseLockService, ICustomerService customerService,
            ISettingService customerSettingsService, ICaseEditModeCalcStrategy caseEditModeCalcStrategy,
            IUserService userService, ISettingsLogic settingsLogic, ICaseFieldSettingsHelper caseFieldSettingsHelper, 
            IWorkingGroupService workingGroupService, IEmailService emailService, ICaseFileService caseFileService,
            ITemporaryFilesCacheFactory temporaryFilesCacheFactory, ILogFileService logFileService,
            ICustomerUserService customerUserService, IUserPermissionsChecker userPermissionsChecker,
            ILogService logService)
        {
            _logService = logService;
            _userPermissionsChecker = userPermissionsChecker;
            _logFileService = logFileService;
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseLockService = caseLockService;
            _customerService = customerService;
            _customerSettingsService = customerSettingsService;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
            _userService = userService;
            _settingsLogic = settingsLogic;
            _caseFieldSettingsHelper = caseFieldSettingsHelper;
            _workingGroupService = workingGroupService;
            _emailService = emailService;
            _caseFileService = caseFileService;
            _userTempFilesStorage = temporaryFilesCacheFactory.CreateForModule(ModuleName.Cases);
            _customerUserService = customerUserService;
        }

        #endregion

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
            var utcNow = DateTime.UtcNow;
            var caseKey = caseId.HasValue && caseId > 0 ? caseId.Value.ToString() : model.CaseGuid;
            var isNew = !caseId.HasValue;
            if (isNew)
            {
                return BadRequest("CaseId is required. Creating new case is not supported.");
            }
            var edit = caseId > 0;
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

            var customerUserSetting = _customerUserService.GetCustomerUserSettings(cid, UserId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{cid}' and user '{UserId}'");


            //TODO: validate input -- apply ui validation rules
            
            var currentCase = _caseService.GetDetachedCaseById(caseId.Value); //TODO: try to Copy/Clone from oldCase instead

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
                currentCase.CaseResponsibleUser_Id = model.ResponsibleUserId;

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
                currentCase.Performer_User_Id = model.PerformerId;

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
            {
                currentCase.WorkingGroup_Id = model.WorkingGroupId;
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                currentCase.StateSecondary_Id = model.StateSecondaryId;
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                currentCase.Priority_Id = model.PriorityId;
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                currentCase.ProductArea_Id = model.ProductAreaId;
            }

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, GlobalEnums.TranslationCaseFields.WatchDate))
            {
                currentCase.WatchDate = model.WatchDate;
            }

            //if (isNew)
            //{
            //    var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(currentCase.User_Id.Value, currentCase.Customer_Id);
            //    if (userDefaultWorkingGroupId.HasValue)
            //    {
            //        currentCase.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
            //    }
            //}

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
            var currentUser = _userService.GetUser(UserId);
            var customerSettings = _customerSettingsService.GetCustomerSettings(oldCase.Customer_Id);
            var basePath = _settingsLogic.GetFilePath(customerSettings);
            var customer = _customerService.GetCustomer(oldCase.Customer_Id);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimezoneId());

            var caseLog = new CaseLog()
            {
                LogGuid = Guid.NewGuid(), //todo: check usages
                TextInternal = model.LogInternalText,
                TextExternal = model.LogExternalText,
                RegUser = currentUser?.UserID ?? string.Empty, // valid for webapi?
                UserId = UserId,
                //todo:
                // FinishingDate = model.FinishingDate
                // FinishingType = model.FinishingType?
            };

            // -> SAVE CASE 
            var caseHistoryId = _caseService.SaveCase(
                currentCase,
                caseLog,
                UserId,
                UserName,
                extraInfo,
                out errors,
                null,  // TODO: Parentcase - if support will be added. Web api dont have Create child/parent functionality
                null); // TODO: FollowerUsers - if support added

            // TODO: caseNotifications?

            //case files
            // todo: Check if new cases should be handled here. Save case files for new cases only!
            if (!edit)
            {
                var temporaryFiles = _userTempFilesStorage.FindFiles(caseKey); //todo: check
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, currentCase.Id, UserId)).ToList();
                _caseFileService.AddFiles(newCaseFiles);
            }

            #region Logs Handling

            caseLog.CaseId = currentCase.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            /* #58573 Check that user have access to write to InternalLogNote */
            var caseInternalLogAccess = _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.CaseInternalLogPermission);
            if (!caseInternalLogAccess)
                caseLog.TextInternal = null;

            var temporaryLogFiles = _userTempFilesStorage.FindFiles(caseKey, ModuleName.Log);
            var temporaryExLogFiles = _logFileService.GetExistingFileNamesByCaseId(currentCase.Id);
            var logFileCount = temporaryLogFiles.Count + temporaryExLogFiles.Count;

            // SAVE LOG
            caseLog.Id = _logService.SaveLog(caseLog, logFileCount, out errors);

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, utcNow, caseLog.Id, currentUser.Id)).ToList();
            _logFileService.AddFiles(newLogFiles, temporaryExLogFiles, caseLog.Id);

            var allLogFiles = temporaryExLogFiles.Select(f => new CaseFileDto(basePath, f.Name, f.IsExistCaseFile ? Convert.ToInt32(currentCase.CaseNumber) : f.LogId.Value, f.IsExistCaseFile)).ToList();
            allLogFiles.AddRange(newLogFiles);

            #endregion // Logs handling

            // send emails
            var caseMailSetting = GetCaseMailSetting(currentCase, customer, customerSettings);
            _caseService.SendCaseEmail(currentCase.Id, caseMailSetting, caseHistoryId, basePath, 
                                       userTimeZone, oldCase, caseLog, allLogFiles, currentUser); //TODO: async or move to scheduler

            // BRE
            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, currentCase, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, currentCase, caseLog, userTimeZone, caseHistoryId, 
                                                    basePath, langId, caseMailSetting, allLogFiles); //TODO: async or move to scheduler


            //delete case and logs temp files
            _userTempFilesStorage.ResetCacheForObject(caseKey);

            // TODO: return errors
            return Ok(oldCase.Id);
        }

        private CaseMailSetting GetCaseMailSetting(Case currentCase, Customer customer, CustomerSettings customerSettings)
        {
            var mailSenders = new MailSenders();
            if (currentCase.WorkingGroup_Id.HasValue)
            {
                var curWg = _workingGroupService.GetWorkingGroup(currentCase.WorkingGroup_Id.Value); 
                if (curWg != null && !string.IsNullOrWhiteSpace(curWg.EMail) && _emailService.IsValidEmail(curWg.EMail))
                    mailSenders.WGEmail = curWg.EMail;
            }

            if (currentCase.DefaultOwnerWG_Id.HasValue && currentCase.DefaultOwnerWG_Id.Value > 0)
            {
                var defaultWgEmail = _workingGroupService.GetWorkingGroup(currentCase.DefaultOwnerWG_Id.Value);//TODO: check if possible to use curWg to reduce requests
                if (defaultWgEmail != null && !string.IsNullOrWhiteSpace(defaultWgEmail.EMail) && _emailService.IsValidEmail(defaultWgEmail.EMail))
                    mailSenders.DefaultOwnerWGEMail = defaultWgEmail.EMail;
            }

            var caseMailSetting = new CaseMailSetting(
                customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                ConfigurationManager.AppSettings[AppSettingsKey.HelpdeskPath],
                customerSettings.DontConnectUserToWorkingGroup);
            caseMailSetting.CustomeMailFromAddress = mailSenders;
            caseMailSetting.DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool();
            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

            return caseMailSetting;
        }

    }
}
