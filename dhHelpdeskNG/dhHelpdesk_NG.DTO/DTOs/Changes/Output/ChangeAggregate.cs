namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeAggregate
    {
        public ChangeAggregate(
            ChangeHeader header,
            RegistrationFields registration,
            AnalyzeFields analyze,
            ImplementationFields implementation)
        {
            this.Implementation = implementation;
            this.Analyze = analyze;
            this.Registration = registration;
            this.Header = header;
        }

        [NotNull]
        public ChangeHeader Header { get; private set; }

        [NotNull]
        public RegistrationFields Registration { get; private set; }

        [NotNull]
        public AnalyzeFields Analyze { get; private set; }

        [NotNull]
        public ImplementationFields Implementation { get; private set; }
    }
}
