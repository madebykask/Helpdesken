namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Documents
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Domain;
    
    public static class DocumentSpecifications
    {
        public static IQueryable<Document> GetForStartPage(
                                   this IQueryable<Document> query,
                                   int[] customers,
                                   int? count,
                                   bool forStartPage)
        {
            query = query.GetByCustomers(customers);

            if (forStartPage)
            {
                query = query.Where(d => d.ShowOnStartPage);
            }

            query = query.SortByCreated();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }
    }
}