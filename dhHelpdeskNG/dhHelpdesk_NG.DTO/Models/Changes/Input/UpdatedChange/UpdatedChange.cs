namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedChange
    {
        public UpdatedChange(
            int id,
            UpdatedOrdererFields orderer,
            UpdatedGeneralFields general,
            UpdatedRegistrationFields registration,
            UpdatedAnalyzeFields analyze,
            UpdatedImplementationFields implementation,
            UpdatedEvaluationFields evaluation)
        {
            this.Id = id;
            this.Orderer = orderer;
            this.General = general;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public UpdatedOrdererFields Orderer { get; private set; }

        [NotNull]
        public UpdatedGeneralFields General { get; private set; }

        [NotNull]
        public UpdatedRegistrationFields Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFields Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationFields Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationFields Evaluation { get; private set; }
    }
}
