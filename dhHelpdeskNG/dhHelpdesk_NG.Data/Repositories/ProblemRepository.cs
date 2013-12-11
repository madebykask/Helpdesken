using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region PROBLEM

    public interface IProblemRepository : IRepository<Problem>
    {
    }

    public class ProblemRepository : RepositoryBase<Problem>, IProblemRepository
    {
        public ProblemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROBLEMEMAILLOG

    public interface IProblemEMailLogRepository : IRepository<ProblemEMailLog>
    {
    }

    public class ProblemEMailLogRepository : RepositoryBase<ProblemEMailLog>, IProblemEMailLogRepository
    {
        public ProblemEMailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROBLEMLOG

    public interface IProblemLogRepository : IRepository<ProblemLog>
    {
    }

    public class ProblemLogRepository : RepositoryBase<ProblemLog>, IProblemLogRepository
    {
        public ProblemLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
