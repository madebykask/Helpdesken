namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldOverviewSetting
    {
        public FieldOverviewSetting(bool show, string caption, bool required)
        {
            this.Show = show;
            this.Caption = caption;
            this.Required = required;
        }

        [NotNull]
        public string Caption { get; private set; }

        public bool Show { get; private set; }

        public bool Required { get; private set; }

        public static FieldOverviewSetting CreateEmpty()
        {
            return new FieldOverviewSetting(false, string.Empty, false);
        }
    }
}
