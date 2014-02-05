namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeEditSettings
    {
        public ChangeEditSettings(
            OrdererFieldEditSettings ordererFields,
            GeneralFieldEditSettings generalFields,
            RegistrationFieldEditSettings registrationFields,
            AnalyzeFieldEditSettings analyzeFields,
            ImplementationFieldEditSettings implementationFields,
            EvaluationFieldEditSettings evaluationFields,
            LogFieldEditSettings logFields)
        {
            this.OrdererFields = ordererFields;
            this.GeneralFields = generalFields;
            this.RegistrationFields = registrationFields;
            this.AnalyzeFields = analyzeFields;
            this.ImplementationFields = implementationFields;
            this.EvaluationFields = evaluationFields;
            this.LogFields = logFields;
        }
        [NotNull]
        public OrdererFieldEditSettings OrdererFields { get; private set; }

        [NotNull]
        public GeneralFieldEditSettings GeneralFields { get; private set; }

        [NotNull]
        public RegistrationFieldEditSettings RegistrationFields { get; private set; }

        [NotNull]
        public AnalyzeFieldEditSettings AnalyzeFields { get; private set; }
        
        [NotNull]
        public ImplementationFieldEditSettings ImplementationFields { get; private set; }

        [NotNull]
        public EvaluationFieldEditSettings EvaluationFields { get; private set; }

        [NotNull]
        public LogFieldEditSettings LogFields { get; private set; }
    }
}
