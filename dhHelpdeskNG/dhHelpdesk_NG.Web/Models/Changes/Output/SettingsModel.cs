namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            Ordered = ordered;
            General = general;
            Registration = registration;
            Analyze = analyze;
            Implementation = implementation;
            Evaluation = evaluation;
            Log = log;
        }

        [NotNull]
        public OrderedFieldSettingGroupModel Ordered { get; private set; }

        [NotNull]
        public GeneralFieldSettingGroupModel General { get; private set; }

        [NotNull]
        public RegistrationFieldSettingGroupModel Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldSettingGroupModel Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldSettingGroupModel Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldSettingGroupModel Evaluation { get; private set; }

        [NotNull]
        public LogFieldSettingGroupModel Log { get; private set; }
    }
}