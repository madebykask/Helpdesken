namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditSettings
    {
        public ChangeEditSettings(
            OrdererEditSettings orderer,
            GeneralEditSettings general,
            RegistrationEditSettings registration,
            AnalyzeEditSettings analyze,
            ImplementationEditSettings implementation,
            EvaluationEditSettings evaluation,
            LogEditSettings log)
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
        public OrdererEditSettings Orderer { get; private set; }

        [NotNull]
        public GeneralEditSettings General { get; private set; }

        [NotNull]
        public RegistrationEditSettings Registration { get; private set; }

        [NotNull]
        public AnalyzeEditSettings Analyze { get; private set; }
        
        [NotNull]
        public ImplementationEditSettings Implementation { get; private set; }

        [NotNull]
        public EvaluationEditSettings Evaluation { get; private set; }

        [NotNull]
        public LogEditSettings Log { get; private set; }
    }
}
