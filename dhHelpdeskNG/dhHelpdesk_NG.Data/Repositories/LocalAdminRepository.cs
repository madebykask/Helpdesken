namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILocalAdminRepository : IRepository<LocalAdmin>
	{
	}

	public class LocalAdminRepository : RepositoryBase<LocalAdmin>, ILocalAdminRepository
	{
		public LocalAdminRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
