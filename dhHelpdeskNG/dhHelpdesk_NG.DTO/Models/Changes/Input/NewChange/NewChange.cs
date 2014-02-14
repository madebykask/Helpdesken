namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewChange : INewBusinessModel
    {
        public NewChange(
            int customerId,
            NewOrdererFields orderer,
            NewGeneralFields general,
            NewRegistrationFields registration,
            NewAnalyzeFields analyze,
            NewImplementationFields implementation,
            NewEvaluationFields evaluation)
        {
            this.CustomerId = customerId;
            this.Orderer = orderer;
            this.General = general;
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
        public NewOrdererFields Orderer { get; private set; }

        [NotNull]
        public NewGeneralFields General { get; private set; }

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
