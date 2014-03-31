namespace DH.Helpdesk.Services.Requests.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateChangeRequest
    {
        public UpdateChangeRequest(
            OperationContext context,
            UpdatedChange change,
            List<int> affectedProcessIds,
            List<int> affectedDepartmentIds,
            List<int> relatedChangeIds,
            List<DeletedFile> deletedFiles,
            List<NewFile> newFiles,
            List<int> deletedLogIds,
            List<NewLog> newLogs)
        {
            this.Context = context;
            this.Change = change;
            this.AffectedProcessIds = affectedProcessIds;
            this.AffectedDepartmentIds = affectedDepartmentIds;
            this.RelatedChangeIds = relatedChangeIds;
            this.DeletedFiles = deletedFiles;
            this.NewFiles = newFiles;
            this.DeletedLogIds = deletedLogIds;
            this.NewLogs = newLogs;
        }

        [NotNull]
        public OperationContext Context { get; private set; }

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

        [NotNull]
        public List<NewLog> NewLogs { get; private set; }
    }
}
