using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Common.Enums.BusinessRule;
using static DH.Helpdesk.BusinessData.Models.Shared.ProcessResult;

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
                                    ILogService logService)
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
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel, out int caseId)
        {
            var res = new ProcessResult("Save Case");
            caseId = -1;

            res = PrimaryValidation(ref caseModel);
            if (res.IsSucceed)
            {
                var timeMetrics = new CaseTimeMetricsModel();
                res = CloneCase(ref caseModel, auxModel, out timeMetrics);
                if (res.IsSucceed)
                {
                    res = ValidateCase(caseModel);
                    if (res.IsSucceed)
                    {
                        var emailSettings = GetEmailSettings(caseModel, auxModel);
                        res = DoSaveCase(caseModel, auxModel, timeMetrics, emailSettings, out caseId);
                    }
                }
            }
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

            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                                            _holidayService,
                                            curCustomer.WorkingDayStart,
                                            curCustomer.WorkingDayEnd,
                                            auxModel.UserTimeZone);

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
                _customerUser = _customerUserService.GetCustomerSettings(caseModel.Customer_Id, auxModel.CurrentUserId);
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
                                         CaseMailSetting mailSettings, out int caseId)
        {
            /*TODO: After merge CaseServices case must be sent to the Repository directly from here(No need to use CaseService anymore) */
            /* Convert caseModel to Case entity to make it ready for Save method */
            caseId = -1;
            var oldCase = new Case();
            if (caseModel.Id != 0)
                oldCase = _caseService.GetCaseById(caseModel.Id);

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
            var historyId = _caseService.SaveCase(caseEntity, logEntity, auxModel.CurrentUserId,
                                                  auxModel.UserIdentityName, extraInfo, out errors);

            if (errors.Any())
                return new ProcessResult("Do Save Case", ResultTypeEnum.ERROR, "Save could not be saved!", errors);

            logEntity.CaseId = caseEntity.Id;
            logEntity.CaseHistoryId = historyId;
            var logId = _logService.SaveLog(logEntity, 0, out errors);
            logEntity.Id = logId;

            var curUser = auxModel.CurrentUserId > 0 ? _userService.GetUser(auxModel.CurrentUserId) : null;
            oldCase = oldCase.Id > 0 ? oldCase : null;

            _caseService.SendCaseEmail(caseEntity.Id, mailSettings, historyId, "", auxModel.UserTimeZone,
                                       oldCase, logEntity, null, curUser);

            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, caseEntity, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, caseEntity, logEntity, auxModel.UserTimeZone,
                                                    historyId, "", auxModel.CurrentLanguageId, mailSettings);

            caseId = caseEntity.Id;
            return new ProcessResult("Case saved");
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

        private Case ConvertCaseModelToCase(CaseModel caseModel, Case oldCase)
        {
            var caseEntity = new Case();
            if (oldCase != null && oldCase.Id > 0)
                caseEntity = oldCase;

            #region Update Case properties

            var properties = caseModel.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var type = prop.PropertyType;
                var typeCode = Type.GetTypeCode(type);
                var caseProperty = caseEntity.GetType().GetProperty(prop.Name);
                if (caseProperty != null)
                {
                    switch (typeCode)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            var intVal = (int)prop.GetValue(caseModel, null);
                            caseProperty.SetValue(caseEntity, intVal);
                            break;

                        case TypeCode.String:
                            var strVal = (string)prop.GetValue(caseModel, null);
                            caseProperty.SetValue(caseEntity, strVal);
                            break;

                        case TypeCode.DateTime:
                            var dateVal = (DateTime)prop.GetValue(caseModel, null);
                            caseProperty.SetValue(caseEntity, dateVal);
                            break;

                        case TypeCode.Decimal:
                            var decimalVal = (decimal)prop.GetValue(caseModel, null);
                            caseProperty.SetValue(caseEntity, decimalVal);
                            break;

                        case TypeCode.Object:
                            if (type == typeof(int?))
                            {
                                var nullIntVal = (int?)prop.GetValue(caseModel, null);
                                caseProperty.SetValue(caseEntity, nullIntVal);
                            }
                            else
                            if (type == typeof(DateTime?))
                            {
                                var nullDateVal = (DateTime?)prop.GetValue(caseModel, null);
                                caseProperty.SetValue(caseEntity, nullDateVal);
                            }
                            else
                            if (type == typeof(Guid))
                            {
                                var guidVal = (Guid)prop.GetValue(caseModel, null);
                                caseProperty.SetValue(caseEntity, guidVal);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            #endregion

            #region Update CaseIsAbout properties

            var isAboutChanged = false;
            var isAbout = new CaseIsAboutEntity();
            if (caseEntity.IsAbout != null)
                isAbout = caseEntity.IsAbout;

            if (caseModel.IsAbout_ReportedBy != oldCase.IsAbout?.ReportedBy)
            {
                isAbout.ReportedBy = caseModel.IsAbout_ReportedBy;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_PersonsName != oldCase.IsAbout?.Person_Name)
            {
                isAbout.Person_Name = caseModel.IsAbout_PersonsName;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_PersonsEmail != oldCase.IsAbout?.Person_Email)
            {
                isAbout.Person_Email = caseModel.IsAbout_PersonsEmail;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_PersonsPhone != oldCase.IsAbout?.Person_Phone)
            {
                isAbout.Person_Phone = caseModel.IsAbout_PersonsPhone;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_PersonsCellPhone != oldCase.IsAbout?.Person_Cellphone)
            {
                isAbout.Person_Cellphone = caseModel.IsAbout_PersonsCellPhone;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_Place != oldCase.IsAbout?.Place)
            {
                isAbout.Place = caseModel.IsAbout_Place;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_UserCode != oldCase.IsAbout?.UserCode)
            {
                isAbout.UserCode = caseModel.IsAbout_UserCode;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_Region_Id != oldCase.IsAbout?.Region_Id)
            {
                isAbout.Region_Id = caseModel.IsAbout_Region_Id;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_Department_Id != oldCase.IsAbout?.Department_Id)
            {
                isAbout.Department_Id = caseModel.IsAbout_Department_Id;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_OU_Id != oldCase.IsAbout?.OU_Id)
            {
                isAbout.OU_Id = caseModel.IsAbout_OU_Id;
                isAboutChanged = true;
            }
            if (caseModel.IsAbout_CostCentre != oldCase.IsAbout?.CostCentre)
            {
                isAbout.CostCentre = caseModel.IsAbout_CostCentre;
                isAboutChanged = true;
            }

            if (!isAboutChanged)
                isAbout = null;

            caseEntity.IsAbout = isAbout;
            #endregion

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

                //// aux model values
                UserId = auxCaseModel.CurrentUserId > 0 ? (int?)auxCaseModel.CurrentUserId : null,
                RegUser = auxCaseModel.UserIdentityName ?? String.Empty
                
            };

            return ret;
        }
    }
}
