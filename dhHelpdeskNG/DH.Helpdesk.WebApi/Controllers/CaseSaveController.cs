using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Common.Extensions.GUID;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.WebApi.Models.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/case")]
    public class CaseSaveController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseLockService _caseLockService;
        private readonly ICustomerService _customerService;
        private readonly ISettingService _customerSettingsService;
        private readonly ICaseEditModeCalcStrategy _caseEditModeCalcStrategy;
        private readonly IUserService _userService;
        private readonly ISettingsLogic _settingsLogic;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IEmailService _emailService;
        private readonly ICaseFileService _caseFileService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ILogFileService _logFileService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ILogService _logService;
        private readonly ITemporaryFilesCache _userTempFilesStorage;
        private readonly IBaseCaseSolutionService _caseSolutionService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IHolidayService _holidayService;
        private readonly ICaseStatisticService _caseStatService;
		private readonly IFileViewLogService _fileViewLogService;
		private readonly IFeatureToggleService _featureToggleService;
        private readonly IFilesStorage _filesStorage;

		public CaseSaveController(ICaseService caseService,
            ICaseLockService caseLockService, ICustomerService customerService, ISettingService customerSettingsService,
            ICaseEditModeCalcStrategy caseEditModeCalcStrategy, IUserService userService, ISettingsLogic settingsLogic,
            IWorkingGroupService workingGroupService, IEmailService emailService,
            ICaseFileService caseFileService, ICustomerUserService customerUserService, ILogFileService logFileService,
            IUserPermissionsChecker userPermissionsChecker, ILogService logService, ITemporaryFilesCache userTempFilesStorage,
            IBaseCaseSolutionService caseSolutionService, IStateSecondaryService stateSecondaryService,
            IHolidayService holidayService, ICaseStatisticService caseStatService,
			IFileViewLogService fileViewLogService,
			IFeatureToggleService featureToggleService, IFilesStorage filesStorage)
        {
            _caseService = caseService;
            _caseLockService = caseLockService;
            _customerService = customerService;
            _customerSettingsService = customerSettingsService;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
            _userService = userService;
            _settingsLogic = settingsLogic;
            _workingGroupService = workingGroupService;
            _emailService = emailService;
            _caseFileService = caseFileService;
            _customerUserService = customerUserService;
            _logFileService = logFileService;
            _userPermissionsChecker = userPermissionsChecker;
            _logService = logService;
            _userTempFilesStorage = userTempFilesStorage;
            _caseSolutionService = caseSolutionService;
            _stateSecondaryService = stateSecondaryService;
            _holidayService = holidayService;
            _caseStatService = caseStatService;
			_fileViewLogService = fileViewLogService;
			_featureToggleService = featureToggleService;
            _filesStorage = filesStorage;
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
        [Route("save/{caseId:int=0}")]
        public async Task<IHttpActionResult> Save([FromUri] int? caseId, [FromUri]int cid, [FromUri] int langId, [FromBody]CaseEditInputModel model)
        {
            var utcNow = DateTime.UtcNow;
            var customerId = cid;
            var caseKey = caseId.HasValue && caseId > 0 ? caseId.Value.ToString() : model.CaseGuid?.ToString();

            var isEdit = caseId.HasValue && caseId.Value > 0;
            var oldCase = isEdit ? await _caseService.GetDetachedCaseByIdAsync(caseId.Value) : new Case();
            if (isEdit)
            {
                //todo: to be removed when case switching is implemented on mobile 
                if (oldCase.Customer_Id != customerId)
                    throw new Exception($"Case customer({oldCase.Customer_Id}) and current customer({customerId}) are different");

                var lockData = await _caseLockService.GetCaseLockAsync(caseId.Value);
                if (lockData != null && lockData.UserId != UserId)
                {
                    return BadRequest($"Case is locked by {lockData.User.UserID}.");
                }

                var editMode = _caseEditModeCalcStrategy.CalcEditMode(oldCase.Customer_Id, UserId, oldCase);
                if (editMode != AccessMode.FullAccess)
                {
                    return BadRequest($"No permission to edit case {caseId}.");
                }
            }

            var customerUserSetting = await _customerUserService.GetCustomerUserSettingsAsync(customerId, UserId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{customerId}' and user '{UserId}'");

            //TODO: validate input -- check for ui validation rules (max length and etc.)

            // ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings = null;
            var userOverview = await _userService.GetUserOverviewAsync(UserId);

            Case currentCase;
            if (!isEdit)
            {
                if (!userOverview.CreateCasePermission.ToBool())
                    SendResponse($"User {UserName} is not allowed to create case.", HttpStatusCode.Forbidden);

                var caseSolution = model.CaseSolutionId.HasValue && model.CaseSolutionId > 0
                    ? _caseSolutionService.GetCaseSolution(model.CaseSolutionId.Value)
                    : null;

                //var customerDefaults = _customerService.GetCustomerDefaults(customerId);
                currentCase = CreateCase(customerId, langId, caseSolution, utcNow);

                if (model.CaseGuid.HasValue)
                    currentCase.CaseGUID = model.CaseGuid.Value;
            }
            else
            {
                currentCase = _caseService.GetDetachedCaseById(caseId.Value);
            }

            #region Initiator
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                currentCase.ReportedBy = model.ReportedBy;
                currentCase.PersonsName = model.PersonName;
                currentCase.PersonsEmail = model.PersonEmail;
                currentCase.PersonsPhone = model.PersonPhone;
                currentCase.PersonsCellphone = model.PersonCellPhone;
                currentCase.Region_Id = model.RegionId;
                currentCase.Department_Id = model.DepartmentId;
                currentCase.OU_Id = model.OrganizationUnitId;
                currentCase.CostCentre = model.CostCentre;
                currentCase.Place = model.Place;
                currentCase.UserCode = model.UserCode;
            }
            #endregion

            #region Regarding
            if (currentCase.IsAbout == null) currentCase.IsAbout = new CaseIsAboutEntity() { Id = currentCase.Id };

            currentCase.IsAbout.ReportedBy = model.IsAbout_ReportedBy;
            currentCase.IsAbout.Person_Name = model.IsAbout_PersonName;
            currentCase.IsAbout.Person_Email = model.IsAbout_PersonEmail;
            currentCase.IsAbout.Person_Phone = model.IsAbout_PersonPhone;
            currentCase.IsAbout.Person_Cellphone = model.IsAbout_PersonCellPhone;
            currentCase.IsAbout.Region_Id = model.IsAbout_RegionId;
            currentCase.IsAbout.Department_Id = model.IsAbout_DepartmentId;
            currentCase.IsAbout.OU_Id = model.IsAbout_OrganizationUnitId;
            currentCase.IsAbout.CostCentre = model.IsAbout_CostCentre;
            currentCase.IsAbout.Place = model.IsAbout_Place;
            currentCase.IsAbout.UserCode = model.IsAbout_UserCode;
            #endregion

            #region ComputerInfo
            currentCase.InventoryNumber = model.InventoryNumber;
            currentCase.InventoryType = model.ComputerTypeId;
            currentCase.InventoryLocation = model.InventoryLocation;
            #endregion

            #region CaseInfo

            currentCase.ChangeTime = DateTime.UtcNow;
            currentCase.RegistrationSourceCustomer_Id = model.RegistrationSourceCustomerId;
            currentCase.CaseType_Id = model.CaseTypeId;// Reqiured
            currentCase.ProductArea_Id = model.ProductAreaId;
            currentCase.System_Id = model.SystemId;
            currentCase.Urgency_Id = model.UrgencyId;
            currentCase.Impact_Id = model.ImpactId;
            currentCase.Category_Id = model.CategoryId;
            currentCase.Supplier_Id = model.SupplierId;
            currentCase.InvoiceNumber = model.InvoiceNumber;
            currentCase.ReferenceNumber = model.ReferenceNumber;
            currentCase.Miscellaneous = model.Miscellaneous;
            currentCase.Caption = model.Caption;
            currentCase.Description = model.Description;
            currentCase.ContactBeforeAction = model.ContactBeforeAction.ToInt();
            currentCase.SMS = model.Sms.ToInt();
            currentCase.AgreedDate = model.AgreedDate;
            currentCase.Available = model.Available;
            currentCase.Cost = model.Cost;
            currentCase.OtherCost = model.OtherCost;
            currentCase.Currency = model.CostCurrency;
            #endregion

            #region CaseManagement
            currentCase.WorkingGroup_Id = model.WorkingGroupId;
            currentCase.CaseResponsibleUser_Id = model.ResponsibleUserId;
            currentCase.Performer_User_Id = model.PerformerId;
            currentCase.Priority_Id = model.PriorityId;
            currentCase.Status_Id = model.StatusId;
            currentCase.StateSecondary_Id = model.StateSecondaryId;
            currentCase.Project_Id = model.ProjectId;
            currentCase.Problem_Id = model.ProblemId;
            currentCase.CausingPartId = model.CausingPartId;
            currentCase.Change_Id = model.ChangeId;
            currentCase.PlanDate = model.PlanDate;
            currentCase.WatchDate = model.WatchDate;
            currentCase.Verified = model.Verified.ToInt();
            currentCase.VerifiedDescription = model.VerifiedDescription;
            currentCase.SolutionRate = model.SolutionRate;
            #endregion

            #region Status / Close case
            DateTime? caseLogFinishingDate = null;
            var isCaseGoingToFinish = model.ClosingReason.HasValue && model.ClosingReason.Value > 0 &&
                                   userOverview.CloseCasePermission.ToBool();
            if (isCaseGoingToFinish)
            {
                if (!model.FinishingDate.HasValue)
                {
                    caseLogFinishingDate = utcNow;
                }
                else if (model.FinishingDate.Value.ToShortDateString() == DateTime.UtcNow.Date.ToShortDateString())
                {
                    caseLogFinishingDate = utcNow;
                }
                else if (oldCase != null && oldCase.ChangeTime.ToShortDateString() == model.FinishingDate.Value.ToShortDateString())
                {
                    var lastChangedTime = new DateTime(oldCase.ChangeTime.Year, oldCase.ChangeTime.Month, oldCase.ChangeTime.Day, 22, 59, 59);
                    caseLogFinishingDate = lastChangedTime;
                }
                else
                {
                    caseLogFinishingDate = DateTime.SpecifyKind(model.FinishingDate.Value, DateTimeKind.Local).ToUniversalTime();
                }
                currentCase.FinishingDate = DatesHelper.Max(currentCase.RegTime, caseLogFinishingDate.Value);
                currentCase.FinishingDescription = model.FinishingDescription;
            }

            //if (isNew)
            //{
            //    var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(currentCase.User_Id.Value, currentCase.Customer_Id);
            //    if (userDefaultWorkingGroupId.HasValue)
            //    {
            //        currentCase.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
            //    }
            //}
            #endregion

            var currentUser = _userService.GetUser(UserId);
            var customerSettings = await _customerSettingsService.GetCustomerSettingsAsync(customerId);
            var basePath = _settingsLogic.GetFilePath(customerSettings);
            var customer = await _customerService.GetCustomerAsync(customerId);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimezoneId());

            #region Working time calculations - TODO: move to service and reuse in helpdesk

            var statistics = ActionExternalTime(currentCase, customer, currentUser, utcNow, isEdit, customerSettings, oldCase, isCaseGoingToFinish);

            #endregion // End Working time calculations - TODO: move to service

            var extraInfo = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.WebApi,
                LeadTimeForNow = currentCase.LeadTime,
                ActionLeadTime = statistics.ActionLeadTime,
                ActionExternalTime = statistics.ActionExternalTime
            };

            currentCase.LatestSLACountDate = _caseStatService.CalculateLatestSLACountDate(oldCase?.StateSecondary_Id, model.StateSecondaryId, oldCase?.LatestSLACountDate);

            IDictionary<string, string> errors;

            var caseLog = new CaseLog()
            {
                LogGuid = Guid.NewGuid(), //todo: check usages
                TextInternal = model.LogInternalText,
                EmailRecepientsInternalLogTo = model.LogInternalEmailTo,
                EmailRecepientsInternalLogCc = model.LogInternalEmailCc,
                TextExternal = model.LogExternalText,
                EmailRecepientsExternalLog = string.Empty, // TODO: if AllocateCaseMail from working group = 1 set value
                // in helpdesk Reguser is always empty, but Selfservice users CurrentSystemUser
                // RegUser is only filled in selfservice
                RegUser = string.Empty,
                UserId = UserId,
                SendMailAboutCaseToNotifier = model.LogSendMailToNotifier,
                SendMailAboutCaseToPerformer = model.LogSendMailToPerformer,
                FinishingDate = userOverview.CloseCasePermission.ToBool() ? caseLogFinishingDate : new DateTime?(),
                FinishingType = userOverview.CloseCasePermission.ToBool() ? model.ClosingReason : new int?()
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

            var disableLogFileView = _featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE);
            //case files
            // todo: Check if new cases should be handled here. Save case files for new cases only!
            if (!isEdit)
            {
                var temporaryFiles = _userTempFilesStorage.FindFiles(caseKey, ModuleName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, currentCase.Id, UserId)).ToList();

                var paths = new List<KeyValuePair<CaseFileDto, string>>();
                _caseFileService.AddFiles(newCaseFiles, paths);

                if (!disableLogFileView)
                {
                    foreach (var file in paths)
                    {
                        _fileViewLogService.Log(currentCase.Id, UserId, file.Key.FileName, file.Value, FileViewLogFileSource.WebApi, FileViewLogOperation.Add);
                    }
                }
            }

            #region ExtendedCase sections
            if (isEdit)// TODO: attach Extended case if required
            {
                //if (m.ExtendedInitiatorGUID.HasValue)
                //{
                //    var exData = _caseService.GetExtendedCaseData(m.ExtendedInitiatorGUID.Value);
                //    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);
                //}
                //else
                //{
                //    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);
                //}

                //if (m.ExtendedRegardingGUID.HasValue)
                //{
                //    var exData = _caseService.GetExtendedCaseData(m.ExtendedRegardingGUID.Value);
                //    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);
                //}
                //else
                //{
                //    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);

                //}
            }
            else
            {
                if (!model.ExtendedCaseGuid.IsEmpty())
                {
                    var exData = _caseService.GetExtendedCaseData(model.ExtendedCaseGuid.Value);
                    _caseService.CreateExtendedCaseRelationship(currentCase.Id, exData.Id);
                }
                //if (model.ReportedBy != null && model.ExtendedInitiatorGUID.HasValue)
                //{
                //    var exData = _caseService.GetExtendedCaseData(model.ExtendedInitiatorGUID.Value);
                //    _caseService.CreateExtendedCaseSectionRelationship(Id, exData.Id, CaseSectionType.Initiator, curCustomer.Id);
                //}
                //if (model.IsAbout_ReportedBy != null && model.ExtendedRegardingGUID.HasValue)
                //{
                //    var exData = _caseService.GetExtendedCaseData(model.ExtendedRegardingGUID.Value);
                //    _caseService.CreateExtendedCaseSectionRelationship(currentCase.Id, exData.Id, CaseSectionType.Regarding, curCustomer.Id);
                //}
            }
            #endregion

            #region Folowers
            var followerUsers = new List<string>();
            //if (isEdit) // TODO: Folowers
            //{
            //    if (!string.IsNullOrEmpty(m.FollowerUsers))
            //    {
            //        followerUsers = m.FollowerUsers.Split(BRConstItem.Email_Char_Separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.Trim()).ToList();
            //    }
            //    _caseExtraFollowersService.SaveExtraFollowers(case_.Id, followerUsers, _workContext.User.UserId);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(m.FollowerUsers))
            //    {
            //        followerUsers = m.FollowerUsers.Split(BRConstItem.Email_Char_Separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.Trim()).ToList();
            //        _caseExtraFollowersService.SaveExtraFollowers(case_.Id, followerUsers, _workContext.User.UserId);
            //    }
            //}
            #endregion

            #region Logs Handling

            caseLog.CaseId = currentCase.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            /* #58573 Check that user have access to write to InternalLogNote */
            var caseInternalLogAccess = _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.CaseInternalLogPermission);
            if (!caseInternalLogAccess)
                caseLog.TextInternal = null;

            var temporaryLogFiles = _userTempFilesStorage.FindFiles(caseKey, ModuleName.Log);
            var temporaryLogInternalFiles = _userTempFilesStorage.FindFiles(caseKey, ModuleName.LogInternal);
            var temporaryExLogFiles = _logFileService.GetExistingFileNamesByCaseId(currentCase.Id); // gets all attached existing files
            var logFileCount = temporaryLogFiles.Count + temporaryExLogFiles.Count + temporaryLogInternalFiles.Count;

            // SAVE LOG
            caseLog.Id = _logService.SaveLog(caseLog, logFileCount, out errors);

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseLogFileDto(f.Content, basePath, f.Name, utcNow, caseLog.Id, currentUser.Id, LogFileType.External, null)).ToList();
            if (temporaryLogInternalFiles.Any())
            {
                var internalLogFiles = temporaryLogInternalFiles.Select(f => new CaseLogFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, currentUser.Id, LogFileType.Internal, null)).ToList();
                newLogFiles.AddRange(internalLogFiles);
            }
            var logPaths = new List<KeyValuePair<CaseLogFileDto, string>>();
            _logFileService.AddFiles(newLogFiles, logPaths, temporaryExLogFiles, caseLog.Id);

            var allLogFiles =
                temporaryExLogFiles.Select(f =>
                    new CaseLogFileDto(basePath,
                        f.Name,
                        f.IsExistCaseFile ? Convert.ToInt32(currentCase.CaseNumber) : f.LogId.Value,
                        f.IsExistCaseFile)
                    {
                        LogType = f.IsInternalLogNote ? LogFileType.Internal : LogFileType.External,
                        ParentLogType = f.LogType
                    }).ToList();

            allLogFiles.AddRange(newLogFiles);

            if (!disableLogFileView)
            {
                foreach (var newLogFile in newLogFiles)
                {
                    var path = _filesStorage.ComposeFilePath(newLogFile.LogType.GetFolderPrefix(),
                        caseLog.Id, basePath, "");
                    _fileViewLogService.Log(currentCase.Id, UserId, newLogFile.FileName, path, FileViewLogFileSource.WebApi, FileViewLogOperation.Add);
                }
            }

            #endregion // Logs handling

            // send emails
            var caseMailSetting = GetCaseMailSetting(currentCase, customer, customerSettings);

            _caseService.SendCaseEmail(currentCase.Id, caseMailSetting, caseHistoryId, basePath,
                                       userTimeZone, oldCase, caseLog, allLogFiles, currentUser,
                                       model.LogExternalEmailsCc); //TODO: async or move to scheduler

            // BRE
            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCaseAfter, currentCase, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, currentCase.Id, caseLog, userTimeZone, caseHistoryId,
                                                    basePath, langId, caseMailSetting, allLogFiles); //TODO: async or move to scheduler


            //delete case and logs temp files
            _userTempFilesStorage.ResetCacheForObject(caseKey);

            // TODO: return errors
            return Ok(currentCase.Id);
        }

        private СaseStatisticsResult ActionExternalTime(Case currentCase, Customer customer, User currentUser, DateTime utcNow, bool isEdit,
            CustomerSettings customerSettings, Case oldCase, bool isCaseGoingToFinish)
        {
            var result = new СaseStatisticsResult
            {
                ActionExternalTime = 0,
                ActionLeadTime = 0
            };

            int[] departmentIds = null;
            if (currentCase.Department_Id.HasValue)
                departmentIds = new int[] {currentCase.Department_Id.Value};

            var cs = _customerSettingsService.GetCustomerSetting(customer.Id);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                _holidayService,
                customer.WorkingDayStart,
                customer.WorkingDayEnd,
                timeZone);

            var workTimeCalc = workTimeCalcFactory.Build(currentCase.RegTime, utcNow, departmentIds);

            if (isEdit)
            {
                var oldCaseSubstateCount = true;
                // offset in Minute
                var customerTimeOffset = customerSettings.TimeZoneOffset;

                StateSecondary caseStateSecondary = null;
                if (oldCase.StateSecondary_Id.HasValue)
                    caseStateSecondary = _stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                if (caseStateSecondary != null && caseStateSecondary.IncludeInCaseStatistics == 0)
                {
                    oldCaseSubstateCount = false;
                    var externalTimeToAdd = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        utcNow,
                        currentCase.Department_Id);
                    currentCase.ExternalTime += externalTimeToAdd;

                    //workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, utcNow, departmentIds, customerTimeOffset);
                    result.ActionExternalTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        utcNow,
                        oldCase.Department_Id, customerTimeOffset);
                }

                if (isCaseGoingToFinish &&
                    oldCaseSubstateCount &&
                    currentCase.FinishingDate.HasValue && currentCase.FinishingDate.Value < utcNow)
                {
                    currentCase.ExternalTime +=
                        workTimeCalc.CalculateWorkTime(currentCase.FinishingDate.Value, utcNow, currentCase.Department_Id);
                }

                var oldDeptIds = oldCase.Department_Id.HasValue ? new[] {oldCase.Department_Id.Value} : null;

                var endTime = isCaseGoingToFinish ? currentCase.FinishingDate.Value.ToUniversalTime() : utcNow;
                var oldWorkTimeCalc =
                    workTimeCalcFactory.Build(oldCase.ChangeTime, endTime, oldDeptIds, customerTimeOffset);
                result.ActionLeadTime = oldWorkTimeCalc.CalculateWorkTime(
                                     oldCase.ChangeTime, endTime,
                                     oldCase.Department_Id, customerTimeOffset) - result.ActionExternalTime;
            }

            var possibleWorktime = workTimeCalc.CalculateWorkTime(
                currentCase.RegTime,
                utcNow,
                currentCase.Department_Id);

            currentCase.LeadTime = possibleWorktime - currentCase.ExternalTime;
            return result;
        }

        private Case CreateCase(int cid, int langId, CaseSolution caseSolution, DateTime utcNow)
        {
            var currentCase = new Case
            {
                RegUserId = "",
                RegUserDomain = "",
                RegLanguage_Id = langId,
                RegistrationSource = caseSolution?.RegistrationSource ?? (int) CaseRegistrationSource.Administrator,
                CaseGUID = new Guid(),
                IpAddress = GetClientIp(),
                Customer_Id = cid,
                User_Id = UserId,
                RegTime = utcNow,
                CaseResponsibleUser_Id = UserId,
                CaseSolution_Id = (caseSolution?.Id ?? 0) == 0 ? null : caseSolution?.Id,
                CurrentCaseSolution_Id = caseSolution?.Id
            };
            // adUser.GetUserFromAdPath(),
            // adUser.GetDomainFromAdPath()
            return currentCase;
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

            var caseMailSetting = 
                new CaseMailSetting(
                    customer.NewCaseEmailList,
                    customer.HelpdeskEmail,
                    ConfigurationManager.AppSettings[AppSettingsKey.HelpdeskPath],
                    customerSettings.DontConnectUserToWorkingGroup)
            {
                CustomeMailFromAddress = mailSenders,
                DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool()
            };

            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

            return caseMailSetting;
        }

    }
}


