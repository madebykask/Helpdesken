namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewChangeAggregate : IBusinessModelWithId
    {
        public NewChangeAggregate(
            NewChangeAggregateHeader header,
            NewRegistrationAggregateFields registration,
            NewAnalyzeAggregateFields analyze,
            NewImplementationAggregateFields implementation,
            NewEvaluationAggregateFields evaluation)
        {
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public NewChangeAggregateHeader Header { get; private set; }

        [NotNull]
        public NewRegistrationAggregateFields Registration { get; private set; }

        [NotNull]
        public NewAnalyzeAggregateFields Analyze { get; private set; }

        [NotNull]
        public NewImplementationAggregateFields Implementation { get; private set; }

        [NotNull]
        public NewEvaluationAggregateFields Evaluation { get; private set; }
    }
}
