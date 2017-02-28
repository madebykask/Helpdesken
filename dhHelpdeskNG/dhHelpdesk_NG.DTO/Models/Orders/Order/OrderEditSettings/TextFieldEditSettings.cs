namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class TextFieldEditSettings : FieldEditSettings
    {
        public TextFieldEditSettings(bool show, string caption,
            bool required, string emailIdentifier, string defaultValue, string help)
            : base(show, caption, required, emailIdentifier, help)
        {
            DefaultValue = defaultValue;
        }

        [NotNull]
        public string DefaultValue { get; private set; }
    }
}