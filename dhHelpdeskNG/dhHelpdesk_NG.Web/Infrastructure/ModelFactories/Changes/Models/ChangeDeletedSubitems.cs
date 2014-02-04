namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Models
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeDeletedSubitems
    {
        public ChangeDeletedSubitems(
            List<string> registrationFiles,
            List<string> analyzeFiles,
            List<string> implementationFiles,
            List<string> evaluationFiles,
            List<int> logIds)
        {
            this.RegistrationFiles = registrationFiles;
            this.AnalyzeFiles = analyzeFiles;
            this.ImplementationFiles = implementationFiles;
            this.EvaluationFiles = evaluationFiles;
            this.LogIds = logIds;
        }

        [NotNull]
        public List<string> RegistrationFiles { get; private set; }

        [NotNull]
        public List<string> AnalyzeFiles { get; private set; }

        [NotNull]
        public List<string> ImplementationFiles { get; private set; }

        [NotNull]
        public List<string> EvaluationFiles { get; private set; }

        [NotNull]
        public List<int> LogIds { get; private set; }
    }
}
