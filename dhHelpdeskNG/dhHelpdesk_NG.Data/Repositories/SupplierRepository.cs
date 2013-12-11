using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
