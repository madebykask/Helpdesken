namespace DH.Helpdesk.Mobile.Models.Changes.SettingsEdit
{
    public sealed class TextFieldSettingModel : FieldSettingModel
    {
        public TextFieldSettingModel()
        {
        }

        public TextFieldSettingModel(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string defaultValue,
            string bookmark)
            : base(showInDetails, showInChanges, showInSelfService, caption, required, bookmark)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
    }
}