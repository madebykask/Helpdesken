using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Common.Enums.BusinessRule;
using static DH.Helpdesk.BusinessData.Models.Shared.ProcessResult;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Enums.CaseSolution;
using DH.Helpdesk.Common.Enums.Condition;
using DH.Helpdesk.Common.Extensions.Object;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public class UniversalCaseService : IUniversalCaseService
    {

        private const string _CASE_TEXT = "ärendet";
        private const string _INVALID_TEXT = "är inte giltigt";
        private const string _INVALID_EMPTY_TEXT = "kan inte vara tomt";

        private string[] _UNIT_TEXTS = { };

        private const int _DEFAULT_CONTACT_BEFORE_ACTION = 0;
        private const int _DEFAULT_COST = 0;
        private const int _DEFAULT_DELETED = 0;
        private const int _DEFAULT_EXTERNAL_TIME = 0;
        private const int _DEFAULT_NO_MAIL_TO_NOTIFIER = 1;
        private const int _DEFAULT_OTHER_COST = 0;
        private const int _DEFAULT_SMS = 0;

        private int _currentLanguageId;
        private IList<CaseFieldSettingsWithLanguage> _caseFieldSettings;
        private IList<CustomKeyValue<string, string>> _localTranslations;
        private CustomerUser _customerUser;

        private readonly ICaseRepository _caseRepository;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ITextTranslationService _textTranslationService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ISettingService _settingService;
        private readonly IHolidayService _holidayService;
        private readonly ICaseService _caseService;
        private readonly ILogService _logService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IExtendedCaseService _extendedCaseService;

        private readonly IDepartmentService _departmentService;
        private readonly IRegionService _regionService;
        private readonly IOUService _oUService;
        private readonly IConditionService _conditionService;
        private readonly ICaseDeletionService _caseDeletionService;
        private IBusinessModelToEntityMapper<CaseModel, Case> _caseModelToEntityMapper;

        public UniversalCaseService(ICaseRepository caseRepository,
                                    ICustomerService customerService,
                                    IUserService userService,
                                    IWorkingGroupService workingGroupService,
                                    ICaseFieldSettingService caseFieldSettingService,
                                    ITextTranslationService textTranslationService,
                                    ICustomerUserService customerUserService,
                                    IStateSecondaryService stateSecondaryService,
                                    ISettingService settingService,
                                    IHolidayService holidayService,
                                    ICaseService caseService,
                                    ILogService logService,
                                    ICaseSolutionService caseSolutionService,
                                    IExtendedCaseService extendedCaseService,
                                    IDepartmentService departmentService,
                                    IRegionService regionService,
                                    IOUService oUService,
                                    IConditionService conditionService,
                                    ICaseDeletionService caseDeletionService,
                                    IBusinessModelToEntityMapper<CaseModel, Case> caseModelToEntityMapper
            )
        {
            _caseRepository = caseRepository;
            _customerService = customerService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _caseFieldSettingService = caseFieldSettingService;
            _textTranslationService = textTranslationService;
            _customerUserService = customerUserService;
            _stateSecondaryService = stateSecondaryService;
            _settingService = settingService;
            _holidayService = holidayService;
            _caseService = caseService;
            _logService = logService;
            _caseSolutionService = caseSolutionService;
            _extendedCaseService = extendedCaseService;
            _departmentService = departmentService;
            _regionService = regionService;
            _oUService = oUService;
            _conditionService = conditionService;
            _caseModelToEntityMapper = caseModelToEntityMapper;
            _caseDeletionService = caseDeletionService;
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel, out int caseId, out decimal caseNumber, bool sendEmail = true)
        {
            var isNewCase = caseModel.Id == 0;

            var res = new ProcessResult("Save Case");
            caseId = -1;
            caseNumber = -1;

            res = PrimaryValidation(ref caseModel);
            if (res.IsSucceed)
            {
                CaseTimeMetricsModel timeMetrics;
                res = CloneCase(ref caseModel, auxModel, out timeMetrics);
                if (res.IsSucceed)
                {
                    res = ValidateCase(caseModel);
                    if (res.IsSucceed)
                    {
                        var emailSettings = sendEmail ? GetEmailSettings(caseModel, auxModel) : null;
                        res = DoSaveCase(caseModel, auxModel, timeMetrics, emailSettings, out caseId, out caseNumber);
                    }
                }
            }

            //connect extended case if new and extendedcasedataid is provided
            if (res.IsSucceed && caseId != -1 && isNewCase && caseModel.ExtendedCaseData_Id.HasValue)
            {
                _caseService.CreateExtendedCaseRelationship(caseId, caseModel.ExtendedCaseData_Id.Value, caseModel.ExtendedCaseForm_Id.Value);
            }

            return res;
        }

        private CaseModel ApplyValuesFromCaseSolution(CaseModel model, int caseTemplateId)
        {
            if (model == null)
                return null;

            var caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId);
            if (caseTemplate == null)
                return model;

            //Check if we should apply template
            if (caseTemplate.OverWritePopUp == 1)
            {
                model.CaseSolution_Id = caseTemplateId;
                model.Customer_Id = caseTemplate.Customer_Id;

                _caseSolutionService.ApplyCaseSolution(model, caseTemplate);

                model.Text_External = caseTemplate.Text_External.IfNullThenElse(model.Text_External);
                model.Text_Internal = caseTemplate.Text_Internal.IfNullThenElse(model.Text_Internal);
                model.FinishingType_Id = caseTemplate.FinishingCause_Id.IfNullThenElse(model.FinishingType_Id);
            }

            return model;
        }

        public ProcessResult SaveCaseCheckSplit(CaseModel caseModel, AuxCaseModel auxModel, out int caseId, out decimal caseNumber)
        {
            var isNewCase = caseModel.Id == 0;
            caseId = -1;
            caseNumber = -1;

            var res = new ProcessResult("Save Case Check Split");

            if (caseModel.CaseSolution_Id.HasValue && isNewCase)
            {
                var caseSolution = _caseSolutionService.GetCaseSolution(caseModel.CaseSolution_Id.Value);

                // Split into "parent" and "child(s)"
                if (caseSolution.CaseRelationType == CaseRelationType.ParentAndChildren)
                {
                    return res = SaveParentAndChildren(caseModel, auxModel, caseSolution, out caseId, out caseNumber);
                }
                // Create indepent cases based on the "parent" case solution template
                if (caseSolution.CaseRelationType == CaseRelationType.OnlyDescendants)
                {
                    return res = SaveNewDescendandts(caseModel, auxModel, caseSolution, out caseId, out caseNumber);
                }


                // Create cases based on "parent", and "child" but independent
                if (caseSolution.CaseRelationType == CaseRelationType.SelfAndDescendandts)
                {
                    return res = SaveNewSelfAndDescendandts(caseModel, auxModel, caseSolution, out caseId, out caseNumber);
                }
            }

            //do regular save
            res = SaveCase(caseModel, auxModel, out caseId, out caseNumber);
            return res;
        }

        public CaseTimeMetricsModel ClaculateCaseTimeMetrics(CaseModel caseModel, AuxCaseModel auxModel, CaseModel oldCase = null)
        {
            var ret = new CaseTimeMetricsModel
            {
                ExternalTime = 0,
                ActionExternalTime = 0,
                LeadTime = 0,
                LeadTimeForNow = 0,
                ActionLeadTime = 0,
                LatestSLACountDate = null
            };

            if (oldCase == null && caseModel.Id > 0)
                oldCase = _caseRepository.GetCase(caseModel.Id);

            var isEditMode = caseModel.Id != 0;
            var curCustomer = _customerService.GetCustomer(caseModel.Customer_Id);
            var setting = _settingService.GetCustomerSetting(curCustomer.Id);
            var customerTimeOffset = setting.TimeZone_offset;

            if (auxModel.UserTimeZone == null && auxModel.CurrentUserId != 0)
            {
                auxModel.UserTimeZone = TimeZoneInfo.Local;
                var timeZoneId = _userService.GetUserTimeZoneId(auxModel.CurrentUserId);
                if (timeZoneId != string.Empty)
                    auxModel.UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            else
            {
                auxModel.UserTimeZone = TimeZoneInfo.Local;
            }
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(curCustomer.TimeZoneId);

            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                                            _holidayService,
                                            curCustomer.WorkingDayStart,
                                            curCustomer.WorkingDayEnd,
                                            timeZone);

            int[] deptIds = null;
            if (caseModel.Department_Id.HasValue)
            {
                deptIds = new int[] { caseModel.Department_Id.Value };
            }

            #region ExtenalTime & ActionExtenalTime 

            if (isEditMode && oldCase != null)
            {
                if (oldCase.StateSecondary_Id.HasValue)
                {
                    var caseSubState = _stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);
                    if (caseSubState.IncludeInCaseStatistics == 0)
                    {
                        var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, auxModel.UtcNow, deptIds);
                        ret.ExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            auxModel.UtcNow,
                            oldCase.Department_Id) + oldCase.ExternalTime;

                        workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, auxModel.UtcNow, deptIds, customerTimeOffset);
                        ret.ActionExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            auxModel.UtcNow,
                            oldCase.Department_Id, customerTimeOffset);
                    }
                }
            }
            #endregion

            #region LeadTime & ActionLeadTime

            var isCaseGoingToFinish = caseModel.FinishingType_Id.HasValue &&
                                      caseModel.FinishingType_Id.Value > 0;

            DateTime uBoundDate = isCaseGoingToFinish ? caseModel.FinishingDate.Value : auxModel.UtcNow;

            /*Always claculate LeadTime (FinishingTime|UtcNow)*/
            var leadWorkTimeCalc = workTimeCalcFactory.Build(caseModel.RegTime, uBoundDate, deptIds);
            var leadTime = leadWorkTimeCalc.CalculateWorkTime(
                caseModel.RegTime,
                uBoundDate.ToUniversalTime(),
                caseModel.Department_Id) - ret.ExternalTime;

            if (oldCase == null)
            {
                ret.LeadTime = isCaseGoingToFinish ? leadTime : 0;
            }
            else
            {
                ret.LeadTime = isCaseGoingToFinish ? leadTime : oldCase.LeadTime.Value;
            }
            ret.LeadTimeForNow = leadTime;

            if (oldCase != null && oldCase.Id > 0)
            {
                var actionLeadWorkTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, uBoundDate, deptIds, customerTimeOffset);
                var actionLeadTime = actionLeadWorkTimeCalc.CalculateWorkTime(
                    oldCase.ChangeTime,
                    uBoundDate.ToUniversalTime(),
                    oldCase.Department_Id, customerTimeOffset) - ret.ActionExternalTime;
                ret.ActionLeadTime = actionLeadTime;
            }

            #endregion

            #region LatestSLACountDate

            ret.LatestSLACountDate = CalculateLatestSLACountDate(oldCase?.StateSecondary_Id, caseModel.StateSecondary_Id, oldCase?.LatestSLACountDate);

            #endregion

            return ret;
        }


        /*********************************************/
        /************ PRIVATE Methods ****************/
        /*********************************************/

        private ProcessResult PrimaryValidation(ref CaseModel caseModel)
        {
            /*In this method you can't translate messages (Use validation instead) */

            var retData = new List<KeyValuePair<string, string>>();

            #region Primary validation

            if (caseModel.Id < 0)
                retData.Add(new KeyValuePair<string, string>("Case_Id", "Case Id is not valid!"));
            else
            {
                var oldCase = new CaseModel();
                if (caseModel.Id != 0)
                {
                    oldCase = _caseRepository.GetCase(caseModel.Id);
                    if (oldCase == null || oldCase.Id != caseModel.Id)
                        retData.Add(new KeyValuePair<string, string>("Case_Id", "Case Id is not valid!"));
                    else
                        caseModel.Customer_Id = oldCase.Customer_Id;
                }

                if (caseModel.Customer_Id <= 0)
                    retData.Add(new KeyValuePair<string, string>("Customer_Id", "Customer Id is not valid!"));
                else
                {
                    var customer = _customerService.GetCustomer(caseModel.Customer_Id);
                    if (customer == null || customer.Id != caseModel.Customer_Id)
                        retData.Add(new KeyValuePair<string, string>("Customer_Id", "Customer Id is not valid!"));
                }
            }

            if (retData.Any())
                return new ProcessResult("Primary Validation", ResultTypeEnum.ERROR, "Case is not valid!", retData);

            #endregion

            return new ProcessResult("Primary Validation.");
        }

        private ProcessResult CloneCase(ref CaseModel caseModel, AuxCaseModel auxModel, out CaseTimeMetricsModel timeMetrics)
        {
            var isEditMode = caseModel.Id != 0;

            var retData = new List<KeyValuePair<string, string>>();

            if (caseModel.Performer_User_Id == 0)
                caseModel.Performer_User_Id = null;

            if (caseModel.CaseResponsibleUser_Id == 0)
                caseModel.CaseResponsibleUser_Id = null;

            if (caseModel.RegLanguage_Id == 0)
                caseModel.RegLanguage_Id = auxModel.CurrentLanguageId;

            if (caseModel.Id == 0)
            {
                caseModel.CaseGUID = Guid.NewGuid();
                caseModel.ContactBeforeAction = _DEFAULT_CONTACT_BEFORE_ACTION;
                caseModel.Cost = _DEFAULT_COST;
                caseModel.Deleted = _DEFAULT_DELETED;
                caseModel.ExternalTime = _DEFAULT_EXTERNAL_TIME;
                caseModel.NoMailToNotifier = _DEFAULT_NO_MAIL_TO_NOTIFIER;
                caseModel.OtherCost = _DEFAULT_OTHER_COST;
                caseModel.SMS = _DEFAULT_SMS;
            }

            /*Load required info*/
            _currentLanguageId = auxModel.CurrentLanguageId;
            _UNIT_TEXTS = new string[] { _INVALID_TEXT, _INVALID_EMPTY_TEXT, _CASE_TEXT };
            _localTranslations = _textTranslationService.GetTranslationsFor(_UNIT_TEXTS.ToList(), _currentLanguageId);
            _caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(caseModel.Customer_Id, _currentLanguageId).ToList();
            if (auxModel.CurrentUserId > 0)
                _customerUser = _customerUserService.GetCustomerUserSettings(caseModel.Customer_Id, auxModel.CurrentUserId);
            var oldCase = new CaseModel();

            /*Apply rules*/
            if (_caseFieldSettings.FirstOrDefault(f => f.Name == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()) == null)
                caseModel.PersonsEmail = string.Empty;

            if (isEditMode)
            {
                oldCase = _caseRepository.GetCase(caseModel.Id);

                if (_customerUser != null && _customerUser.UserInfoPermission == 0)
                {
                    caseModel.ReportedBy = oldCase.ReportedBy;
                    caseModel.Place = oldCase.Place;
                    caseModel.PersonsName = oldCase.PersonsName;
                    caseModel.PersonsEmail = oldCase.PersonsEmail;
                    caseModel.PersonsPhone = oldCase.PersonsPhone;
                    caseModel.PersonsCellphone = oldCase.PersonsCellphone;
                    caseModel.Region_Id = oldCase.Region_Id;
                    caseModel.Department_Id = oldCase.Department_Id;
                    caseModel.OU_Id = oldCase.OU_Id;
                    caseModel.UserCode = oldCase.UserCode;
                }
                caseModel.RegTime = oldCase.RegTime;
            }
            else
            {
                oldCase = null;
                caseModel.RegTime = auxModel.UtcNow;
                if (caseModel.RegLanguage_Id == 0)
                    caseModel.RegLanguage_Id = auxModel.CurrentLanguageId;
            }

            if (caseModel.FinishingType_Id > 0)
            {
                if (!caseModel.FinishingDate.HasValue)
                    caseModel.FinishingDate = auxModel.UtcNow;
                else
                {
                    if (caseModel.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        caseModel.FinishingDate = auxModel.UtcNow;
                    }
                    else if (oldCase != null && oldCase.ChangeTime.ToShortDateString() == caseModel.FinishingDate.Value.ToShortDateString())
                    {
                        var lastChangedTime = new DateTime(oldCase.ChangeTime.Year, oldCase.ChangeTime.Month, oldCase.ChangeTime.Day, 22, 59, 59);
                        caseModel.FinishingDate = lastChangedTime;
                    }
                    else
                    {
                        caseModel.FinishingDate = DateTime.SpecifyKind(caseModel.FinishingDate.Value, DateTimeKind.Local).ToUniversalTime();
                    }
                }
                caseModel.FinishingDate = DatesHelper.Max(caseModel.RegTime, caseModel.FinishingDate.Value);
            }

            if (caseModel.ProductArea_Id.HasValue && caseModel.ProductAreaSetDate == null)
                caseModel.ProductAreaSetDate = auxModel.UtcNow;

            /*Calculate Times*/
            var calculatedTimes = ClaculateCaseTimeMetrics(caseModel, auxModel, oldCase);
            caseModel.LeadTime = calculatedTimes.LeadTime;
            caseModel.ExternalTime = calculatedTimes.ExternalTime;
            caseModel.LatestSLACountDate = calculatedTimes.LatestSLACountDate;
            timeMetrics = calculatedTimes;

            return new ProcessResult("Case cloned.");
        }

        private ProcessResult ValidateCase(CaseModel caseModel)
        {
            if (!_caseFieldSettings.Any())
                return new ProcessResult("Case Validation", ResultTypeEnum.ERROR, string.Format("{0} {1}", TranslateLocally(_CASE_TEXT), TranslateLocally(_INVALID_TEXT)));

            var retData = new List<KeyValuePair<string, string>>();

            #region case validation

            if (caseModel.Id == 0)
            {
                if (caseModel.CaseType_Id == 0)
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.CaseType_Id));
            }

            if (caseModel.FinishingDate.HasValue)
            {
                if (!caseModel.FinishingType_Id.HasValue)
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.ClosingReason));
            }

            if (retData.Any())
                return new ProcessResult("Case Validation", ResultTypeEnum.ERROR, "Case is not valid!", retData);


            #endregion

            return new ProcessResult("Case Validated.");
        }

        private ProcessResult DoSaveCase(CaseModel caseModel, AuxCaseModel auxModel,
                                         CaseTimeMetricsModel timesModel,
                                         CaseMailSetting mailSettings, out int caseId, out decimal caseNumber)
        {
            /*TODO: After merge CaseServices case must be sent to the Repository directly from here(No need to use CaseService anymore) */
            /* Convert caseModel to Case entity to make it ready for Save method */
            caseId = -1;
            caseNumber = -1;
            var oldCase = new Case();
            if (caseModel.Id != 0)
                oldCase = _caseService.GetCaseById(caseModel.Id);

            //create case entity
            var caseEntity = ConvertCaseModelToCase(caseModel, oldCase);

            var logEntity = GetCaseLog(caseModel, auxModel);
            var extraInfo = new CaseExtraInfo()
            {
                ActionExternalTime = timesModel.ActionExternalTime,
                ActionLeadTime = timesModel.ActionLeadTime,
                LeadTimeForNow = timesModel.LeadTimeForNow,
                CreatedByApp = auxModel.CurrentApp
            };

            IDictionary<string, string> errors;
            var historyId = _caseService.SaveCase(caseEntity, logEntity, auxModel.CurrentUserId, auxModel.UserIdentityName, extraInfo, out errors);

            if (errors.Any())
                return new ProcessResult("Do Save Case", ResultTypeEnum.ERROR, "Case could not be saved!", errors);

            logEntity.CaseId = caseEntity.Id;
            logEntity.CaseHistoryId = historyId;
            var logId = _logService.SaveLog(logEntity, 0, out errors);
            logEntity.Id = logId;

            var curUser = auxModel.CurrentUserId > 0 ? _userService.GetUser(auxModel.CurrentUserId) : null;
            oldCase = oldCase.Id > 0 ? oldCase : null;

            if (mailSettings != null)
            {
                _caseService.SendCaseEmail(caseEntity.Id, mailSettings, historyId, "", auxModel.UserTimeZone, oldCase, logEntity, null, curUser);
            }

            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, caseEntity, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, caseEntity.Id, logEntity, auxModel.UserTimeZone, historyId, "", auxModel.CurrentLanguageId, mailSettings);

            caseId = caseEntity.Id;
            caseNumber = caseEntity.CaseNumber;
            return new ProcessResult("Case saved", caseEntity);
        }

        private KeyValuePair<string, string> GenerateInvalidMessage(GlobalEnums.TranslationCaseFields caseFieldName)
        {
            var caseFieldNameStr = caseFieldName.ToString();
            var field = _caseFieldSettings.FirstOrDefault(f => f.Name.Equals(caseFieldNameStr, StringComparison.CurrentCultureIgnoreCase));
            if (field == null)
                return new KeyValuePair<string, string>(caseFieldNameStr, string.Format("{0} {1}", caseFieldName, TranslateLocally(_INVALID_TEXT)));
            else
                return new KeyValuePair<string, string>(caseFieldNameStr, string.Format("{0} {1}", field.Label, TranslateLocally(_INVALID_TEXT)));
        }

        private KeyValuePair<string, string> GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields caseFieldName)
        {
            var caseFieldNameStr = caseFieldName.ToString();
            var field = _caseFieldSettings.FirstOrDefault(f => f.Name.Equals(caseFieldNameStr, StringComparison.CurrentCultureIgnoreCase));
            if (field == null)
                return new KeyValuePair<string, string>(caseFieldNameStr, string.Format("{0} {1}", caseFieldName, TranslateLocally(_INVALID_EMPTY_TEXT)));
            else
                return new KeyValuePair<string, string>(caseFieldNameStr, string.Format("{0} {1}", field.Label, TranslateLocally(_INVALID_EMPTY_TEXT)));
        }

        private string TranslateLocally(string text)
        {
            var translations = _localTranslations.Where(t => t.Key.Equals(text, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return translations.Any() ? translations.First().Value : text;
        }

        private DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;

            if (oldSubStateId.HasValue)
            {
                var oldSubStatus = _stateSecondaryService.GetStateSecondary(oldSubStateId.Value);
                if (oldSubStatus != null)
                    oldSubStateMode = oldSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (oldSubStateMode == -1 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == -1)
                ret = oldSLADate;
            else if (oldSubStateMode == 1 && newSubStateMode == 1)
                ret = oldSLADate;
            else if (oldSubStateMode == 0 && newSubStateMode == 0)
                ret = oldSLADate;

            return ret;
        }

        private MailSenders GetMailSenders(CaseModel caseModel)
        {
            var mailSenders = new MailSenders();
            if (caseModel.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(caseModel.WorkingGroup_Id.Value);
                mailSenders.WGEmail = curWG.EMail;
            }

            if (caseModel.DefaultOwnerWG_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(caseModel.DefaultOwnerWG_Id.Value);
                mailSenders.DefaultOwnerWGEMail = curWG.EMail;
            }

            return mailSenders;
        }

        private CaseMailSetting GetEmailSettings(CaseModel caseModel, AuxCaseModel auxModel)
        {
            var curCustomer = _customerService.GetCustomer(caseModel.Customer_Id);
            var setting = _settingService.GetCustomerSetting(caseModel.Customer_Id);
            var mailSenders = GetMailSenders(caseModel);
            mailSenders.SystemEmail = curCustomer.HelpdeskEmail;

            var caseMailSetting = new CaseMailSetting(
                curCustomer.NewCaseEmailList,
                curCustomer.HelpdeskEmail,
                auxModel.AbsolutreUrl,
                setting.DontConnectUserToWorkingGroup)
            {
                CustomeMailFromAddress = mailSenders
            };

            return caseMailSetting;
        }

        public Case ConvertCaseModelToCase(CaseModel caseModel, Case oldCase)
        {
            var caseEntity = oldCase != null && oldCase.Id > 0 ? oldCase : new Case();
            caseEntity.IsAbout = oldCase?.IsAbout ?? new CaseIsAboutEntity()
            {
                Id = oldCase?.Id ?? 0
            };

            //note: also handles Case.IsAbout properties due to additional Case.IsAbout_<name> setters
            _caseModelToEntityMapper.Map(caseModel, caseEntity);

            return caseEntity;
        }

        private CaseLog GetCaseLog(CaseModel caseModel, AuxCaseModel auxCaseModel)
        {
            var ret = new CaseLog
            {
                Id = 0, // Will bet set after save case
                CaseHistoryId = 0, // Will bet set after save case
                CaseId = caseModel.Id,
                FinishingDate = caseModel.FinishingDate,
                FinishingType = caseModel.FinishingType_Id,
                LogGuid = Guid.NewGuid(),
                LogType = 0,
                TextExternal = caseModel.Text_External,
                TextInternal = caseModel.Text_Internal,

                //SendMailAboutLog = 
                SendMailAboutCaseToNotifier = false,

                //// aux model values
                UserId = auxCaseModel.CurrentUserId > 0 ? (int?)auxCaseModel.CurrentUserId : null,
                RegUser = auxCaseModel.UserIdentityName ?? String.Empty

            };

            return ret;
        }

        /// <summary>
        /// If case is split and copied to another customer, departmentId needs to be changed to corresponding Id of the other customer.
        /// </summary>
        private int? ChangeDepartmentId(int baseCustomerId, int? baseDepartmentId, int descendantCustomerId)
        {
            if (!baseDepartmentId.HasValue)
                return null;

            if (baseCustomerId != descendantCustomerId && baseDepartmentId.HasValue)
            {
                try
                {
                    var baseDepartment = _departmentService.GetDepartment(baseDepartmentId.Value);
                    int? newDepartmentId = _departmentService.GetDepartmentIdByCustomerAndName(descendantCustomerId, baseDepartment.DepartmentName);
                    return newDepartmentId;
                }
                catch (Exception)
                {
                    return baseDepartmentId;
                }
            }

            return baseDepartmentId;
        }

        //todo: refactor Save methods to use share same/duplicate logic
        #region Split MultiCase

        private ProcessResult SaveNewDescendandts(CaseModel caseModel, AuxCaseModel baseAuxModel, CaseSolution baseCaseSolution, out int caseId, out decimal caseNumber)
        {
            var res = new ProcessResult("SaveNew Descendants");

            //Multicase - Split
            var conditionTypeId = (int)((ConditionType)Enum.Parse(typeof(ConditionType), ConditionType.MultiCaseSplit.ToString()));

            caseId = -1;
            caseNumber = -1;

            var baseCaseId = -1;
            decimal caseNum = -1;

            var baseCaseExtendedCaseDataId = caseModel.ExtendedCaseData_Id ?? 0;

            //save base case first to perform conditions validations and then delete it
            var baseCaseModel = caseModel.DeepClone();
            baseCaseModel = ApplyValuesFromCaseSolution(baseCaseModel, baseCaseSolution.Id);

            //prevent email notifications from sending for base (multiform) case
            var baseCaseRes = SaveCase(baseCaseModel, baseAuxModel, out baseCaseId, out caseNum, false);

            if (baseCaseRes.IsSucceed)
            {
                try
                {
                    if (baseCaseSolution.SplitToCaseSolutionDescendants != null && baseCaseSolution.SplitToCaseSolutionDescendants.Any())
                    {
                        var caseSolutions = baseCaseSolution.SplitToCaseSolutionDescendants
                            .Where(x => x.SplitToCaseSolutionDescendant.Status > 0)
                            .OrderBy(x => x.SplitToCaseSolutionDescendant.SortOrder)
                            .ToList();

                        var baseCaseExCaseForm = baseCaseSolution.ExtendedCaseForms?.FirstOrDefault();

                        foreach (var splitToCaseSolutionEntity in caseSolutions)
                        {
                            var childCaseSolutionId = splitToCaseSolutionEntity.SplitToCaseSolutionDescendant.Id;
                            var childCaseTemplate = _caseSolutionService.GetCaseSolution(childCaseSolutionId);

                            var doSplit = _conditionService.CheckConditions(baseCaseId, childCaseSolutionId, conditionTypeId);

                            if (doSplit.Show)
                            {
                                ExtendedCaseFormEntity exCaseForm = null;

                                var childCaseModel = caseModel.DeepClone();

                                //TODO: refactor and make a long term solution, this is just for DepartmentId
                                childCaseModel.Department_Id = ChangeDepartmentId(baseCaseModel.Customer_Id, baseCaseModel.Department_Id, childCaseTemplate.Customer_Id);

                                //apply values from case solution
                                childCaseModel = ApplyValuesFromCaseSolution(childCaseModel, childCaseSolutionId);

                                //apply caseBinding values from child extended case template which are missing in the parent form
                                if (baseCaseExtendedCaseDataId > 0)
                                {
                                    exCaseForm = childCaseTemplate.ExtendedCaseForms.FirstOrDefault();
                                    ApplyCaseBindingValuesFromExtendedCase(childCaseModel, exCaseForm, baseCaseExtendedCaseDataId, baseCaseExCaseForm);
                                }

                                // prevent extended case relation creation in SaveCase
                                childCaseModel.ExtendedCaseData_Id = null;
                                childCaseModel.ExtendedCaseForm_Id = null;

                                int childCaseId;
                                res = SaveCase(childCaseModel, baseAuxModel, out childCaseId, out caseNum);

                                if (res.IsSucceed && childCaseId > 0)
                                {
                                    if (caseId == -1)
                                    {
                                        caseNumber = caseNum;
                                        caseId = childCaseId;
                                    }

                                    if (baseCaseExtendedCaseDataId > 0 && exCaseForm != null)
                                    {
                                        // create a copy from parent extended case 
                                        _extendedCaseService.CopyExtendedCaseToCase(baseCaseExtendedCaseDataId, childCaseId, baseAuxModel.UserIdentityName, exCaseForm.Id);
                                    }

                                }
                            }
                        }
                    }
                }
                finally
                {
                    //delete base case
                    if (baseCaseId > 0)
                        if (_caseDeletionService != null)
                        {
                            _caseDeletionService.Delete(baseCaseId, "", null);
                        }
                }
            }

            return res;
        }

        private ProcessResult SaveNewSelfAndDescendandts(CaseModel baseCaseModel, AuxCaseModel baseAuxModel, CaseSolution baseCaseSolution, out int caseId, out decimal caseNumber)
        {
            //Multicase - Split
            int conditionType_Id = (int)((ConditionType)Enum.Parse(typeof(ConditionType), ConditionType.MultiCaseSplit.ToString()));

            var isNewCase = baseCaseModel.Id == 0;
            var res = new ProcessResult("SaveNew SelfAndDescendandts");
            var selfCaseId = -1;

            //apply values from case solution
            baseCaseModel = ApplyValuesFromCaseSolution(baseCaseModel, baseCaseSolution.Id);

            decimal caseNum;

            //save "base"
            res = SaveCase(baseCaseModel, baseAuxModel, out selfCaseId, out caseNum);

            if (res.IsSucceed && selfCaseId != -1)
            {
                if (baseCaseSolution.SplitToCaseSolutionDescendants != null && baseCaseSolution.SplitToCaseSolutionDescendants.Any())
                {
                    var baseCaseExtendedCaseDataId = baseCaseModel.ExtendedCaseData_Id ?? 0;
                    var baseCaseExCaseForm = baseCaseSolution.ExtendedCaseForms?.FirstOrDefault();

                    foreach (var item in baseCaseSolution.SplitToCaseSolutionDescendants.Where(x => x.SplitToCaseSolutionDescendant.Status > 0).OrderBy(x => x.SplitToCaseSolutionDescendant.SortOrder))
                    {
                        var childCaseTemplate = _caseSolutionService.GetCaseSolution(item.SplitToCaseSolutionDescendant.Id);

                        var doSplit = _conditionService.CheckConditions(selfCaseId, item.SplitToCaseSolutionDescendant.Id, conditionType_Id);

                        if (doSplit.Show)
                        {
                            ExtendedCaseFormEntity exCaseForm = null;

                            var childCaseModel = baseCaseModel.DeepClone();

                            //TODO: refactor and make a long term solution, this is just for DepartmentId
                            baseCaseModel.Department_Id = ChangeDepartmentId(childCaseModel.Customer_Id, childCaseModel.Department_Id, childCaseTemplate.Customer_Id);

                            int childCaseId = -1;
                            decimal childCaseNum;

                            //apply values from case solution
                            childCaseModel = ApplyValuesFromCaseSolution(childCaseModel, item.SplitToCaseSolutionDescendant.Id);

                            //apply caseBinding values from child extended case template which are missing in the parent form
                            if (baseCaseExtendedCaseDataId > 0)
                            {
                                exCaseForm = childCaseTemplate.ExtendedCaseForms.FirstOrDefault();
                                ApplyCaseBindingValuesFromExtendedCase(childCaseModel, exCaseForm, baseCaseExtendedCaseDataId, baseCaseExCaseForm);
                            }

                            // Required to avoid extended Case creation during SaveCase. Extended case is created later.
                            childCaseModel.ExtendedCaseData_Id = null;
                            childCaseModel.ExtendedCaseForm_Id = null;

                            var childRes = SaveCase(childCaseModel, baseAuxModel, out childCaseId, out childCaseNum);

                            if (childRes.IsSucceed && childCaseId != -1)
                            {
                                if (baseCaseExtendedCaseDataId > 0 && exCaseForm != null)
                                {
                                    // create a copy from parent extended case 
                                    _extendedCaseService.CopyExtendedCaseToCase(baseCaseExtendedCaseDataId, childCaseId, baseAuxModel.UserIdentityName, exCaseForm.Id);
                                }
                            }
                        }
                    }
                }
            }

            caseId = selfCaseId;
            caseNumber = caseNum;
            return res;
        }

        private ProcessResult SaveParentAndChildren(CaseModel baseCaseModel, AuxCaseModel baseAuxModel, CaseSolution baseCaseSolution, out int caseId, out decimal caseNumber)
        {
            IDictionary<string, string> errors;

            //Multicase - Split
            int conditionType_Id = (int)((ConditionType)Enum.Parse(typeof(ConditionType), ConditionType.MultiCaseSplit.ToString()));

            var isNewCase = baseCaseModel.Id == 0;
            ProcessResult res = new ProcessResult("Save Parent And Children");
            int selfCaseId = -1;

            //apply values from case solution
            baseCaseModel = ApplyValuesFromCaseSolution(baseCaseModel, baseCaseModel.CaseSolution_Id.Value);

            decimal _caseNum;
            //save base
            res = SaveCase(baseCaseModel, baseAuxModel, out selfCaseId, out _caseNum);

            if (res.IsSucceed && selfCaseId != -1)
            {
                //check if there should be created child cases
                if (baseCaseSolution.SplitToCaseSolutionDescendants != null && baseCaseSolution.SplitToCaseSolutionDescendants.Any())
                {
                    var baseCaseExtendedCaseDataId = baseCaseModel.ExtendedCaseData_Id ?? 0;
                    var baseCaseExCaseForm = baseCaseSolution.ExtendedCaseForms?.FirstOrDefault();

                    foreach (var item in baseCaseSolution.SplitToCaseSolutionDescendants.Where(x => x.SplitToCaseSolutionDescendant.Status > 0).OrderBy(x => x.SplitToCaseSolutionDescendant.SortOrder))
                    {
                        var childCaseTemplate = _caseSolutionService.GetCaseSolution(item.SplitToCaseSolutionDescendant.Id);

                        var doSplit = _conditionService.CheckConditions(selfCaseId, item.SplitToCaseSolutionDescendant.Id, conditionType_Id);

                        if (doSplit.Show)
                        {
                            ExtendedCaseFormEntity exCaseForm = null;
                            var childCaseModel = baseCaseModel.DeepClone();

                            //TODO: refactor and make a long term solution, this is just for DepartmentId
                            baseCaseModel.Department_Id = ChangeDepartmentId(childCaseModel.Customer_Id, childCaseModel.Department_Id, childCaseTemplate.Customer_Id);

                            int childCaseId = -1;
                            decimal childCaseNum;

                            //apply values from case solution
                            childCaseModel = ApplyValuesFromCaseSolution(childCaseModel, item.SplitToCaseSolutionDescendant.Id);

                            //apply caseBinding values from child extended case template which are missing in the parent form
                            if (baseCaseExtendedCaseDataId > 0)
                            {
                                exCaseForm = childCaseTemplate.ExtendedCaseForms.FirstOrDefault();
                                ApplyCaseBindingValuesFromExtendedCase(childCaseModel, exCaseForm, baseCaseExtendedCaseDataId, baseCaseExCaseForm);
                            }

                            // Required to avoid extended Case creation during SaveCase. Extended case is created later.
                            childCaseModel.ExtendedCaseData_Id = null;
                            childCaseModel.ExtendedCaseForm_Id = null;

                            var childRes = SaveCase(childCaseModel, baseAuxModel, out childCaseId, out childCaseNum);

                            if (childRes.IsSucceed && childCaseId != -1)
                            {
                                if (baseCaseExtendedCaseDataId > 0 && exCaseForm != null)
                                {
                                    _extendedCaseService.CopyExtendedCaseToCase(baseCaseExtendedCaseDataId, childCaseId, baseAuxModel.UserIdentityName, exCaseForm.Id);
                                }

                                //TODO: om fel, visa det
                                _caseService.AddChildCase(childCaseId, selfCaseId, out errors);

                                if (!errors.Any())
                                    _caseService.SetIndependentChild(childCaseId, true);
                            }
                        }
                    }
                }
            }

            caseId = selfCaseId;
            caseNumber = _caseNum;
            return res;
        }

        #region ApplyValuesCaseBindingValuesFromExtendedCase

        // This method is required to set missing caseBinding values that have not been set in MultiForm due to some reasons.
        // One of the reasons is when same caseBinding should be used in several child forms - in that case its not possible to have caseBinding in the MultiForm exCase template and caseBinding should be processed when child case is created.
        private void ApplyCaseBindingValuesFromExtendedCase(CaseModel caseModel, ExtendedCaseFormEntity exCaseForm, int exCaseDataId, ExtendedCaseFormEntity baseCaseExCaseForm)
        {
            var caseBindingKeysToExclude = _extendedCaseService.GetTemplateCaseBindingKeys(baseCaseExCaseForm.Id);
            var caseBindingValues = _extendedCaseService.GetTemplateCaseBindingValues(exCaseForm.Id, exCaseDataId);
            if (caseBindingValues != null && caseBindingValues.Count > 0)
            {
                foreach (var caseBindingValue in caseBindingValues)
                {
                    if (caseBindingKeysToExclude != null && caseBindingKeysToExclude.Contains(caseBindingValue.Key, StringComparer.OrdinalIgnoreCase) ||
                        string.IsNullOrEmpty(caseBindingValue.Value))
                        continue;

                    SetCaseBindingValue(caseModel, caseBindingValue.Key, caseBindingValue.Value);
                }
            }
        }

        //todo: introduce constants for caseBinding keys
        // other places with caseBinding keys: Helpdesk\EditPage.js, SelfService\case.exeteded.js
        private void SetCaseBindingValue(CaseModel caseModel, string caseBinding, string value)
        {
            switch (caseBinding.ToLower().Trim())
            {
                case "administrator_id":
                    caseModel.Performer_User_Id = value.ToInt();
                    break;
                case "reportedby":
                    caseModel.ReportedBy = value;
                    break;
                case "region_id":
                    caseModel.Region_Id = value.ToInt();
                    break;
                case "department_id":
                    caseModel.Department_Id = value.ToInt();
                    break;
                case "ou_id_1":
                    caseModel.OU_Id = value.ToInt();
                    break;
                case "ou_id_2":
                    caseModel.OU_Id = value.ToInt();
                    break;
                case "productarea_id":
                    caseModel.ProductArea_Id = value.ToInt();
                    break;
                case "status_id":
                    caseModel.Status_Id = value.ToInt();
                    break;
                case "substatus_id":
                    caseModel.StateSecondary_Id = value.ToInt();
                    break;
                case "priority_id":
                    caseModel.Priority_Id = value.ToInt();
                    break;
                case "plandate":
                    caseModel.PlanDate = DateTime.ParseExact(value, DateFormats.Date, CultureInfo.InvariantCulture);
                    break;
                case "watchdate":
                    caseModel.WatchDate = DateTime.ParseExact(value, DateFormats.Date, CultureInfo.InvariantCulture);
                    break;
                case "persons_name":
                    caseModel.PersonsName = value;
                    break;
                case "persons_phone":
                    caseModel.PersonsPhone = value;
                    break;
                case "usercode":
                    caseModel.UserCode = value;
                    break;
                case "persons_email":
                    caseModel.PersonsEmail = value;
                    break;
                case "persons_cellphone":
                    caseModel.PersonsCellphone = value;
                    break;
                case "place":
                    caseModel.Place = value;
                    break;
                case "costcentre":
                    caseModel.CostCentre = value;
                    break;
                case "caption":
                    caseModel.Caption = value;
                    break;
                //case "log_textinternal":
                //    caseModel.log_textinternal = value;
                //    break;
                //case "case_relation_type":
                //    caseModel.case_relation_type = value;
                //    break;
                case "inventorytype":
                    caseModel.InventoryType = value;
                    break;
                case "inventorylocation":
                    caseModel.InventoryLocation = value;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
