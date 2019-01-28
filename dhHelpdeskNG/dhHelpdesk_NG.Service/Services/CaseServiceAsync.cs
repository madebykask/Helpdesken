using System.Collections.Generic;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.MapperData.CaseHistory;

namespace DH.Helpdesk.Services.Services
{
    public partial class CaseService
    {
        public Task<List<CaseHistoryMapperData>> GetCaseHistoriesAsync(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoriesAsync(caseId);
        }
    }
}