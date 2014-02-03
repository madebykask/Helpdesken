namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class WorkingGroupUsers
    {
        public WorkingGroupUsers(int workingGroupId, List<int> userIds)
        {
            this.WorkingGroupId = workingGroupId;
            this.UserIds = userIds;
        }

        [IsId]
        public int WorkingGroupId { get; private set; }

        [NotNull]
        public List<int> UserIds { get; private set; }
    }
}
