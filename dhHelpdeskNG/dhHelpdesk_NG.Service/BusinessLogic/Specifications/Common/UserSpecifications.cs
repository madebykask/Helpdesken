namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Common
{
    using System.Linq;

    public static class UserSpecifications
    {
        public static IQueryable<Domain.User> GetActiveUsers(this IQueryable<Domain.User> query)
        {
            query = query.Where(u => u.IsActive != 0);

            return query;
        }

        public static IQueryable<Domain.User> GetAdministrators(this IQueryable<Domain.User> query, int customerId)
        {
            query = query.GetByCustomer(customerId)
                        .GetActiveUsers();

            return query;
        } 
    }
}