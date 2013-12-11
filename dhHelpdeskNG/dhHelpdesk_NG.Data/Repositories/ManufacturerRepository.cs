using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IManufacturerRepository : IRepository<Manufacturer>
	{
	}

	public class ManufacturerRepository : RepositoryBase<Manufacturer>, IManufacturerRepository
	{
		public ManufacturerRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
