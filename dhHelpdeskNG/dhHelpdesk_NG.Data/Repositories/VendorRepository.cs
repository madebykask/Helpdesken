using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IVendorRepository : IRepository<Vendor>
	{
	}

	public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
	{
		public VendorRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
