namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
