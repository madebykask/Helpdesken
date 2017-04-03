namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class TextFieldSettings : FieldSettings
    {
        protected TextFieldSettings()
        {            
        }

        [NotNull]
        public string DefaultValue { get; protected set; }

        public static TextFieldSettings CreateUpdated(
                        bool show,
                        bool showInList,
                        bool showExternal,
                        string label,
                        bool required,
                        string emailIdentifier,
                        string defaultValue,
                        string help)
        {
            return new TextFieldSettings
            {
                Show = show,
                ShowInList = showInList,
                ShowExternal = showExternal,
                Label = label,
                Required = required,
                EmailIdentifier = emailIdentifier,
                DefaultValue = !string.IsNullOrEmpty(defaultValue) ? defaultValue : string.Empty,
                FieldHelp = !string.IsNullOrEmpty(help) ? help : string.Empty,
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
                string defaultValue,
                string help)
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
                DefaultValue = defaultValue,
                FieldHelp = help
            };
        }
    }
}