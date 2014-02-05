namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit
{
    public sealed class StringFieldSetting : FieldSetting
    {
        public StringFieldSetting(
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
