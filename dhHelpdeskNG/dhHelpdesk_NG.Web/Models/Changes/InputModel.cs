namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class InputModel
    {
        public InputModel()
        {
            
        }

        public InputModel(RegistrationModel registration, AnalyzeModel analyze)
        {
            this.Registration = registration;
            this.Analyze = analyze;
        }

        [NotNull]
        public RegistrationModel Registration { get; set; }

        [NotNull]
        public AnalyzeModel Analyze { get; set; }
    }
}