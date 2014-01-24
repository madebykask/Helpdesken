namespace dhHelpdesk_NG.DTO.DTOs.Changes.ChangeDetailedOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeDetailedOverview
    {
        public ChangeDetailedOverview(
            int id,
            OrdererFieldGroupDto ordererFields,
            GeneralFieldGroupDto generalFields,
            RegistrationFieldGroupDto registrationFields,
            AnalyzeFieldGroupDto analyzeFields,
            ImplementationFieldGroupDto implementationFields,
            EvaluationFieldGroupDto evaluationFields)
        {
            this.Id = id;
            this.Orderer = ordererFields;
            this.General = generalFields;
            this.Registration = registrationFields;
            this.Analyze = analyzeFields;
            this.Implementation = implementationFields;
            this.Evaluation = evaluationFields;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public OrdererFieldGroupDto Orderer { get; private set; }

        [NotNull]
        public GeneralFieldGroupDto General { get; private set; }

        [NotNull]
        public RegistrationFieldGroupDto Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldGroupDto Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldGroupDto Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldGroupDto Evaluation { get; private set; }
    }
}
