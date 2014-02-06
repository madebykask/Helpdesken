namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class SearchParameters
    {
        public SearchParameters(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            ChangeStatus status,
            int selectCount)
        {
            this.CustomerId = customerId;
            this.StatusIds = statusIds;
            this.ObjectIds = objectIds;
            this.OwnerIds = ownerIds;
            this.WorkingGroupIds = workingGroupIds;
            this.AdministratorIds = administratorIds;
            this.Pharse = pharse;
            this.Status = status;
            this.SelectCount = selectCount;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNull]
        public List<int> StatusIds { get; private set; }

        [NotNull]
        public List<int> ObjectIds { get; private set; }

        [NotNull]
        public List<int> OwnerIds { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; private set; }

        [NotNull]
        public List<int> AdministratorIds { get; private set; }

        public string Pharse { get; private set; }

        public ChangeStatus Status { get; private set; }

        [MinValue(0)]
        public int SelectCount { get; private set; }
    }
}
