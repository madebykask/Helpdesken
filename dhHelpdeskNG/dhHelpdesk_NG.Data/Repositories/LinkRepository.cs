namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILinkRepository : IRepository<Link>
	{
	}

	public class LinkRepository : RepositoryBase<Link>, ILinkRepository
	{
		public LinkRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}
	}
}
