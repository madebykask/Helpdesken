using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
	public interface ILogicalDriveRepository : IRepository<LogicalDrive>
	{
	}

	public class LogicalDriveRepository : RepositoryBase<LogicalDrive>, ILogicalDriveRepository
	{
		public LogicalDriveRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
