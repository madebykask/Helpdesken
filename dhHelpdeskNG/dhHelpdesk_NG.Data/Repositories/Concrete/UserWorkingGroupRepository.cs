namespace DH.Helpdesk.Dal.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class UserWorkingGroupRepository : RepositoryBase<UserWorkingGroup>, IUserWorkingGroupRepository
    {
        public UserWorkingGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<WorkingGroupUsers> FindWorkingGroupsUserIds(List<int> workingGroupIds, bool includeAdmins = true, bool activeUsers = false)
        {
            var items = (from wg in DataContext.WorkingGroups
                         from wgUser in wg.UserWorkingGroups.DefaultIfEmpty()
                         where workingGroupIds.Contains(wg.Id)
                         select new
                         {
                             Id = wg.Id,
                             UserId = (int?)wgUser.User_Id,
                             UserRole = (int?)wgUser.UserRole,
                             IsActiveUser = (int?)wgUser.User.IsActive,
                         }).ToList();

            
            if (!includeAdmins)
            {
                items = items.Where(x => x.UserRole == 2).ToList();
            }

            if (activeUsers)
            {
                items = items.Where(x => x.IsActiveUser == 1).ToList();
            }

            var result =
                 items.GroupBy(o => o.Id)
                      .Select(o => new WorkingGroupUsers(o.Key, o.Where(u => u.UserId.HasValue).Select(u => u.UserId.Value).ToList()))
                      .ToList();
         
            return result;
        }
    }
}