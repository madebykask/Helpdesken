namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeOverviewSettings
    {
        public ChangeOverviewSettings(
            OrdererOverviewSettings orderer,
            GeneralOverviewSettings general,
            RegistrationOverviewSettings registration,
            AnalyzeOverviewSettings analyze,
            ImplementationOverviewSettings implementation,
            EvaluationOverviewSettings evaluation)
        {
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [NotNull]
        public OrdererOverviewSettings Orderer { get; private set; }

        [NotNull]
        public GeneralOverviewSettings General { get; private set; }

        [NotNull]
        public RegistrationOverviewSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeOverviewSettings Analyze { get; private set; }

        [NotNull]
        public ImplementationOverviewSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationOverviewSettings Evaluation { get; private set; }
    }
}
