namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditSettings
    {
        public ChangeEditSettings(
            OrdererFieldEditSettings orderer,
            GeneralFieldEditSettings general,
            RegistrationFieldEditSettings registration,
            AnalyzeFieldEditSettings analyze,
            ImplementationFieldEditSettings implementation,
            EvaluationFieldEditSettings evaluation,
            LogFieldEditSettings log)
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
        public OrdererFieldEditSettings Orderer { get; private set; }

        [NotNull]
        public GeneralFieldEditSettings General { get; private set; }

        [NotNull]
        public RegistrationFieldEditSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeFieldEditSettings Analyze { get; private set; }
        
        [NotNull]
        public ImplementationFieldEditSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationFieldEditSettings Evaluation { get; private set; }

        [NotNull]
        public LogFieldEditSettings Log { get; private set; }
    }
}
