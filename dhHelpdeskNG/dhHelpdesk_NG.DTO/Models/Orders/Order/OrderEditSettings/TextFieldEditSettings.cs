namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class TextFieldEditSettings : FieldEditSettings
    {
        public TextFieldEditSettings(bool show, string caption, bool required, string emailIdentifier, string defaultValue)
            : base(show, caption, required, emailIdentifier)
        {
            this.DefaultValue = defaultValue;
        }

        [NotNull]
        public string DefaultValue { get; private set; }
    }
}