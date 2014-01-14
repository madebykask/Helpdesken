namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldSettingsDto
    {
        public FieldSettingsDto(
            OrderedFieldSettingGroupDto ordered,
            GeneralFieldSettingGroupDto general,
            RegistrationFieldSettingGroupDto registration,
            AnalyzeFieldSettingGroupDto analyze,
            ImplementationFieldSettingGroupDto implementation,
            EvaluationFieldSettingGroupDto evaluation,
            LogFieldSettingGroupDto log)
        {
            this.Ordered = ordered;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.Log = log;
        }

        [NotNull]
        public OrderedFieldSettingGroupDto Ordered { get; private set; }

        [NotNull]
        public GeneralFieldSettingGroupDto General { get; private set; }

        [NotNull]
        public RegistrationFieldSettingGroupDto Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldSettingGroupDto Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldSettingGroupDto Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldSettingGroupDto Evaluation { get; private set; }

        [NotNull]
        public LogFieldSettingGroupDto Log { get; private set; }
    }
}
