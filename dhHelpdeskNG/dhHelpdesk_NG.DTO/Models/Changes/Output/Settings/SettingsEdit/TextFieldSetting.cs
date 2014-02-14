namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    public sealed class TextFieldSetting : FieldSetting
    {
        public TextFieldSetting(
            string name,
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string defaultValue,
            string bookmark)
            : base(name, showInDetails, showInChanges, showInSelfService, caption, required, bookmark)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; private set; }
    }
}
