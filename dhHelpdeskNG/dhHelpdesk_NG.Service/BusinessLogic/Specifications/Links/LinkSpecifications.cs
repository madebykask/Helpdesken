namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Links
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class LinkSpecifications
    {
        public static IQueryable<Link> GetLinksForStartPage(
                                   this IQueryable<Link> query,
                                   int[] customers,
                                   int? count,
                                   bool forStartPage)
        {
            query = query.Where(l => l.Customer_Id.HasValue && customers.Contains(l.Customer_Id.Value));

            if (forStartPage)
            {
                query = query.Where(l => l.ShowOnStartPage == 1);
            }

            query = query.OrderBy(l => l.Customer.Name)
                        .ThenBy(l => l.SortOrder);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }
    }
}