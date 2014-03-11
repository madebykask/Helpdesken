namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    public class InventorySettings
    {
        private InventorySettings(DefaultSettings defaultSettings)
        {
            this.DefaultSettings = defaultSettings;
        }

        public int InventoryTypeId { get; private set; }

        public DefaultSettings DefaultSettings { get; private set; }
    }
}