namespace DH.Helpdesk.BusinessData.Models.Common.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldOverviewSetting
    {
        public FieldOverviewSetting(bool show, string caption)
        {
            this.Show = show;
            this.Caption = caption;
        }

        public bool Show { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}
