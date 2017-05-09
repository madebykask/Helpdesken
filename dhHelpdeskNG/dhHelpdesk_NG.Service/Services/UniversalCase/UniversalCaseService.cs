using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using static DH.Helpdesk.BusinessData.Models.Shared.ProcessResult;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Common.Tools;

namespace DH.Helpdesk.Services.Services.UniversalCase
{   
    public class UniversalCaseService: IUniversalCaseService
    {

        private class CaseTimeMetrics
        {
            public int ExternalTime { get; set; }
            public int ActionExternalTime { get; set; }
            public int LeadTime { get; set; }
            public int LeadTimeForNow { get; set; }
            public int ActionLeadTime { get; set; }
            public DateTime? LatestSLACountDate { get; set; }
        }

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
        private IList<CustomKeyValue<string,string>> _localTranslations;
        private CustomerUser _customerUser;

        private readonly ICaseRepository _caseRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ITextTranslationService _textTranslationService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ISettingService _settingService;
        private readonly IHolidayService _holidayService;


        public UniversalCaseService(ICaseRepository caseRepository,
                                    ICustomerService customerService,
                                    IWorkingGroupService workingGroupService,
                                    ICaseFieldSettingService caseFieldSettingService,
                                    ITextTranslationService textTranslationService,
                                    ICustomerUserService customerUserService,
                                    IStateSecondaryService stateSecondaryService,
                                    ISettingService settingService,
                                    IHolidayService holidayService)
        {
            _caseRepository = caseRepository;
            _customerService = customerService;
            _workingGroupService = workingGroupService;
            _caseFieldSettingService = caseFieldSettingService;
            _textTranslationService = textTranslationService;
            _customerUserService = customerUserService;
            _stateSecondaryService = stateSecondaryService;
            _settingService = settingService;
            _holidayService = holidayService;
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel)
        {
            var pValidation = PrimaryValidate(ref caseModel);
            if (!pValidation.IsSucceed)
                return pValidation;

            var res = CloneCase(ref caseModel, auxModel);            
            if (res.IsSucceed)
            {
                var _validationRes = ValidateCase(caseModel);
                var mailSender = GetMailSenders(caseModel);

                return new ProcessResult("Save Case");
            }
            else
                return res;
        }

        private ProcessResult PrimaryValidate(ref CaseModel caseModel)
        {
            /*In this method you can't translate messages (Use validation instead) */

            var retData = new List<KeyValuePair<string, string>>();

            #region Primary validation

            if (!caseModel.Id.IsValueChanged() || caseModel.Id < 0)
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

                if (!caseModel.Customer_Id.IsValueChanged() || caseModel.Customer_Id <= 0)
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

        private ProcessResult CloneCase(ref CaseModel caseModel, AuxCaseModel auxModel)
        {
            var isEditMode = caseModel.Id != 0;

            var retData = new List<KeyValuePair<string, string>>();

            if (caseModel.PerformerUser_Id == 0)
                caseModel.PerformerUser_Id = null;

            if (caseModel.CaseResponsibleUser_Id == 0)
                caseModel.CaseResponsibleUser_Id = null;

            if (caseModel.RegLanguage_Id == 0)
                caseModel.RegLanguage_Id = auxModel.CurrentLanguageId;

            if (caseModel.Id == 0)
            {
                caseModel.CaseGuid = Guid.NewGuid();
                if (!caseModel.ContactBeforeAction.IsValueChanged())
                    caseModel.ContactBeforeAction = _DEFAULT_CONTACT_BEFORE_ACTION;

                if (!caseModel.Cost.IsValueChanged())
                    caseModel.Cost = _DEFAULT_COST;

                if (!caseModel.Deleted.IsValueChanged())
                    caseModel.Deleted = _DEFAULT_DELETED;

                if (!caseModel.ExternalTime.IsValueChanged())
                    caseModel.ExternalTime = _DEFAULT_EXTERNAL_TIME;

                if (!caseModel.NoMailToNotifier.IsValueChanged())
                    caseModel.NoMailToNotifier = _DEFAULT_NO_MAIL_TO_NOTIFIER;

                if (!caseModel.OtherCost.IsValueChanged())
                    caseModel.OtherCost = _DEFAULT_OTHER_COST;

                if (!caseModel.SMS.IsValueChanged())
                    caseModel.SMS = _DEFAULT_SMS;
            }

            /*Load required info*/
            _currentLanguageId = auxModel.CurrentLanguageId;
            _UNIT_TEXTS = new string[] { _INVALID_TEXT, _INVALID_EMPTY_TEXT, _CASE_TEXT };
            _localTranslations = _textTranslationService.GetTranslationsFor(_UNIT_TEXTS.ToList(), _currentLanguageId);
            _caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(caseModel.Customer_Id, _currentLanguageId).ToList();
            _customerUser = _customerUserService.GetCustomerSettings(caseModel.Customer_Id, auxModel.CurrentUserId);

            var oldCase = new CaseModel();
            /*Apply rules*/
            if (_caseFieldSettings.FirstOrDefault(f => f.Name == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()) == null)
                caseModel.PersonsEmail = string.Empty;

            if (isEditMode)
            {
                oldCase = _caseRepository.GetCase(caseModel.Id);

                if (_customerUser.UserInfoPermission == 0)
                {
                    caseModel.ReportedBy = NotChangedValue.STRING;
                    caseModel.Place = NotChangedValue.STRING;
                    caseModel.PersonsName = NotChangedValue.STRING;
                    caseModel.PersonsEmail = NotChangedValue.STRING;
                    caseModel.PersonsPhone = NotChangedValue.STRING;
                    caseModel.PersonsCellPhone = NotChangedValue.STRING;
                    caseModel.Region_Id = NotChangedValue.NULLABLE_INT;
                    caseModel.Department_Id = NotChangedValue.NULLABLE_INT;
                    caseModel.OU_Id = NotChangedValue.NULLABLE_INT;
                    caseModel.UserCode = NotChangedValue.STRING;
                }
                caseModel.RegTime = NotChangedValue.DATETIME;
            }
            else
            {
                caseModel.RegTime = auxModel.UtcNow;
            }

            if (caseModel.FinishingType_Id.IsValueChanged() && caseModel.FinishingType_Id > 0)
            {
                if (!caseModel.FinishingDate.HasValue || !caseModel.FinishingDate.IsValueChanged())
                    caseModel.FinishingDate = auxModel.UtcNow;
                else                
                {                    
                    if (caseModel.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        caseModel.FinishingDate = auxModel.UtcNow;
                    }
                    else if (oldCase != null && oldCase.ChangedTime.ToShortDateString() == caseModel.FinishingDate.Value.ToShortDateString())
                    {
                        var lastChangedTime = new DateTime(oldCase.ChangedTime.Year, oldCase.ChangedTime.Month, oldCase.ChangedTime.Day, 22, 59, 59);
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
                if (!caseModel.CaseType_Id.IsValueChanged())
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.CaseType_Id));
            }            

            if (caseModel.FinishingDate.IsValueChanged() && caseModel.FinishingDate.HasValue)
            {
                if (!caseModel.FinishingType_Id.HasValue || !caseModel.FinishingType_Id.IsValueChanged())
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.ClosingReason));
            }

