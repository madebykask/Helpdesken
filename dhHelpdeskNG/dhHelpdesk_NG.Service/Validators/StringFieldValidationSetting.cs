namespace dhHelpdesk_NG.Service.Validators
{
    public sealed class StringFieldValidationSetting : FieldValidationSetting
    {
        public StringFieldValidationSetting(bool readOnly, bool required, int? minLength)
            : base(readOnly, required)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}
