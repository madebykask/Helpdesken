using DH.Helpdesk.BusinessData.Enums.Users;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications
{
    using System;
    using System.Collections.Generic;
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

        public static IQueryable<T> GetByCustomers<T>(this IQueryable<T> query, int[] customerIds)
            where T : class, ICustomerEntity
        {
            if (customerIds != null && customerIds.Any())
            {
                query = query.Where(x => customerIds.Contains(x.Customer_Id));                
            }

            return query;
        }

        public static IQueryable<T> GetByNullableCustomer<T>(this IQueryable<T> query, int? customerId)
            where T : class, INulableCustomerEntity
        {
            query = query.Where(x => customerId == null ? x.Customer_Id == null : x.Customer_Id == customerId);

            return query;
        }

        public static IQueryable<T> GetById<T>(this IQueryable<T> query, int? id) where T : Entity
        {
            if (id.HasValue)
            {
                query = query.Where(x => x.Id == id);
            }

            return query;
        }

        public static IQueryable<T> GetByIds<T>(this IQueryable<T> query, List<int> ids) where T : Entity
        {
            if (ids != null && ids.Any())
            {
                query = query.Where(x => ids.Contains(x.Id));                
            }

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
            where T : class, ICustomerEntity, IStartPageEntity, IDatedEntity
        {
            query = query.GetByCustomers(customers);

            if (forStartPage)
            {
                query = query.Where(x => x.ShowOnStartPage == 1);
            }

            query = query.SortByCreated();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }

        public static IQueryable<T> GetForStartPageWithOptionalCustomer<T>(
                                    this IQueryable<T> query,
                                    int[] customers,
                                    int? count,
                                    bool forStartPage)
            where T : class, IOptionalCustomerEntity, IStartPageEntity, IDatedEntity
        {
            query = query.Where(x => x.Customer_Id.HasValue && customers.Contains(x.Customer_Id.Value));

            if (forStartPage)
            {
                query = query.Where(x => x.ShowOnStartPage == 1);
            }

            query = query.SortByCreated();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }

        public static IQueryable<T> GetForStartPage<T>(
                                    this IQueryable<T> query,
                                    int[] customers,
                                    int? count)
            where T : class, ICustomerEntity, IDatedEntity
        {
            query = query.GetByCustomers(customers)
                        .SortByCreated();

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

        public static IQueryable<T> RestrictByWorkingGroupsOnyRead<T>(this IQueryable<T> query, IWorkContext workContext)
            where T : class, IWorkingGroupEntity
        {
            //var userGroups = workContext.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id);
            var userGroups = workContext.User.UserWorkingGroups.Where(u => u.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).Select(u => u.WorkingGroup_Id);
            query = query.Where(x => !x.WGs.Any() || x.WGs.Any(g => userGroups.Contains(g.Id)));

            return query;
        }
        
        public static IQueryable<T> RestrictByWorkingGroup<T>(this IQueryable<T> query, IWorkContext workContext)
            where T : class, ISingleWorkingGroupEntity
        {
            var userGroups = workContext.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id);

            query = query.Where(x => x.WorkingGroup == null || userGroups.Contains(x.WorkingGroup.Id));

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

        /// <summary>
        /// ///////////////////////////
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="workContext"></param>
        /// <returns></returns>
        public static IQueryable<T> RestrictLinkByWorkingGroupsAndUsers<T>(this IQueryable<T> query, IWorkContext workContext)
           where T : class, IWorkingGroupEntity, IUserEntity
        {
            query = query.RestrictByWorkingGroups(workContext)
                        .RestrictByUsers(workContext);

            return query;
        }

        public static IQueryable<T> RestrictLinkByWorkingGroups<T>(this IQueryable<T> query, IWorkContext workContext)
          where T : class, IWorkingGroupEntity
        {
            var userGroups = workContext.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id);

            query = query.Where(x => !x.WGs.Any() || x.WGs.Any(g => userGroups.Contains(g.Id)));

            return query;
        }

        public static IQueryable<T> RestrictLinkByUsers<T>(this IQueryable<T> query, IWorkContext workContext)
           where T : class, IUserEntity
        {
            query = query.Where(x => !x.Us.Any() || x.Us.Any(u => u.Id == workContext.User.UserId));

            return query;
        }


        public static IQueryable<T> GetLinkForStartPage<T>(
                                   this IQueryable<T> query,
                                   int[] customers,
                                   int? count,
                                   bool forStartPage)
           where T : class, ICustomerEntity, IStartPageEntity, IDatedEntity
        {
            query = query.GetByCustomers(customers);

            if (forStartPage)
            {
                query = query.Where(x => x.ShowOnStartPage == 1);
            }

            query = query.SortByCreated();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }
        /// <summary>
        /// ////////////////////////
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>

        public static IQueryable<T> SortByCreated<T>(this IQueryable<T> query)
            where T : class, IDatedEntity
        {
            query = query.OrderByDescending(x => x.CreatedDate);

            return query;
        }  

        public static IQueryable<T> GetActive<T>(this IQueryable<T> query)
            where T : class, IActiveEntity
        {
            query = query.Where(x => x.IsActive != 0);

            return query;
        }

        public static IQueryable<T> GetActiveByCustomer<T>(this IQueryable<T> query, int customerId)
                    where T : class, ICustomerEntity, IActiveEntity
        {
            query = query.Where(x => x.Customer_Id == customerId).GetActive();

            return query;
        }

        public static IQueryable<T> GetShowable<T>(this IQueryable<T> query)
                    where T : class, IStartPageEntity
        {
            query = query.Where(x => x.ShowOnStartPage != 0);

            return query;
        } 
    }
}
