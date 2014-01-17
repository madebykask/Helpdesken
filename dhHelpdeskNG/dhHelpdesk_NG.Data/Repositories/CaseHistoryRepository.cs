using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
        IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId);

        void SetNullProblemByProblemId(int problemId);
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
            return q.OrderBy(l => l.Id);
        }

        public void SetNullProblemByProblemId(int problemId)
        {
            var cases =
                this.DataContext.CaseHistories.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.Problem_Id = null;
            }
        }
    }
}
