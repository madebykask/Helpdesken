
using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.Services.Services.UniversalCase
{
    public interface IUniversalCaseService
    {
        CaseModel GetCase(int id);

        string SaveCase(CaseModel caseModel, AuxCaseModel auxModel);
    }
}
