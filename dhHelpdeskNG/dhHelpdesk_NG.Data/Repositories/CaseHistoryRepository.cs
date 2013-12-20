using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
        IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId);
    }

    public class CaseHistoryRepository : RepositoryBase<CaseHistory>, ICaseHistoryRepository
    {
        public CaseHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            var q = (from ch in this.DataContext.CaseHistories
                     where ch.Case_Id == caseId
                     select ch);
            return q.OrderByDescending(l => l.Id);
        }

    }
}
