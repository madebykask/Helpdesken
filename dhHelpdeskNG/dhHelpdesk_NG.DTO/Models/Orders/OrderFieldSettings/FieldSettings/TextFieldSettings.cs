namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class TextFieldSettings : FieldSettings
    {
        private TextFieldSettings()
        {            
        }

        [NotNullAndEmpty]
        public string DefaultValue { get; private set; }

        public static TextFieldSettings CreateUpdated(
                        string orderField,
                        bool show,
                        bool showInList,
                        bool showExternal,
                        string label,
                        bool required,
                        string emailIdentifier,
                        string defaultValue)
        {
            return new TextFieldSettings
            {
                OrderField = orderField,
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                DefaultValue = defaultValue
            };
        }

        public static TextFieldSettings CreateForEdit(
                string orderField,
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string defaultValue)
        {
            return new TextFieldSettings
            {
                OrderField = orderField,
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                DefaultValue = defaultValue
            };
        }
    }
}