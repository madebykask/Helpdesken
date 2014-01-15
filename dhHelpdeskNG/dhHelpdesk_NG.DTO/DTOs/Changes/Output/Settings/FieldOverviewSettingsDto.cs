namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldOverviewSettingsDto
    {
        public FieldOverviewSettingsDto(
            OrdererFieldOverviewSettingGroupDto orderer,
            GeneralFieldOverviewSettingGroupDto general,
            RegistrationFieldOverviewSettingGroupDto registration,
            AnalyzeFieldOverviewSettingGroupDto analyze,
            ImplementationFieldOverviewSettingGroupDto implementation,
            EvaluationFieldOverviewSettingGroupDto evaluation)
        {
            Orderer = orderer;
            General = general;
            Registration = registration;
            Analyze = analyze;
            Implementation = implementation;
            Evaluation = evaluation;
        }

        [NotNull]
        public OrdererFieldOverviewSettingGroupDto Orderer { get; private set; }

        [NotNull]
        public GeneralFieldOverviewSettingGroupDto General { get; private set; }

        [NotNull]
        public RegistrationFieldOverviewSettingGroupDto Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldOverviewSettingGroupDto Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldOverviewSettingGroupDto Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldOverviewSettingGroupDto Evaluation { get; private set; }
    }
}
