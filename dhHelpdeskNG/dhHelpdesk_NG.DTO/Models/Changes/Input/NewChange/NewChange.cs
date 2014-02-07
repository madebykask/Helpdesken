namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewChange : INewBusinessModel
    {
        public NewChange(
            int customerId,
            NewChangeHeader header,
            NewRegistrationFields registration,
            NewAnalyzeFields analyze,
            NewImplementationFields implementation,
            NewEvaluationFields evaluation)
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
        public int CustomerId { get; set; }

        [NotNull]
        public NewChangeHeader Header { get; private set; }

        [NotNull]
        public NewRegistrationFields Registration { get; private set; }

        [NotNull]
        public NewAnalyzeFields Analyze { get; private set; }

        [NotNull]
        public NewImplementationFields Implementation { get; private set; }

        [NotNull]
        public NewEvaluationFields Evaluation { get; private set; }
    }
}
