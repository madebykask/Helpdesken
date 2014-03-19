namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsForModelEdit
    {
        public InventoryFieldSettingsForModelEdit(int inventoryTypeId, DefaultFieldSettings defaultSettings)
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