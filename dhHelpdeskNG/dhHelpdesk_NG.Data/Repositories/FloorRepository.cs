namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
