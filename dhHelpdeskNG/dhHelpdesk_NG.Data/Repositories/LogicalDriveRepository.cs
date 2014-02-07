namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
