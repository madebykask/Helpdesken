namespace DH.Helpdesk.SelfService.Models.Notifiers.ConfigurableFields
{
    using DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes;

    public sealed class IntegerFieldModel : ConfigurableFieldModel<int>
    {
        public IntegerFieldModel()
        {
        }

        public IntegerFieldModel(bool show)
            : base(show)
        {
        }

        public IntegerFieldModel(bool show, string caption, int value, bool required)
            : base(show, caption)
        {
            this.Value = value;
            this.Required = required;
        }

        [LocalizedRequiredFrom("Required")]
        public override int Value { get; set; }

        public bool Required { get; set; }
    }
}