namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeDetailedOverview
    {
        public ChangeDetailedOverview(
            int id,
            OrdererFields orderer,
            GeneralFields general,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation,
            EvaluationFields evaluation)
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
        public OrdererFields Orderer { get; private set; }

        [NotNull]
        public GeneralFields General { get; private set; }

        [NotNull]
        public RegistrationFields Registration { get; private set; }

        [NotNull]
        public AnalyzeFields Analyze { get; private set; }

        [NotNull]
        public ImplementationFields Implementation { get; private set; }

        [NotNull]
        public EvaluationFields Evaluation { get; private set; }
    }
}
