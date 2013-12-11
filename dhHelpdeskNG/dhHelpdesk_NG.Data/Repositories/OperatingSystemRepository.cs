using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface IOperatingSystemRepository : IRepository<OperatingSystem>
	{
	}

	public class OperatingSystemRepository : RepositoryBase<OperatingSystem>, IOperatingSystemRepository
	{
		public OperatingSystemRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
