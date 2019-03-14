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
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Extensions.Boolean;
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
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Services.Infrastructure;

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
        private readonly ITemporaryFilesCache _userTempFilesStorage;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly IBaseCaseSolutionService _caseSolutionService;
		private readonly IStateSecondaryService _stateSecondaryService;

        public CaseSaveController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            ICaseLockService caseLockService, ICustomerService customerService, ISettingService customerSettingsService,
            ICaseEditModeCalcStrategy caseEditModeCalcStrategy, IUserService userService, ISettingsLogic settingsLogic,
            ICaseFieldSettingsHelper caseFieldSettingsHelper, IWorkingGroupService workingGroupService, IEmailService emailService,
            ICaseFileService caseFileService, ICustomerUserService customerUserService, ILogFileService logFileService,
            IUserPermissionsChecker userPermissionsChecker, ILogService logService, ITemporaryFilesCache userTempFilesStorage,
            ICaseSolutionSettingService caseSolutionSettingService, IBaseCaseSolutionService caseSolutionService, IStateSecondaryService stateSecondaryService)
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
            _workingGroupService = workingGroupService;
            _emailService = emailService;
            _caseFileService = caseFileService;
            _customerUserService = customerUserService;
            _logFileService = logFileService;
            _userPermissionsChecker = userPermissionsChecker;
            _logService = logService;
            _userTempFilesStorage = userTempFilesStorage;
            _caseSolutionSettingService = caseSolutionSettingService;
            _caseSolutionService = caseSolutionService;
			_stateSecondaryService = stateSecondaryService;
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
        public async Task<IHttpActionResult> Post([FromUri] int? caseId, [FromUri]int cid, [FromUri] int langId, [FromBody]CaseEditInputModel model)
        {
            var utcNow = DateTime.UtcNow;
            var caseKey = caseId.HasValue && caseId > 0 ? caseId.Value.ToString() : model.CaseGuid?.ToString();

            var isEdit = caseId.HasValue && caseId.Value > 0;
            var oldCase = isEdit ? _caseService.GetDetachedCaseById(caseId.Value) : new Case();
            if (isEdit)
            {
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

            var customerUserSetting = await _customerUserService.GetCustomerUserSettingsAsync(cid, UserId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{cid}' and user '{UserId}'");

            //TODO: validate input -- check for ui validation rules (max length and etc.)

            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings = null;
            Case currentCase;
            if(!isEdit)
            {
                var userOverview = await _userService.GetUserOverviewAsync(UserId);
                if (!userOverview.CreateCasePermission.ToBool())
                    SendResponse($"User {UserName} is not allowed to create case.", HttpStatusCode.Forbidden);

                CaseSolution caseSolution = null; 
                if (model.CaseSolutionId.HasValue && model.CaseSolutionId > 0)
                    caseSolution = _caseSolutionService.GetCaseSolution(model.CaseSolutionId.Value);
                    
                var customerDefaults = _customerService.GetCustomerDefaults(cid);
                currentCase = new Case();

                ApplyTemplateOrDefaultValues(cid, langId, currentCase, caseSolution, customerDefaults, userOverview);
                if (model.CaseGuid.HasValue)
                    currentCase.CaseGUID = model.CaseGuid.Value;

                if (model.CaseSolutionId.HasValue && model.CaseSolutionId > 0) //caseTemplateSettings are only needed when creating a new case from template
                    caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(model.CaseSolutionId.Value);
            }
            else
            {
                currentCase = _caseService.GetDetachedCaseById(caseId.Value);
            }

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);

            #region Initiator
            if (customerUserSetting.UserInfoPermission.ToBool())
            {
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ReportedBy))
                    currentCase.ReportedBy = model.ReportedBy;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Persons_Name))
                    currentCase.PersonsName = model.PersonName;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Persons_EMail))
                    currentCase.PersonsEmail = model.PersonEmail;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Persons_Phone))
                    currentCase.PersonsPhone = model.PersonPhone;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Persons_CellPhone))
                    currentCase.PersonsCellphone = model.PersonCellPhone;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Region_Id))
                    currentCase.Region_Id = model.RegionId;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Department_Id))
                    currentCase.Department_Id = model.DepartmentId;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.OU_Id))
                    currentCase.OU_Id = model.OrganizationUnitId;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.CostCentre))
                    currentCase.CostCentre = model.CostCentre;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Place))
                    currentCase.Place = model.Place;
                if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.UserCode))
                    currentCase.UserCode = model.UserCode;
            }
            #endregion

            #region Regarding
            if (currentCase.IsAbout == null) currentCase.IsAbout = new CaseIsAboutEntity() { Id = currentCase.Id };

            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy))
                currentCase.IsAbout.ReportedBy = model.IsAbout_ReportedBy;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name))
                currentCase.IsAbout.Person_Name = model.IsAbout_PersonName;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail))
                currentCase.IsAbout.Person_Email = model.IsAbout_PersonEmail;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone))
                currentCase.IsAbout.Person_Phone = model.IsAbout_PersonPhone;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone))
                currentCase.IsAbout.Person_Cellphone = model.IsAbout_PersonCellPhone;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id))
                currentCase.IsAbout.Region_Id = model.IsAbout_RegionId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id))
                currentCase.IsAbout.Department_Id = model.IsAbout_DepartmentId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id))
                currentCase.IsAbout.OU_Id = model.IsAbout_OrganizationUnitId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre))
                currentCase.IsAbout.CostCentre = model.IsAbout_CostCentre;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_Place))
                currentCase.IsAbout.Place = model.IsAbout_Place;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.IsAbout_UserCode))
                currentCase.IsAbout.UserCode = model.IsAbout_UserCode;
            #endregion

            #region ComputerInfo
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.InventoryNumber))
                currentCase.InventoryNumber = model.InventoryNumber;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ComputerType_Id))
                currentCase.InventoryType = model.ComputerTypeId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.InventoryLocation))
                currentCase.InventoryLocation = model.InventoryLocation;
            #endregion

            #region CaseInfo

            currentCase.ChangeTime = DateTime.UtcNow;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer))
                currentCase.RegistrationSourceCustomer_Id = model.RegistrationSourceCustomerId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
                currentCase.CaseType_Id = model.CaseTypeId;// Reqiured
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
                currentCase.ProductArea_Id = model.ProductAreaId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.System_Id))
                currentCase.System_Id = model.SystemId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Urgency_Id))
                currentCase.Urgency_Id = model.UrgencyId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Impact_Id))
                currentCase.Impact_Id = model.ImpactId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Category_Id))
                currentCase.Category_Id = model.CategoryId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Supplier_Id))
                currentCase.Supplier_Id = model.SupplierId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.InvoiceNumber))
                currentCase.InvoiceNumber = model.InvoiceNumber;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ReferenceNumber))
                currentCase.ReferenceNumber = model.ReferenceNumber;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Miscellaneous))
                currentCase.Miscellaneous = model.Miscellaneous;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Caption))
                currentCase.Caption = model.Caption;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Description))
                currentCase.Description = model.Description;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ContactBeforeAction))
                currentCase.ContactBeforeAction = model.ContactBeforeAction.ToInt();
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.SMS))
                currentCase.SMS = model.Sms.ToInt();
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.AgreedDate))
                currentCase.AgreedDate = model.AgreedDate;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Available))
                currentCase.Available = model.Available;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Cost))
            {
                currentCase.Cost = model.Cost;
                currentCase.OtherCost = model.OtherCost;
                currentCase.Currency = model.CostCurrency;
            }
            #endregion

            #region CaseManagement
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
                currentCase.WorkingGroup_Id = model.WorkingGroupId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id))
                currentCase.CaseResponsibleUser_Id = model.ResponsibleUserId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Performer_User_Id))
                currentCase.Performer_User_Id = model.PerformerId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
                currentCase.Priority_Id = model.PriorityId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Status_Id))
                currentCase.Status_Id = model.StatusId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
                currentCase.StateSecondary_Id = model.StateSecondaryId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Project))
                currentCase.Project_Id = model.ProjectId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Problem))
                currentCase.Problem_Id = model.ProblemId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.CausingPart))
                currentCase.CausingPartId = model.CausingPartId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Change))
                currentCase.Change_Id = model.ChangeId;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.PlanDate))
                currentCase.PlanDate = model.PlanDate;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.WatchDate))
                currentCase.WatchDate = model.WatchDate;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.Verified))
                currentCase.Verified = model.Verified.ToInt();
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.VerifiedDescription))
                currentCase.VerifiedDescription = model.VerifiedDescription;
            if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.SolutionRate))
                currentCase.SolutionRate = model.SolutionRate;
			#endregion

			#region Status
			//if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.FinishingDescription))
			//    currentCase.FinishingDescription = model.FinishingDescription;
			// if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.ClosingReason))
			////    currentCase = model.ClosingReason; // TODO: closing
			//if (_caseFieldSettingsHelper.IsActive(caseFieldSettings, caseTemplateSettings, GlobalEnums.TranslationCaseFields.FinishingDate))
			//    currentCase.FinishingDate = model.FinishingDate;  // TODO: closing

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
			var customerSettings = _customerSettingsService.GetCustomerSettings(cid);
			var basePath = _settingsLogic.GetFilePath(customerSettings);
			var customer = _customerService.GetCustomer(cid);
			var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimezoneId());

			if (isEdit)
			{
				int[] departmentIds = null;
				if (currentCase.Department_Id.HasValue)
					departmentIds = new int[] { currentCase.Department_Id.Value };

				var workTimeCalcFactory = new WorkTimeCalculatorFactory(
					ManualDependencyResolver.Get<IHolidayService>(),
					customer.WorkingDayStart,
					customer.WorkingDayEnd,
					TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId));

				var workTimeCalc = workTimeCalcFactory.Build(oldCase.RegTime, utcNow, departmentIds);

				var casestatesecundary = _stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

				if (casestatesecundary.IncludeInCaseStatistics == 0)
				{
					var externalTimeToAdd = workTimeCalc.CalculateWorkTime(
						oldCase.ChangeTime,
						utcNow,
						currentCase.Department_Id);
					currentCase.ExternalTime += externalTimeToAdd;
				}

				var possibleWorktime = workTimeCalc.CalculateWorkTime(
					currentCase.RegTime,
					utcNow,
					currentCase.Department_Id);

				currentCase.LeadTime = possibleWorktime - currentCase.ExternalTime;
			}

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
            if (!isEdit)
            {
                var temporaryFiles = _userTempFilesStorage.FindFiles(caseKey); //todo: check
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, currentCase.Id, UserId)).ToList();
                _caseFileService.AddFiles(newCaseFiles);
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
                //if (model.ExtendedCaseGuid != Guid.Empty)
                //{
                //    var exData = _caseService.GetExtendedCaseData(model.ExtendedCaseGuid);
                //    _caseService.CreateExtendedCaseRelationship(currentCase.Id, exData.Id);
                //}
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
            return Ok(currentCase.Id);
        }

        private void ApplyTemplateOrDefaultValues(int cid, int langId, Case currentCase,
            CaseSolution caseSolution, CaseDefaultsInfo customerDefaults, UserOverview userOverview)
        {
            currentCase.RegUserId = ""; // adUser.GetUserFromAdPath(),
            currentCase.RegUserDomain = ""; // adUser.GetDomainFromAdPath()
            currentCase.RegLanguage_Id = langId;
            currentCase.RegistrationSource = caseSolution?.RegistrationSource ?? (int) CaseRegistrationSource.Administrator;
            currentCase.CaseGUID = new Guid();
            currentCase.IpAddress = GetClientIp();
            currentCase.Customer_Id = cid;
            currentCase.User_Id = UserId;
            currentCase.RegTime = DateTime.UtcNow;
            currentCase.CaseResponsibleUser_Id = UserId;
            currentCase.Region_Id = caseSolution?.Region_Id ?? customerDefaults.RegionId;
            currentCase.CaseType_Id = caseSolution?.CaseType_Id ?? customerDefaults.CaseTypeId;
            currentCase.Supplier_Id = caseSolution?.Supplier_Id ?? customerDefaults.SupplierId;
            currentCase.Priority_Id = caseSolution?.Priority_Id ?? customerDefaults.PriorityId;
            currentCase.Status_Id = caseSolution?.Status_Id ?? customerDefaults.StatusId;
            currentCase.WorkingGroup_Id = caseSolution?.WorkingGroup_Id; //?? userOverview.DefaultWorkingGroupId;

            currentCase.ReportedBy = caseSolution?.ReportedBy;
            currentCase.PersonsName = caseSolution?.PersonsName;
            currentCase.PersonsEmail = caseSolution?.PersonsEmail;
            currentCase.PersonsPhone = caseSolution?.PersonsPhone;
            currentCase.PersonsCellphone = caseSolution?.PersonsCellPhone;
            currentCase.Department_Id = caseSolution?.Department_Id;
            currentCase.OU_Id = caseSolution?.OU_Id;
            currentCase.CostCentre = caseSolution?.CostCentre;
            currentCase.Place = caseSolution?.Place;
            currentCase.UserCode = caseSolution?.UserCode;

            if (currentCase.IsAbout == null) currentCase.IsAbout = new CaseIsAboutEntity() {Id = currentCase.Id};
            currentCase.IsAbout.ReportedBy = caseSolution?.IsAbout_ReportedBy;
            currentCase.IsAbout.Person_Name = caseSolution?.IsAbout_PersonsName;
            currentCase.IsAbout.Person_Email = caseSolution?.IsAbout_PersonsEmail;
            currentCase.IsAbout.Person_Phone = caseSolution?.IsAbout_PersonsPhone;
            currentCase.IsAbout.Person_Cellphone = caseSolution?.IsAbout_PersonsCellPhone;
            currentCase.IsAbout.Region_Id = caseSolution?.IsAbout_Region_Id;
            currentCase.IsAbout.Department_Id = caseSolution?.IsAbout_Department_Id;
            currentCase.IsAbout.OU_Id = caseSolution?.IsAbout_OU_Id;
            currentCase.IsAbout.CostCentre = caseSolution?.IsAbout_CostCentre;
            currentCase.IsAbout.Place = caseSolution?.IsAbout_Place;
            currentCase.IsAbout.UserCode = caseSolution?.IsAbout_UserCode;

            currentCase.InventoryNumber = caseSolution?.InventoryNumber;
            currentCase.InventoryType = caseSolution?.InventoryType;
            currentCase.InventoryLocation = caseSolution?.InventoryLocation;

            currentCase.RegistrationSourceCustomer_Id = caseSolution?.RegistrationSource;
            currentCase.ProductArea_Id = caseSolution?.ProductArea_Id;
            currentCase.System_Id = caseSolution?.System_Id;
            currentCase.Urgency_Id = caseSolution?.Urgency_Id;
            currentCase.Impact_Id = caseSolution?.Impact_Id;
            currentCase.Category_Id = caseSolution?.Category_Id;
            currentCase.InvoiceNumber = caseSolution?.InvoiceNumber;
            currentCase.ReferenceNumber = caseSolution?.ReferenceNumber;
            currentCase.Miscellaneous = caseSolution?.Miscellaneous;
            currentCase.Caption = caseSolution?.Caption;
            currentCase.ContactBeforeAction = caseSolution?.ContactBeforeAction ?? 0;
            currentCase.SMS = caseSolution?.SMS ?? 0;
            currentCase.AgreedDate = caseSolution?.AgreedDate;
            currentCase.Available = caseSolution?.Available;
            currentCase.Cost = caseSolution?.Cost ?? 0;
            currentCase.OtherCost = caseSolution?.OtherCost ?? 0;
            currentCase.Currency = caseSolution?.Currency;

            currentCase.Performer_User_Id = caseSolution?.PerformerUser_Id;
            currentCase.StateSecondary_Id = caseSolution?.StateSecondary_Id;
            currentCase.Project_Id = caseSolution?.Project_Id;
            currentCase.Problem_Id = caseSolution?.Problem_Id;
            currentCase.CausingPartId = caseSolution?.CausingPartId;
            currentCase.Change_Id = caseSolution?.Change_Id;
            currentCase.PlanDate = caseSolution?.PlanDate;
            currentCase.WatchDate = caseSolution?.WatchDate;
            currentCase.Verified = caseSolution?.Verified ?? 0;
            currentCase.VerifiedDescription = caseSolution?.VerifiedDescription;
            currentCase.SolutionRate = caseSolution?.SolutionRate;
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
