namespace DH.Helpdesk.Services.BusinessLogic.Specifications
{
    using System.Linq;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Interfaces;

    public static class GlobalSpecifications
    {
        public static IQueryable<T> GetByCustomer<T>(this IQueryable<T> query, int customerId)
            where T : class, ICustomerEntity
        {
            query = query.Where(x => x.Customer_Id == customerId);

            return query;
        }

        public static IQueryable<T> GetByCustomer<T>(this IQueryable<T> query, int? customerId)
            where T : class, INulableCustomerEntity
        {
            query = query.Where(x => customerId == null ? x.Customer_Id == null : x.Customer_Id == customerId);

            return query;
        }

        public static IQueryable<T> GetById<T>(this IQueryable<T> query, int id)
            where T : Entity
        {
            query = query.Where(x => x.Id == id);

            return query;
        } 
    }
}
