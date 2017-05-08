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

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public class UniversalCaseService: IUniversalCaseService
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
        private IList<CustomKeyValue<string,string>> _localTranslations;

        private readonly ICaseRepository _caseRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ITextTranslationService _textTranslationService;

        public UniversalCaseService(ICaseRepository caseRepository,
                                    ICustomerService customerService,
                                    IWorkingGroupService workingGroupService,
                                    ICaseFieldSettingService caseFieldSettingService,
                                    ITextTranslationService textTranslationService)
        {
            _caseRepository = caseRepository;
            _customerService = customerService;
            _workingGroupService = workingGroupService;
            _caseFieldSettingService = caseFieldSettingService;
            _textTranslationService = textTranslationService;
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

            _currentLanguageId = auxModel.CurrentLanguageId;
            _UNIT_TEXTS = new string[] { _INVALID_TEXT, _INVALID_EMPTY_TEXT, _CASE_TEXT };
            _localTranslations = _textTranslationService.GetTranslationsFor(_UNIT_TEXTS.ToList(), _currentLanguageId);
            _caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(caseModel.Customer_Id, _currentLanguageId)
                                                         .ToList();

            return new ProcessResult("Case cloned.");
        }

        private ProcessResult ValidateCase(CaseModel caseModel)
        {
            if (!_caseFieldSettings.Any())
                return new ProcessResult("Case Validation", ResultTypeEnum.ERROR, string.Format("{0} {1}", TranslateLocally(_CASE_TEXT), TranslateLocally(_INVALID_TEXT)));

            var retData = new List<KeyValuePair<string, string>>();

            #region  case validation
            
            if (caseModel.Id == 0)
            {                
                if (!caseModel.CaseType_Id.IsValueChanged())
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.CaseType_Id));
            }

            if (caseModel.FinishingType_Id.IsValueChanged() && caseModel.FinishingType_Id > 0)
            {
                if (!caseModel.FinishingDate.HasValue || !caseModel.FinishingDate.IsValueChanged())
                    retData.Add(GenerateCantBeNullOrEmptyMessage(GlobalEnums.TranslationCaseFields.FinishingDate));
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
    }
}
