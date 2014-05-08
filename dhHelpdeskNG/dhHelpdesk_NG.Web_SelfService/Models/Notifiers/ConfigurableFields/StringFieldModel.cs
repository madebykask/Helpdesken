namespace DH.Helpdesk.SelfService.Models.Notifiers.ConfigurableFields
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes;

    public sealed class StringFieldModel : ConfigurableFieldModel<string>
    {
        public StringFieldModel()
        {
        }

        public StringFieldModel(bool show)
            : base(show)
        {
        }

        public StringFieldModel(bool show, string caption, string value)
            : base(show, caption)
        {
            this.Value = value;
        }

        public StringFieldModel(bool show, string caption, string value, bool required, int maxLength)
            : base(show, caption)
        {
            this.Value = value;
            this.Required = required;
            this.MaxLength = 100;
        }

        [MinValue(0)]
        public int MaxLength { get; set; }

        public bool Required { get; set; }

        [LocalizedRequiredFrom("Required")]
        [LocalizedMaxLengthFrom("MaxLength")]
        public override string Value { get; set; }
    }
}