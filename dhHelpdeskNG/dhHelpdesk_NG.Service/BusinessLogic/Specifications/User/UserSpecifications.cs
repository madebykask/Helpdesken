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
                query =
                    query.Where(
                        u =>
                        u.Default_WorkingGroup_Id != null
                        && workingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value));
            }

            return query;
        }
    }
}
