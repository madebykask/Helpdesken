namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Models
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;

    public sealed class ChangeNewSubitems
    {
        public ChangeNewSubitems(
            List<WebTemporaryFile> registrationFiles,
            List<WebTemporaryFile> analyzeFiles,
            List<WebTemporaryFile> implementationFiles,
            List<WebTemporaryFile> evaluationFiles)
        {
            this.RegistrationFiles = registrationFiles;
            this.AnalyzeFiles = analyzeFiles;
            this.ImplementationFiles = implementationFiles;
            this.EvaluationFiles = evaluationFiles;
        }

        [NotNull]
        public List<WebTemporaryFile> RegistrationFiles { get; private set; }

        [NotNull]
        public List<WebTemporaryFile> AnalyzeFiles { get; private set; }

        [NotNull]
        public List<WebTemporaryFile> ImplementationFiles { get; private set; }

        [NotNull]
        public List<WebTemporaryFile> EvaluationFiles { get; private set; }
    }
}
