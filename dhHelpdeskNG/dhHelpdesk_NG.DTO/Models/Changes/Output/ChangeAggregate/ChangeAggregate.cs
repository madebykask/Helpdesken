namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeAggregate
    {
        public ChangeAggregate(
            int id,
            ChangeHeader header,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation,
            EvaluationFields evaluation,
            List<HistoriesDifference> histories)
        {
            this.Id = id;
            this.Implementation = implementation;
            this.Analyze = analyze;
            this.Registration = registration;
            this.Header = header;
            this.Evaluation = evaluation;
            this.Histories = histories;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public ChangeHeader Header { get; private set; }

        [NotNull]
        public RegistrationFields Registration { get; private set; }

        [NotNull]
        public AnalyzeFields Analyze { get; private set; }

        [NotNull]
        public ImplementationFields Implementation { get; private set; }

        [NotNull]
        public EvaluationFields Evaluation { get; private set; }

        [NotNull]
        public List<HistoriesDifference> Histories { get; private set; }
    }
}
