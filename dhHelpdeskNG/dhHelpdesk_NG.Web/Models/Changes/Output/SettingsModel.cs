namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class SettingsModel
    {
         public SettingsModel(
            OrderedFieldSettingGroupModel ordered,
            GeneralFieldSettingGroupModel general,
            RegistrationFieldSettingGroupModel registration,
            AnalyzeFieldSettingGroupModel analyze,
            ImplementationFieldSettingGroupModel implementation,
            EvaluationFieldSettingGroupModel evaluation,
            LogFieldSettingGroupModel log)
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

        public OrderedFieldSettingGroupModel Ordered { get; private set; }

        public GeneralFieldSettingGroupModel General { get; private set; }

        public RegistrationFieldSettingGroupModel Registration { get; private set; }

        public AnalyzeFieldSettingGroupModel Analyze { get; private set; }

        public ImplementationFieldSettingGroupModel Implementation { get; private set; }

        public EvaluationFieldSettingGroupModel Evaluation { get; private set; }

        public LogFieldSettingGroupModel Log { get; private set; }
    }
}