namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsOverview
    {
        public InventoryFieldSettingsOverview(int inventoryTypeId, DefaultFieldSettings defaultSettings)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DefaultSettings = defaultSettings;
        }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [NotNull]
        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}