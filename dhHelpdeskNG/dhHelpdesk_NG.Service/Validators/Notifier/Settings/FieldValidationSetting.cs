namespace dhHelpdesk_NG.Service.Validators.Notifier.Settings
{
    public sealed class FieldValidationSetting
    {
        public FieldValidationSetting(bool readOnly, bool required, int? minLength)
        {
            this.ReadOnly = readOnly;
            this.Required = required;
            this.MinLength = minLength;
        }

        public bool Required { get; private set; }

        public int? MinLength { get; private set; }

        public bool ReadOnly { get; private set; }
    }
}
