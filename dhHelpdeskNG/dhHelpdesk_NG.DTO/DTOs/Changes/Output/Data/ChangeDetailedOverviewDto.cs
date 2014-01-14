namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeDetailedOverviewDto
    {
        public ChangeDetailedOverviewDto(
            OrdererFieldGroupDto ordererFields,
            GeneralFieldGroupDto generalFields,
            RegistrationFieldGroupDto registrationFields,
            AnalyzeFieldGroupDto analyzeFields,
            ImplementationFieldGroupDto implementationFields,
            EvaluationFieldGroupDto evaluationFields)
        {
            OrdererFields = ordererFields;
            GeneralFields = generalFields;
            RegistrationFields = registrationFields;
            AnalyzeFields = analyzeFields;
            ImplementationFields = implementationFields;
            EvaluationFields = evaluationFields;
        }

        [NotNull]
        public OrdererFieldGroupDto OrdererFields { get; private set; }

        [NotNull]
        public GeneralFieldGroupDto GeneralFields { get; private set; }

        [NotNull]
        public RegistrationFieldGroupDto RegistrationFields { get; private set; }

        [NotNull]
        public AnalyzeFieldGroupDto AnalyzeFields { get; private set; }

        [NotNull]
        public ImplementationFieldGroupDto ImplementationFields { get; private set; }

        [NotNull]
        public EvaluationFieldGroupDto EvaluationFields { get; private set; }
    }
}
