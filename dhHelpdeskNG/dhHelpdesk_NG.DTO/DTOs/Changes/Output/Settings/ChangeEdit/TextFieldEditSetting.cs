namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    public sealed class TextFieldEditSetting : FieldEditSetting
    {
        public TextFieldEditSetting(bool show, string caption, bool required, string defaultValue, string bookmark)
            : base(show, caption, required, bookmark)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; private set; }
    }
}
