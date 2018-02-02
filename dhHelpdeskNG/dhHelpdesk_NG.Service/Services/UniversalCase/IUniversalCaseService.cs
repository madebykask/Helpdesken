using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public interface IUniversalCaseService
    {
        CaseModel GetCase(int id);

        ProcessResult SaveCase(CaseModel caseModel, AuxCaseModel auxModel, out int caseId, out decimal caseNumber);

        CaseTimeMetricsModel ClaculateCaseTimeMetrics(CaseModel caseModel, AuxCaseModel auxModel, CaseModel oldCase = null);

        ProcessResult SaveCaseCheckSplit(CaseModel parentCaseModel, AuxCaseModel parentAuxModel, out int caseId, out decimal caseNumber);
    }
}
