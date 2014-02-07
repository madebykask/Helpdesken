namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChangeAggregate
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewChangeAggregate : INewBusinessModel
    {
        public NewChangeAggregate(
            int customerId,
            NewChangeAggregateHeader header,
            NewRegistrationAggregateFields registration,
            NewAnalyzeAggregateFields analyze,
            NewImplementationAggregateFields implementation,
            NewEvaluationAggregateFields evaluation)
        {
            this.CustomerId = customerId;
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int CustomerId { get; private set; }

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
