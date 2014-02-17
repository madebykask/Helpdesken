namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedSettings
    {
        public UpdatedSettings(
            int customerId,
            int languageId,
            UpdatedOrdererSettings orderer,
            UpdatedGeneralSettings general,
            UpdatedRegistrationSettings registration,
            UpdatedAnalyzeSettings analyze,
            UpdatedImplementationSettings implementation,
            UpdatedEvaluationSettings evaluation,
            UpdatedLogSettings log)
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
        public UpdatedOrdererSettings Orderer { get; private set; }

        [NotNull]
        public UpdatedGeneralSettings General { get; private set; }

        [NotNull]
        public UpdatedRegistrationSettings Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeSettings Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationSettings Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationSettings Evaluation { get; private set; }

        [NotNull]
        public UpdatedLogSettings Log { get; private set; }
    }
}
