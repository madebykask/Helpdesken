namespace DH.Helpdesk.Web.Infrastructure.Filters.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangesFilter
    {
        public ChangesFilter(
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> affectedProcessIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            ChangeStatus? status,
            int recordsOnPage)
        {
            this.StatusIds = statusIds;
            this.ObjectIds = objectIds;
            this.OwnerIds = ownerIds;
            this.AffectedProcessIds = affectedProcessIds;
            this.WorkingGroupIds = workingGroupIds;
            this.AdministratorIds = administratorIds;
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
        }

        private ChangesFilter()
        {
            this.StatusIds = new List<int>(0);
            this.ObjectIds = new List<int>(0);
            this.OwnerIds = new List<int>(0);
            this.AffectedProcessIds = new List<int>(0);
            this.WorkingGroupIds = new List<int>(0);
            this.AdministratorIds = new List<int>(0);
        }

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
        public int RecordsOnPage { get; private set; }

        public static ChangesFilter CreateDefault()
        {
            return new ChangesFilter { Status = ChangeStatus.Active, RecordsOnPage = 100 };
        }
    }
}