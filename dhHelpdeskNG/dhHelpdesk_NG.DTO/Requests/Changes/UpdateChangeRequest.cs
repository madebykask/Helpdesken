namespace DH.Helpdesk.BusinessData.Requests.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateChangeRequest
    {
        public UpdateChangeRequest(
            int customerId,
            UpdatedChange change,
            List<int> affectedProcessIds,
            List<int> affectedDepartmentIds,
            List<int> relatedChangeIds,
            List<DeletedFile> deletedFiles,
            List<NewFile> newFiles,
            List<int> deletedLogIds)
        {
            this.CustomerId = customerId;
            this.Change = change;
            this.AffectedProcessIds = affectedProcessIds;
            this.AffectedDepartmentIds = affectedDepartmentIds;
            this.RelatedChangeIds = relatedChangeIds;
            this.DeletedFiles = deletedFiles;
            this.NewFiles = newFiles;
            this.DeletedLogIds = deletedLogIds;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNull]
        public UpdatedChange Change { get; private set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; private set; }

        [NotNull]
        public List<int> AffectedDepartmentIds { get; private set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; private set; }

        [NotNull]
        public List<DeletedFile> DeletedFiles { get; private set; }

        [NotNull]
        public List<NewFile> NewFiles { get; private set; }

        [NotNull]
        public List<int> DeletedLogIds { get; private set; }
    }
}
