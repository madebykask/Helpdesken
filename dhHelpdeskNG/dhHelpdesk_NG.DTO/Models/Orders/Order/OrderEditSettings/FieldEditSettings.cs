namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using Common.ValidationAttributes;

    public class FieldEditSettings
    {
        public FieldEditSettings(
                bool show, 
                string caption, 
                bool required, 
                string emailIdentifier,
                string help)
        {
            EmailIdentifier = emailIdentifier;
            Required = required;
            Caption = caption;
            Show = show;
            Help = help;
        }

        public bool Show { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string EmailIdentifier { get; private set; } 

        public string Help { get; private set; }
    }
}