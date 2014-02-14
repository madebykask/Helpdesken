namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
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
