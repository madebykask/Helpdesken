namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSettingOverview
    {
        public InventoryDynamicFieldSettingOverview(int id, bool show, string caption)
        {
            this.Id = id;
            this.IsShow = show;
            this.Caption = caption;
        }

        [IsId]
        public int Id { get; set; }

        public bool IsShow { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}