            if (retData.Any())
                return new ProcessResult("Case Validation", ResultTypeEnum.ERROR, "Case is not valid!", retData);


            #endregion

            return new ProcessResult("Case Validated.");
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

        private MailSenders GetMailSenders(CaseModel caseModel)
        {
            var mailSenders = new MailSenders();
            if (caseModel.WorkingGroup_Id.HasValue && caseModel.WorkingGroup_Id.IsValueChanged())
            {
                var curWG = _workingGroupService.GetWorkingGroup(caseModel.WorkingGroup_Id.Value);
                mailSenders.WGEmail = curWG.EMail;
            }

            if (caseModel.DefaultOwnerWG_Id.HasValue && caseModel.DefaultOwnerWG_Id.IsValueChanged())
            {
                var curWG = _workingGroupService.GetWorkingGroup(caseModel.DefaultOwnerWG_Id.Value);
                mailSenders.DefaultOwnerWGEMail = curWG.EMail;
            }

            return mailSenders;
        }

        private CaseTimeMetrics GetClaculatedTimes(CaseModel caseModel, AuxCaseModel auxModel, CaseModel oldCase = null)
        {
            var ret = new CaseTimeMetrics
            {
                ExternalTime = 0,
                ActionExternalTime = 0,
                LeadTime = 0,
                LeadTimeForNow =0,
                ActionLeadTime = 0,
                LatestSLACountDate = null
            };

            var isEditMode = caseModel.Id != 0;
            var curCustomer = _customerService.GetCustomer(caseModel.Customer_Id);
            var setting = _settingService.GetCustomerSetting(curCustomer.Id);
            var customerTimeOffset = setting.TimeZone_offset;

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
                        var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangedTime, auxModel.UtcNow, deptIds);
                        ret.ExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangedTime,
                            auxModel.UtcNow,
                            oldCase.Department_Id) + oldCase.ExternalTime;                       

                        workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangedTime, auxModel.UtcNow, deptIds, customerTimeOffset);
                        ret.ActionExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangedTime,
                            auxModel.UtcNow,
                            oldCase.Department_Id, customerTimeOffset);
                    }
                }
            }
            #endregion

            #region LeadTime & ActionLeadTime

            var isCaseGoingToFinish = caseModel.FinishingType_Id.IsValueChanged() &&
                                      caseModel.FinishingType_Id.HasValue &&
                                      caseModel.FinishingType_Id.Value > 0;

            DateTime uBoundDate = isCaseGoingToFinish? caseModel.FinishingDate.Value : auxModel.UtcNow;
                        
            /*Always claculate LeadTime (FinishingTime|UtcNow)*/             
            var leadWorkTimeCalc = workTimeCalcFactory.Build(caseModel.RegTime, uBoundDate, deptIds);
            var leadTime = leadWorkTimeCalc.CalculateWorkTime(
                caseModel.RegTime,
                uBoundDate.ToUniversalTime(),
                caseModel.Department_Id) - ret.ExternalTime;

            ret.LeadTime = isCaseGoingToFinish? leadTime : NotChangedValue.INT;
            ret.LeadTimeForNow = leadTime;

            if (oldCase != null && oldCase.Id > 0)
            {
                var actionLeadWorkTimeCalc = workTimeCalcFactory.Build(oldCase.ChangedTime, uBoundDate, deptIds, customerTimeOffset);
                var actionLeadTime = actionLeadWorkTimeCalc.CalculateWorkTime(
                    oldCase.ChangedTime,
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
    }
}
