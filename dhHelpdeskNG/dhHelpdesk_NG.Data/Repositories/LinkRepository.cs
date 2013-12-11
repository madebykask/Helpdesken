using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
