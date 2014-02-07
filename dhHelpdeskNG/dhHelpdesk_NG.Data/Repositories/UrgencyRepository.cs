namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
