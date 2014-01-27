namespace dhHelpdesk_NG.Web.Models.Changes.InputModel
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class InputModel
    {
        public InputModel()
        {
        }

        public InputModel(
            ChangeHeaderModel header,
            RegistrationModel registration,
            AnalyzeModel analyze,
            ImplementationModel implementation,
            EvaluationModel evaluation,
            HistoryModel history)
        {
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.History = history;
        }

        [NotNull]
        public ChangeHeaderModel Header { get; set; }

        [NotNull]
        public RegistrationModel Registration { get; set; }

        [NotNull]
        public AnalyzeModel Analyze { get; set; }

        [NotNull]
        public ImplementationModel Implementation { get; set; }

        [NotNull]
        public EvaluationModel Evaluation { get; set; }

        public HistoryModel History { get; set; }
    }
}