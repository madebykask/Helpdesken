using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region URGENCY

    public interface IUrgencyRepository : IRepository<Urgency>
    {
    }

    public class UrgencyRepository : RepositoryBase<Urgency>, IUrgencyRepository
    {
        public UrgencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region URGENCYLANGUAGE

    public interface IUrgencyLanguageRepository : IRepository<UrgencyLanguage>
    {
    }

    public class UrgencyLanguageRepository : RepositoryBase<UrgencyLanguage>, IUrgencyLanguageRepository
    {
        public UrgencyLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
