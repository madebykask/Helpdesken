namespace dhHelpdesk_NG.Web.Models.Changes
{
    public sealed class StringFieldSettingModel : FieldSettingModel
    {
        public StringFieldSettingModel()
        {
        }

        public StringFieldSettingModel(
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

        public string DefaultValue { get; private set; }
    }
}