

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Repositories;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public class UniversalCaseService: IUniversalCaseService
    {
        private readonly ICaseRepository _caseRepository;

        public UniversalCaseService(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public CaseModel GetCase(int id)
        {
            return _caseRepository.GetCase(id);
        }

        public string SaveCase(CaseModel caseModel, AuxCaseModel auxModel)
        {
            return "";
        }
    }
}
