namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IUserWorkingGroupRepository : IRepository<UserWorkingGroup>
    {
        List<WorkingGroupUsers> FindWorkingGroupsUserIds(List<int> workingGroupIds);
    }
}