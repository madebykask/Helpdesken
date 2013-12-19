namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    public sealed class DisplayStringFieldSettingDto : DisplayFieldSettingDto
    {
        public DisplayStringFieldSettingDto(bool show, string caption, bool required, int? minLength)
            : base(show, caption, required)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}
