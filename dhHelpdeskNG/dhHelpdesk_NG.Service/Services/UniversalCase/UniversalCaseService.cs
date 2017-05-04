

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories;
using System.Collections.Generic;
using System.Linq;
using static DH.Helpdesk.BusinessData.Models.Shared.ProcessResult;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public class UniversalCaseService: IUniversalCaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICustomerService _customerService;

        public UniversalCaseService(ICaseRepository caseRepository,
                                    ICustomerService customerService)
        {
            _caseRepository = caseRepository;
            _customerService = customerService;
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel)
        {
            var _caseModel = Clone(caseModel, auxModel);
            return new ProcessResult("SaveCase");
        }


        private CaseModel Clone(CaseModel caseModel, AuxCaseModel auxModel)
        {
            if (caseModel.PerformerUser_Id == 0)
                caseModel.PerformerUser_Id = null;

            if (caseModel.CaseResponsibleUser_Id == 0)
                caseModel.CaseResponsibleUser_Id = null;

            if (caseModel.RegLanguage_Id == 0)
                caseModel.RegLanguage_Id = auxModel.CurrentLanguageId;

            return caseModel;
        }

        private ProcessResult ValidateCase(CaseModel caseModel)
        {
            var retData = new List<KeyValuePair<string, string>>();

            #region  Primary validation

            if (caseModel.Id == 0 && caseModel.Customer_Id <= 0)
                retData.Add(new KeyValuePair<string, string>("Customer_Id", string.Format("Customer Id ({0}) is not valid!", caseModel.Customer_Id)));
            
            if (retData.Any())
            {
               return new ProcessResult("CaseValidation", ResultTypeEnum.ERROR, "Case is not valid!", retData);
            }

            #endregion


            return new ProcessResult("CaseValidation");            
        }
         
    }
}
