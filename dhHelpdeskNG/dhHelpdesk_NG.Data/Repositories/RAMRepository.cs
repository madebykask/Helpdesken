using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IRAMRepository : IRepository<RAM>
	{
	}

	public class RAMRepository : RepositoryBase<RAM>, IRAMRepository
	{
		public RAMRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
