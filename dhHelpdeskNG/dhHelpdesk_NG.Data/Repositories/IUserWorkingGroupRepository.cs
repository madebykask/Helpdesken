namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IUserWorkingGroupRepository : IRepository<UserWorkingGroup>
    {
        List<WorkingGroupUsers> FindWorkingGroupsUserIds(List<int> workingGroupIds, bool includeAdmins = true, bool? isMemberOfGroup = null, bool activeUsers = false);
    }
}