namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public sealed class TextFieldSettingsModel : FieldSettingsModel
    {
        public TextFieldSettingsModel()
        {            
        }

        public TextFieldSettingsModel(
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string defaultValue,
                string help)
                : base(show, showInList, showExternal, label, required, emailIdentifier, help)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
    }
}