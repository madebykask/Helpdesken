using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Common
{
    using System.Linq;

    public static class UserSpecifications
    {
        public static IQueryable<Domain.User> GetActiveUsers(this IQueryable<Domain.User> query, int customerId)
        {
            query = query.Where(u => u.IsActive != 0 && u.CustomerUsers.Any(c => c.Customer_Id == customerId));

            return query;
        }

        public static IQueryable<Domain.User> GetAdministrators(this IQueryable<Domain.User> query, int customerId)
        {
            //query = from u in Domain.User
            //            where u.CustomerUsers.Any(c => c.Customer_Id == customerId) // u.Customer_Id == customerId &&
            //            select u;

            //return query;

            query = query.GetByCustomer(customerId)
                        .GetActiveUsers(customerId)
                        .Where(u => u.Performer == 1 && u.UserGroup_Id > UserGroups.User);

            return query;
        } 
    }
}