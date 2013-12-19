namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public class DisplayFieldSettingDto
    {
        public DisplayFieldSettingDto(bool show, string caption, bool required)
        {
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            this.Show = show;
            this.Caption = caption;
            this.Required = required;
        }

        public string Caption { get; private set; }

        public bool Show { get; private set; }

        public bool Required { get; private set; }
    }
}
