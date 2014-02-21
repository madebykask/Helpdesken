namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSettingOverview
    {
        public FieldSettingOverview(bool show, string caption)
        {
            this.IsShow = show;
            this.Caption = caption;
        }

        public bool IsShow { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}