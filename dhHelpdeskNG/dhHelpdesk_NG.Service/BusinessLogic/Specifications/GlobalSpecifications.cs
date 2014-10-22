namespace DH.Helpdesk.Services.BusinessLogic.Specifications
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure.Context;
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

        public static IQueryable<T> GetForStartPage<T>(
                                    this IQueryable<T> query,
                                    int[] customers,
                                    int? count,
                                    bool forStartPage)
            where T : class, ICustomerEntity, IStartPageEntity
        {
            if (customers != null && customers.Any())
            {
                query = query.Where(x => customers.Contains(x.Customer_Id));
            }

            if (forStartPage)
            {
                query = query.Where(x => x.ShowOnStartPage == 1);
            }

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }

        public static IQueryable<T> RestrictByWorkingGroups<T>(this IQueryable<T> query, IWorkContext workContext)
            where T : class, IWorkingGroupEntity
        {
            var userGroups = workContext.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id);

            query = query.Where(x => !x.WGs.Any() || x.WGs.Any(g => userGroups.Contains(g.Id)));

            return query;
        }

        public static IQueryable<T> RestrictByUsers<T>(this IQueryable<T> query, IWorkContext workContext)
            where T : class, IUserEntity
        {
            query = query.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == workContext.User.UserId));

            return query;
        }

        public static IQueryable<T> RestrictByWorkingGroupsAndUsers<T>(this IQueryable<T> query, IWorkContext workContext)
            where T : class, IWorkingGroupEntity, IUserEntity
        {
            query = query.RestrictByWorkingGroups(workContext)
                        .RestrictByUsers(workContext);

            return query;
        } 
    }
}
