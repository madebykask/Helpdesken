namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldSettings
    {
        public FieldSettings(
            OrdererFieldSettings orderer,
            GeneralFieldSettings general,
            RegistrationFieldSettings registration,
            AnalyzeFieldSettings analyze,
            ImplementationFieldSettings implementation,
            EvaluationFieldSettings evaluation,
            LogFieldSettings log)
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
        public OrdererFieldSettings Orderer { get; private set; }

        [NotNull]
        public GeneralFieldSettings General { get; private set; }

        [NotNull]
        public RegistrationFieldSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldSettings Analyze { get; private set; }

        [NotNull]
        public ImplementationFieldSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldSettings Evaluation { get; private set; }

        [NotNull]
        public LogFieldSettings Log { get; private set; }
    }
}
