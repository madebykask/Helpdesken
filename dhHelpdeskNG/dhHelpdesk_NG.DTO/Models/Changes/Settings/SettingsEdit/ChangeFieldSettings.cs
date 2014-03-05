namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeFieldSettings
    {
        #region Public Properties

        [NotNull]
        public AnalyzeFieldSettings Analyze { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

        [NotNull]
        public EvaluationFieldSettings Evaluation { get; private set; }

        [NotNull]
        public GeneralFieldSettings General { get; private set; }

        [NotNull]
        public ImplementationFieldSettings Implementation { get; private set; }

        [NotNull]
        public LogFieldSettings Log { get; private set; }

        [NotNull]
        public OrdererFieldSettings Orderer { get; private set; }

        [NotNull]
        public RegistrationFieldSettings Registration { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static ChangeFieldSettings CreateForEdit(
            OrdererFieldSettings orderer,
            GeneralFieldSettings general,
            RegistrationFieldSettings registration,
            AnalyzeFieldSettings analyze,
            ImplementationFieldSettings implementation,
            EvaluationFieldSettings evaluation,
            LogFieldSettings log)
        {
            return new ChangeFieldSettings
                       {
                           Orderer = orderer,
                           General = general,
                           Registration = registration,
                           Analyze = analyze,
                           Implementation = implementation,
                           Evaluation = evaluation,
                           Log = log
                       };
        }

        public static ChangeFieldSettings CreateUpdated(
            int customerId,
            int languageId,
            OrdererFieldSettings orderer,
            GeneralFieldSettings general,
            RegistrationFieldSettings registration,
            AnalyzeFieldSettings analyze,
            ImplementationFieldSettings implementation,
            EvaluationFieldSettings evaluation,
            LogFieldSettings log)
        {
            return new ChangeFieldSettings
                       {
                           CustomerId = customerId,
                           LanguageId = languageId,
                           Orderer = orderer,
                           General = general,
                           Registration = registration,
                           Analyze = analyze,
                           Implementation = implementation,
                           Evaluation = evaluation,
                           Log = log
                       };
        }

        #endregion
    }
}