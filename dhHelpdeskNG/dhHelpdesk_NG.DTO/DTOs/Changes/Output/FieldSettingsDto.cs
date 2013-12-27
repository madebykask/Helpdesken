namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
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
            Ordered = ordered;
            General = general;
            Registration = registration;
            Analyze = analyze;
            Implementation = implementation;
            Evaluation = evaluation;
            Log = log;
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
