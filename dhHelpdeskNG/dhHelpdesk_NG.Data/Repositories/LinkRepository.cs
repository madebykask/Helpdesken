using System.Linq;
using DH.Helpdesk.BusinessData.Models.Link.Output;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILinkRepository : IRepository<Link>
    {
        IEnumerable<LinkOverview> GetLinkOverviews(int[] customers);
    }

	public class LinkRepository : RepositoryBase<Link>, ILinkRepository
	{
		public LinkRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{
		}

	    public IEnumerable<LinkOverview> GetLinkOverviews(int[] customers)
	    {
	        return DataContext.Links
	            .Where(l => l.Customer_Id.HasValue && customers.Contains(l.Customer_Id.Value))
	            .Select(l => new LinkOverview()
	            {
	                Customer_Id = l.Customer_Id,
                    CustomerName = l.Customer != null ? l.Customer.Name : null,
                    URLName = l.URLName,
                    URLAddress = l.URLAddress,
                    LinkGroup_Id = l.LinkGroup_Id,
                    LinkGroupName = l.LinkGroup != null ? l.LinkGroup.LinkGroupName : null
	            });
	    }
	}

   
}
