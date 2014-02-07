namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
