namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class SettingsModel
    {
        public SettingsModel()
        {
        }

         public SettingsModel(
            OrderedFieldSettingGroupModel ordered,
            GeneralFieldSettingGroupModel general,
            RegistrationFieldSettingGroupModel registration,
            AnalyzeFieldSettingGroupModel analyze,
            ImplementationFieldSettingGroupModel implementation,
            EvaluationFieldSettingGroupModel evaluation,
            LogFieldSettingGroupModel log)
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
        public OrderedFieldSettingGroupModel Ordered { get; set; }

        [NotNull]
        public GeneralFieldSettingGroupModel General { get; set; }

        [NotNull]
        public RegistrationFieldSettingGroupModel Registration { get; set; }

        [NotNull]
        public AnalyzeFieldSettingGroupModel Analyze { get; set; }

        [NotNull]
        public ImplementationFieldSettingGroupModel Implementation { get; set; }

        [NotNull]
        public EvaluationFieldSettingGroupModel Evaluation { get; set; }

        [NotNull]
        public LogFieldSettingGroupModel Log { get; set; }
    }
}