namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region OULANGUAGE

    public interface IOULanguageRepository : IRepository<OULanguage>
    {
    }

    public class OULanguageRepository : RepositoryBase<OULanguage>, IOULanguageRepository
    {
        public OULanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
