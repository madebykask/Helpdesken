namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(ordered, "ordered");
            ArgumentsValidator.NotNull(general, "general");
            ArgumentsValidator.NotNull(registration, "registration");
            ArgumentsValidator.NotNull(analyze, "analyze");
            ArgumentsValidator.NotNull(implementation, "implementation");
            ArgumentsValidator.NotNull(evaluation, "evaluation");
            ArgumentsValidator.NotNull(log, "log");

            Ordered = ordered;
            General = general;
            Registration = registration;
            Analyze = analyze;
            Implementation = implementation;
            Evaluation = evaluation;
            Log = log;
        }

        public OrderedFieldSettingGroupDto Ordered { get; private set; }

        public GeneralFieldSettingGroupDto General { get; private set; }

        public RegistrationFieldSettingGroupDto Registration { get; private set; }

        public AnalyzeFieldSettingGroupDto Analyze { get; private set; }

        public ImplementationFieldSettingGroupDto Implementation { get; private set; }

        public EvaluationFieldSettingGroupDto Evaluation { get; private set; }

        public LogFieldSettingGroupDto Log { get; private set; }
    }
}
