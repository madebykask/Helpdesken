namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
