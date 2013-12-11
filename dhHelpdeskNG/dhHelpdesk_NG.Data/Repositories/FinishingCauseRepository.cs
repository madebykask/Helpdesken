using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region FINISHINGCAUSE

    public interface IFinishingCauseRepository : IRepository<FinishingCause>
    {
    }

    public class FinishingCauseRepository : RepositoryBase<FinishingCause>, IFinishingCauseRepository
    {
        public FinishingCauseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region FINISHINGCAUSECATEGORY

    public interface IFinishingCauseCategoryRepository : IRepository<FinishingCauseCategory>
    {
    }

    public class FinishingCauseCategoryRepository : RepositoryBase<FinishingCauseCategory>, IFinishingCauseCategoryRepository
    {
        public FinishingCauseCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
