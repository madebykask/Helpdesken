namespace dhHelpdesk_NG.DTO.DTOs.Changes.Change
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class Change
    {
        public Change(
            ChangeHeader header,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation,
            EvaluationFields evaluation)
        {
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [NotNull]
        public ChangeHeader Header { get; private set; }

        [NotNull]
        public RegistrationFields Registration { get; private set; }

        [NotNull]
        public AnalyzeFields Analyze { get; private set; }

        [NotNull]
        public ImplementationFields Implementation { get; private set; }

        [NotNull]
        public EvaluationFields Evaluation { get; private set; }
    }
}
