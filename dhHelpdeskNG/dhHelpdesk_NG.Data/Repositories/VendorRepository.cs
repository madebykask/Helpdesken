namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
