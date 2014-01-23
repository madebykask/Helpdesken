namespace dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedChange
    {
        public UpdatedChange(
            int id,
            UpdatedChangeHeader header,
            UpdatedRegistrationFields registration,
            UpdatedAnalyzeFields analyze,
            UpdatedImplementationFields implementation,
            UpdatedEvaluationFields evaluation)
        {
            this.Id = id;
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
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
