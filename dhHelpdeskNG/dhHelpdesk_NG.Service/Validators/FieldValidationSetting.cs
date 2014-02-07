namespace DH.Helpdesk.Services.Validators
{
    public class FieldValidationSetting
    {
        public FieldValidationSetting(bool readOnly, bool required)
        {
            this.ReadOnly = readOnly;
            this.Required = required;
        }

        public bool Required { get; private set; }

        public bool ReadOnly { get; private set; }
    }
}
