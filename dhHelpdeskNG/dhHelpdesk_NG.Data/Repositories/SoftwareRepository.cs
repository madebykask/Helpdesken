using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface ISoftwareRepository : IRepository<Software>
	{
	}

	public class SoftwareRepository : RepositoryBase<Software>, ISoftwareRepository
	{
		public SoftwareRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
