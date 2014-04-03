namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewChange : INewBusinessModel
    {
        #region Constructors and Destructors

        public NewChange(
            int customerId,
            int registrationLanguageId,
            NewOrdererFields orderer,
            NewGeneralFields general,
            NewRegistrationFields registration)
        {
            this.CustomerId = customerId;
            this.RegistrationLanguageId = registrationLanguageId;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int CustomerId { get; set; }

        [NotNull]
        public NewGeneralFields General { get; private set; }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public NewOrdererFields Orderer { get; private set; }

        [NotNull]
        public NewRegistrationFields Registration { get; private set; }

        [IsId]
        public int RegistrationLanguageId { get; private set; }

        #endregion

        #region Properties

        [NotNull]
        internal NewAnalyzeFields Analyze { get; set; }

        [NotNull]
        internal NewEvaluationFields Evaluation { get; set; }

        [NotNull]
        internal NewImplementationFields Implementation { get; set; }

        #endregion

        #region Methods

        internal void InitializeAnalyzePartWithDefaultValues()
        {
            this.Analyze = new NewAnalyzeFields(
                null,
                null,
                null,
                null,
                0,
                0,
                null,
                0,
                null,
                null,
                null,
                false,
                false,
                StepStatus.None,
                null,
                null,
                null);
        }

        internal void InitializeEvaluationPathWithDefaultValues()
        {
            this.Evaluation = new NewEvaluationFields(null, false);
        }

        internal void InitializeImplementationPartWithDefautValues()
        {
            this.Implementation = new NewImplementationFields(null, null, null, false, false, null, false, false);
        }

        #endregion
    }
}