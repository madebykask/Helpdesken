using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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