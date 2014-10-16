namespace DH.Helpdesk.Services.BusinessLogic.Specifications
{
    using System;
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

        public static IQueryable<T> GetById<T>(this IQueryable<T> query, int id) where T : Entity
        {
            query = query.Where(x => x.Id == id);

            return query;
        }

        public static IQueryable<T> GetByLanguage<T>(this IQueryable<T> query, int languageId)
            where T : class, ILanguageEntity
        {
            query = query.Where(x => x.Language_Id == languageId);

            return query;
        }

        public static IQueryable<T> GetByGuid<T>(this IQueryable<T> query, Guid guid)
            where T : class, IGuidEntity
        {
            query = query.Where(x => x.Guid == guid);

            return query;
        }
    }
}
