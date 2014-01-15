namespace dhHelpdesk_NG.DTO.DTOs
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldOverviewSettingDto
    {
        public FieldOverviewSettingDto(bool show, string caption)
        {
            Show = show;
            Caption = caption;
        }

        public bool Show { get; private set; }

        [NotNull]
        public string Caption { get; private set; }
    }
}
