namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldOverviewSettings
    {
        public FieldOverviewSettings(
            OrdererFieldOverviewSettings orderer,
            GeneralFieldOverviewSettings general,
            RegistrationFieldOverviewSettings registration,
            AnalyzeFieldOverviewSettings analyze,
            ImplementationFieldOverviewSettings implementation,
            EvaluationFieldOverviewSettings evaluation)
        {
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [NotNull]
        public OrdererFieldOverviewSettings Orderer { get; private set; }

        [NotNull]
        public GeneralFieldOverviewSettings General { get; private set; }

        [NotNull]
        public RegistrationFieldOverviewSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldOverviewSettings Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldOverviewSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldOverviewSettings Evaluation { get; private set; }
    }
}
