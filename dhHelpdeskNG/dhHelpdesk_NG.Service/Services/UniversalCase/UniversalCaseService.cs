using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static DH.Helpdesk.BusinessData.Models.Shared.ProcessResult;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public class UniversalCaseService: IUniversalCaseService
    {
        private const int _DEFAULT_CONTACT_BEFORE_ACTION = 0;
        private const int _DEFAULT_COST = 0;
        private const int _DEFAULT_DELETED = 0;
        private const int _DEFAULT_EXTERNAL_TIME = 0;
        private const int _DEFAULT_NO_MAIL_TO_NOTIFIER = 1;
        private const int _DEFAULT_OTHER_COST = 0;
        private const int _DEFAULT_SMS = 0;

        private readonly ICaseRepository _caseRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkingGroupService _workingGroupService;

        public UniversalCaseService(ICaseRepository caseRepository,
                                    ICustomerService customerService,
                                    IWorkingGroupService workingGroupService)
        {
            _caseRepository = caseRepository;
            _customerService = customerService;
            _workingGroupService = workingGroupService;
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel)
        {
            var _caseModel = CloneCase(caseModel, auxModel);
            var _validationRes = ValidateCase(caseModel);
            if (_validationRes.IsSucceed)
                //Save Case
                return new ProcessResult("Save Case");
            else
                return _validationRes;
        }

        private CaseModel CloneCase(CaseModel caseModel, AuxCaseModel auxModel)
        {
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
            
            return caseModel;
        }

        private ProcessResult ValidateCase(CaseModel caseModel)
        {
            var retData = new List<KeyValuePair<string, string>>();

            #region  Primary validation

            if (!caseModel.Id.IsValueChanged())            
                retData.Add(new KeyValuePair<string, string>("Case_Id", "Case Id is not specified!"));                
            
            if (caseModel.Id < 0)            
                retData.Add(new KeyValuePair<string, string>("Case_Id", string.Format("Case Id ({0}) is not valid!", caseModel.Id)));            

            if (caseModel.Id == 0)
            {
                if (!caseModel.Customer_Id.IsValueChanged())
                    retData.Add(new KeyValuePair<string, string>("Customer_Id", "Customer Id is not specified!"));
                else if (caseModel.Customer_Id <= 0)
                    retData.Add(new KeyValuePair<string, string>("Customer_Id", string.Format("Customer Id ({0}) is not valid!", caseModel.Customer_Id)));

                if (!caseModel.CaseType_Id.IsValueChanged())
                    retData.Add(new KeyValuePair<string, string>("CaseType_Id", "CaseType Id is not specified!"));                
            }

            if (caseModel.FinishingType_Id.IsValueChanged() && caseModel.FinishingType_Id > 0)
            {
                if (!caseModel.FinishingDate.HasValue || !caseModel.FinishingDate.IsValueChanged())
                    retData.Add(new KeyValuePair<string, string>("FinishingDate", "FinishingDate can not be empty or null!"));                
            }

            if (caseModel.FinishingDate.IsValueChanged() && caseModel.FinishingDate.HasValue)
            {
                if (!caseModel.FinishingType_Id.HasValue || !caseModel.FinishingType_Id.IsValueChanged())
                    retData.Add(new KeyValuePair<string, string>("FinishingType_Id", "FinishingType_Id can not be null!"));
            }

            if (retData.Any())            
               return new ProcessResult("CaseValidation", ResultTypeEnum.ERROR, "Case is not valid!", retData);            


            #endregion

            return new ProcessResult("CaseValidation");            
        }
         
    }
}
