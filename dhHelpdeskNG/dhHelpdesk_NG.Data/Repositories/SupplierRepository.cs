namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ISupplierRepository : IRepository<Supplier>
	{
	}

	public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
	{
		public SupplierRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
