namespace dhHelpdesk_NG.Service.Validators.Notifier.Settings
{
    public sealed class FieldValidationSetting
    {
        public FieldValidationSetting(bool required, int? minLength, bool canFill)
        {
            CanFill = canFill;
            this.Required = required;
            this.MinLength = minLength;
        }

        public bool Required { get; private set; }

        public int? MinLength { get; private set; }

        public bool CanFill { get; private set; }
    }
}
