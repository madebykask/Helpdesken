namespace DH.Helpdesk.Services.Requests.Changes
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateChangeRequest
    {
        public UpdateChangeRequest(
            int customerId,
            int languageId,
            UpdatedChange change,
            List<int> affectedProcessIds,
            List<int> affectedDepartmentIds,
            List<int> relatedChangeIds,
            List<DeletedFile> deletedFiles,
            List<NewFile> newFiles,
            List<int> deletedLogIds,
            NewLog analyzeLog,
            NewLog implementationLog,
            NewLog evaluationLog)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.Change = change;
            this.AffectedProcessIds = affectedProcessIds;
            this.AffectedDepartmentIds = affectedDepartmentIds;
            this.RelatedChangeIds = relatedChangeIds;
            this.DeletedFiles = deletedFiles;
            this.NewFiles = newFiles;
            this.DeletedLogIds = deletedLogIds;
            this.AnalyzeNewLog = analyzeLog;
            this.ImplementationNewLog = implementationLog;
            this.EvaluationNewLog = evaluationLog;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

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

        public NewLog AnalyzeNewLog { get; set; }

        public NewLog ImplementationNewLog { get; set; }

        public NewLog EvaluationNewLog { get; set; }
    }
}
