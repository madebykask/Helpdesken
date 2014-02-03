namespace dhHelpdesk_NG.Data.Repositories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

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

            return workingGroupsUsers.Select(u => new WorkingGroupUsers(u.Id, (List<int>)u.UserIds)).ToList();
        }
    }
}