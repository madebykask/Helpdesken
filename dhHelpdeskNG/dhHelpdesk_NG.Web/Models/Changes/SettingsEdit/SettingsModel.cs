namespace DH.Helpdesk.Web.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SettingsModel
    {
        public SettingsModel()
        {
        }

        public SettingsModel(
            OrdererSettingsModel orderer,
            GeneralSettingsModel general,
            RegistrationSettingsModel registration,
            AnalyzeSettingsModel analyze,
            ImplementationSettingsModel implementation,
            EvaluationSettingsModel evaluation,
            LogSettingsModel log)
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
        public OrdererSettingsModel Orderer { get; set; }

        [NotNull]
        public GeneralSettingsModel General { get; set; }

        [NotNull]
        public RegistrationSettingsModel Registration { get; set; }

        [NotNull]
        public AnalyzeSettingsModel Analyze { get; set; }

        [NotNull]
        public ImplementationSettingsModel Implementation { get; set; }

        [NotNull]
        public EvaluationSettingsModel Evaluation { get; set; }

        [NotNull]
        public LogSettingsModel Log { get; set; }
    }
}