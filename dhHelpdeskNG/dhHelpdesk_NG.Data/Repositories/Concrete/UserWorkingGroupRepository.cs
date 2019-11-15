using System;
using DH.Helpdesk.BusinessData.Enums.Users;
using System.Data.Entity;

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

        public List<WorkingGroupUsers> FindWorkingGroupsUserIds(List<int> workingGroupIds, bool includeAdmins = true, bool? isMemberOfGroup = null, bool activeUsers = false)
        {
            if(workingGroupIds == null) throw new ArgumentNullException(nameof(workingGroupIds));

            var items = (from wg in DataContext.WorkingGroups.AsNoTracking()
                from wgUser in wg.UserWorkingGroups.DefaultIfEmpty()
                where workingGroupIds.Contains(wg.Id) && wgUser.User_Id != 0
                select new
                {
                    Id = wg.Id,
                    UserId = wgUser == null ? (int?) null : wgUser.User_Id,
                    UserRole = wgUser == null ? (int?)null : wgUser.UserRole,
                    IsActiveUser = wgUser == null ? (int?)null : wgUser.User.IsActive,
                    IsMemberOfGroup = wgUser == null ? (bool?)null : wgUser.IsMemberOfGroup
                });

            if (!includeAdmins)
            {
                items = items.Where(x => x.UserRole != WorkingGroupUserPermission.ADMINSTRATOR);
            }

            if (isMemberOfGroup.HasValue)
            {
                items = items.Where(x => x.IsMemberOfGroup == isMemberOfGroup.Value);
            }

            if (activeUsers)
            {
                items = items.Where(x => x.IsActiveUser == 1);
            }

            var result =
                 items.GroupBy(o => o.Id)
                      .Select(o => new WorkingGroupUsers
                      {
                          WorkingGroupId = o.Key,
                          UserIds = o.Where(u => u.UserId.HasValue).Select(u => u.UserId.Value).ToList()
                      })
                      .ToList();
         
            return result;
        }
    }
}