namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedSettings
    {
        public UpdatedSettings(
            int customerId,
            int languageId,
            UpdatedOrdererFieldSettings orderer,
            UpdatedGeneralFieldSettings general,
            UpdatedRegistrationFieldSettings registration,
            UpdatedAnalyzeFieldSettings analyze,
            UpdatedImplementationFieldSettings implementation,
            UpdatedEvaluationFieldSettings evaluation,
            UpdatedLogFieldSettings log)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
            this.Log = log;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [NotNull]
        public UpdatedOrdererFieldSettings Orderer { get; private set; }

        [NotNull]
        public UpdatedGeneralFieldSettings General { get; private set; }

        [NotNull]
        public UpdatedRegistrationFieldSettings Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFieldSettings Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationFieldSettings Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationFieldSettings Evaluation { get; private set; }

        [NotNull]
        public UpdatedLogFieldSettings Log { get; private set; }
    }
}
