namespace dhHelpdesk_NG.DTO.DTOs.Changes.ChangeAggregate
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeAggregate
    {
        public ChangeAggregate(
            ChangeHeader header,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation,
            EvaluationFields evaluation)
        {
            this.Implementation = implementation;
            this.Analyze = analyze;
            this.Registration = registration;
            this.Header = header;
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
