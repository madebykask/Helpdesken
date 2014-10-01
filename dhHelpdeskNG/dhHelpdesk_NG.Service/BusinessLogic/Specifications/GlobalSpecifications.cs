namespace DH.Helpdesk.Services.BusinessLogic.Specifications
{
    using System.Linq;

    using DH.Helpdesk.Domain.Interfaces;

    public static class GlobalSpecifications
    {
        public static IQueryable<T> GetByCustomer<T>(this IQueryable<T> query, int customerId)
            where T : class, ICustomerEntity
        {
            query = query.Where(x => x.Customer_Id == customerId);

            return query;
        }
    }
}
