namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Customers
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class CustomerSpecifications
    {
        public static IQueryable<Customer> GetByIds(this IQueryable<Customer> query, int[] ids)
        {
            if (ids == null || !ids.Any())
            {
                return query;
            }

            query = query.Where(c => ids.Contains(c.Id));

            return query;
        } 
    }
}