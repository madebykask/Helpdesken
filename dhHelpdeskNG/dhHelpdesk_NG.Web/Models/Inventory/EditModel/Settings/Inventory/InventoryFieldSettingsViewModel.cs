namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsViewModel
    {
        private InventoryFieldSettingsViewModel(
            int inventoryTypeId,
            DefaultFieldSettingsModel defaultSettings,
            List<InventoryFieldSettingModel> inventoryDynamicFieldSettings)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DefaultSettings = defaultSettings;
            this.InventoryDynamicFieldSettings = inventoryDynamicFieldSettings;
        }

        [NotNull]
        public int InventoryTypeId { get; private set; }

        [NotNull]
        public DefaultFieldSettingsModel DefaultSettings { get; private set; }

        [NotNull]
        public List<InventoryFieldSettingModel> InventoryDynamicFieldSettings { get; private set; }
    }
}