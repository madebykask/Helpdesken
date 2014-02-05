namespace dhHelpdesk_NG.DTO.DTOs
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldOverviewSetting
    {
        public FieldOverviewSetting(bool show, string caption)
        {
            Show = show;
            Caption = caption;
        }

        public bool Show { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}
