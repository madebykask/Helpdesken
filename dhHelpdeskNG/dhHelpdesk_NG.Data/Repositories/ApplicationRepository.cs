using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
