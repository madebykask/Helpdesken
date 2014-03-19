namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    public class InventoryFieldSettings
    {
        private InventoryFieldSettings(int inventoryTypeId, DefaultFieldSettings defaultSettings)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DefaultSettings = defaultSettings;
        }

        public int InventoryTypeId { get; private set; }

        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}