namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSettingOverview
    {
        public InventoryDynamicFieldSettingOverview(int id, string caption)
        {
            this.Id = id;
            this.Caption = caption;
        }

        [IsId]
        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}