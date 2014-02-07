namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IApplicationRepository : IRepository<Application>
    {
    }

    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
