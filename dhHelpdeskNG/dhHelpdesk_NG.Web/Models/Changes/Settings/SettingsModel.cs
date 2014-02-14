namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SettingsModel
    {
        public SettingsModel()
        {
        }

         public SettingsModel(
            OrdererFieldSettingsModel orderer,
            GeneralFieldSettingsModel general,
            RegistrationFieldSettingsModel registration,
            AnalyzeFieldSettingsModel analyze,
            ImplementationFieldSettingsModel implementation,
            EvaluationFieldSettingsModel evaluation,
            LogFieldSettingsModel log)
        {
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.Log = log;
        }

        [NotNull]
        public OrdererFieldSettingsModel Orderer { get; set; }

        [NotNull]
        public GeneralFieldSettingsModel General { get; set; }

        [NotNull]
        public RegistrationFieldSettingsModel Registration { get; set; }

        [NotNull]
        public AnalyzeFieldSettingsModel Analyze { get; set; }

        [NotNull]
        public ImplementationFieldSettingsModel Implementation { get; set; }

        [NotNull]
        public EvaluationFieldSettingsModel Evaluation { get; set; }

        [NotNull]
        public LogFieldSettingsModel Log { get; set; }
    }
}