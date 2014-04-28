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

        public List<WorkingGroupUsers> FindWorkingGroupsUserIds(List<int> workingGroupIds)
        {
            var workingGroupsUsers =
                this.DataContext.UserWorkingGroups.Where(g => workingGroupIds.Contains(g.WorkingGroup_Id))
                    .GroupBy(g => g.WorkingGroup_Id)
                    .Select(g => new { Id = g.Key, UserIds = g.Select(group => group.User_Id) })
                    .ToList();

            var result = new List<WorkingGroupUsers>(workingGroupIds.Count);

            foreach (var workingGroupId in workingGroupIds)
            {
                var group = workingGroupsUsers.SingleOrDefault(g => g.Id == workingGroupId);
                if (group == null)
                {
                    result.Add(new WorkingGroupUsers(workingGroupId, new List<int>()));
                }
                else
                {
                    result.Add(new WorkingGroupUsers(workingGroupId, group.UserIds.ToList()));
                }
            }

            return result;
        }
    }
}