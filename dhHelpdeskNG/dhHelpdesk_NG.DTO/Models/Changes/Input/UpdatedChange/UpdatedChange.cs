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

        public UpdatedOrdererFields Orderer { get; internal set; }

        public UpdatedGeneralFields General { get; internal set; }

        public UpdatedRegistrationFields Registration { get; internal set; }

        public UpdatedAnalyzeFields Analyze { get; internal set; }

        public UpdatedImplementationFields Implementation { get; internal set; }

        public UpdatedEvaluationFields Evaluation { get; internal set; }
    }
}
