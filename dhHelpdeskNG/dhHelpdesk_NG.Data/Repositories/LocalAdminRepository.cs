using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
