namespace DH.Helpdesk.Services.Validators.Common
{
    public sealed class ElementaryValidationRule
    {
        public ElementaryValidationRule(bool readOnly, bool required)
        {
            this.ReadOnly = readOnly;
            this.Required = required;
        }

        public bool Required { get; private set; }

        public bool ReadOnly { get; private set; }
    }
}
