namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    public sealed class StringFieldSettingDto : FieldSettingDto
    {
        public StringFieldSettingDto(
            string name,
            bool showInDetails,
            bool showInChanges,
            bool showInChangesService,
            string caption,
            bool required,
            string defaultValue,
            string bookmark)
            : base(name, showInDetails, showInChanges, showInChangesService, caption, required, bookmark)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; private set; }
    }
}
