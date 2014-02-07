namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IBuildingRepository : IRepository<Building>
    {
    }

    public class BuildingRepository : RepositoryBase<Building>, IBuildingRepository
    {
        public BuildingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}