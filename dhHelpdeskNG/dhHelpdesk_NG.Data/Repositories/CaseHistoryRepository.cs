namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseHistoryRepository : IRepository<CaseHistory>
    {
        IEnumerable<CaseHistory> GetCaseHistoryByCaseId(int caseId);

        void SetNullProblemByProblemId(int problemId);

        CaseHistory GetCloneOfLatest(int caseId);
        CaseHistory GetCloneOfPenultimate(int caseId);
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
            var cases = this.DataContext.CaseHistories.Where(x => x.Problem_Id == problemId).ToList();

            foreach (var item in cases)
            {
                item.Problem_Id = null;
            }
        }

        public CaseHistory GetCloneOfLatest(int caseId)
        {
            return this.DataContext.Set<CaseHistory>()
                .AsNoTracking()
                .Where(it => it.Case_Id == caseId)
                .OrderByDescending(it => it.Id)
                .FirstOrDefault();
        }

        public CaseHistory GetCloneOfPenultimate(int caseId)
        {
            return DataContext.Set<CaseHistory>()
                .AsNoTracking()
                .Where(it => it.Case_Id == caseId)
                .OrderByDescending(it => it.Id)
                .Skip(1).Take(1)
                .FirstOrDefault();
        }
    }
}
