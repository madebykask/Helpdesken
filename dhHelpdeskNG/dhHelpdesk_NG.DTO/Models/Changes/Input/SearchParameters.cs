namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchParameters
    {
        public SearchParameters(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> affectedProcessIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            ChangeStatus? status,
            int selectCount)
        {
            this.CustomerId = customerId;
            this.StatusIds = statusIds;
            this.ObjectIds = objectIds;
            this.OwnerIds = ownerIds;
            this.AffectedProcessIds = affectedProcessIds;
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
        public List<int> AffectedProcessIds { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; private set; }

        [NotNull]
        public List<int> AdministratorIds { get; private set; }

        public string Pharse { get; private set; }

        public ChangeStatus? Status { get; private set; }

        [MinValue(0)]
        public int SelectCount { get; private set; }
    }
}
