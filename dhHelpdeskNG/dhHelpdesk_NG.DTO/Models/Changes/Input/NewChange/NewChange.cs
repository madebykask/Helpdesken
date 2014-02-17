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
            NewRegistrationFields registration)
        {
            this.CustomerId = customerId;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
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
        internal NewAnalyzeFields Analyze { get; set; }

        [NotNull]
        internal NewImplementationFields Implementation { get; set; }

        [NotNull]
        internal NewEvaluationFields Evaluation { get; set; }
    }
}
