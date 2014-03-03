namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeProcessingSettings
    {
        public ChangeProcessingSettings(
            OrdererProcessingSettings orderer,
            GeneralProcessingSettings general,
            RegistrationProcessingSettings registration,
            AnalyzeProcessingSettings analyze,
            ImplementationProcessingSettings implementation,
            EvaluationProcessingSettings evaluation)
        {
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [NotNull]
        public OrdererProcessingSettings Orderer { get; private set; }

        [NotNull]
        public GeneralProcessingSettings General { get; private set; }

        [NotNull]
        public RegistrationProcessingSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeProcessingSettings Analyze { get; private set; }

        [NotNull]
        public ImplementationProcessingSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationProcessingSettings Evaluation { get; private set; }
    }
}
