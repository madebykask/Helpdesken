namespace DH.Helpdesk.BusinessData.Models.Shared.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldOverviewSetting
    {
        public FieldOverviewSetting(bool show, string caption)
        {
            Show = show;
            Caption = caption;
        }

        public bool Show { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public static FieldOverviewSetting CreateUnshowable()
        {
            return new FieldOverviewSetting(false, string.Empty);
        }
    }
}
