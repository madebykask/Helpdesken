namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldEditSettings
    {
        public FieldEditSettings(
                bool show, 
                string caption, 
                bool required, 
                string emailIdentifier)
        {
            this.EmailIdentifier = emailIdentifier;
            this.Required = required;
            this.Caption = caption;
            this.Show = show;
        }

        public bool Show { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string EmailIdentifier { get; private set; } 
    }
}