using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IImpactRepository : IRepository<Impact>
    {
    }

    public class ImpactRepository : RepositoryBase<Impact>, IImpactRepository
    {
        public ImpactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
