using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IFloorRepository : IRepository<Floor>
    {
    }

    public class FloorRepository : RepositoryBase<Floor>, IFloorRepository
    {
        public FloorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
