using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Services.BusinessLogic.Specifications.User
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class UserSpecifications
    {
        public static IQueryable<User> GetWorkingGroupsUsers(this IQueryable<User> query, int[] workingGroups)
        {
            if (workingGroups != null && workingGroups.Any())
            {
                // http://redmine.fastdev.se/issues/10997
                /*query =
                    query.Where(
                        u =>
                        u.Default_WorkingGroup_Id != null
                        && workingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value));*/

                query =
                    query.Where(
                        u =>
                        u.UserWorkingGroups.Any(w => workingGroups.Contains(w.WorkingGroup_Id)));
            }

            return query;
        }

        public static IQueryable<User> GetActive(this IQueryable<User> query)
        {
            query = query.Where(u => u.IsActive != 0);

            return query;
        }

        public static IQueryable<User> GetPerformers(this IQueryable<User> query)
        {
            query = query.Where(u => u.Performer == 1 && u.UserGroup_Id > UserGroups.User);
            return query;
        }

        public static IQueryable<User> GetAdministratorsWithEmails(this IQueryable<User> query, int customerId)
        {
            query = query
                .GetByCustomer(customerId)
                .GetActive()
                .GetPerformers()
                .Where(u => u.Email != string.Empty);

            return query;
        }

        public static IQueryable<User> GetOrderedByName(this IQueryable<User> query)
        {
            query = query
                    .OrderBy(u => u.SurName)
                    .ThenBy(u => u.FirstName);

            return query;
        } 
    }
}
