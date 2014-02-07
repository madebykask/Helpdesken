namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Change
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Change
    {
        public Change(
            int id,
            ChangeHeader header,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation,
            EvaluationFields evaluation)
        {
            this.Id = id;
            this.Header = header;
            this.Registration = registration;
            this.Analyze = analyze;
            this.Implementation = implementation;
            this.Evaluation = evaluation;
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
    }
}
