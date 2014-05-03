namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsWithTypeInfoOverview
    {
        public InventoryFieldSettingsWithTypeInfoOverview(
            int inventoryTypeId,
            string inventoryTypeName,
            DefaultFieldSettings defaultSettings)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryTypeName = inventoryTypeName;
            this.DefaultSettings = defaultSettings;
        }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [NotNullAndEmpty]
        public string InventoryTypeName { get; private set; }

        [NotNull]
        public DefaultFieldSettings DefaultSettings { get; private set; }
    }
}