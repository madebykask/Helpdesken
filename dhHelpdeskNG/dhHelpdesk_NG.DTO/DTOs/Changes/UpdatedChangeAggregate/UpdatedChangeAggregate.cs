namespace dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedChangeAggregate
    {
        public UpdatedChangeAggregate(
            int id,
            UpdatedChangeHeader header,
            UpdatedRegistrationFields registration,
            UpdatedAnalyzeFields analyze,
            UpdatedImplementationFields implementation,
            UpdatedEvaluationFields evaluation)
        {
            this.Id = id;
            this.Implementation = implementation;
            this.Analyze = analyze;
            this.Registration = registration;
            this.Header = header;
            this.Evaluation = evaluation;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public UpdatedChangeHeader Header { get; private set; }

        [NotNull]
        public UpdatedRegistrationFields Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFields Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationFields Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationFields Evaluation { get; private set; }
    }
}
