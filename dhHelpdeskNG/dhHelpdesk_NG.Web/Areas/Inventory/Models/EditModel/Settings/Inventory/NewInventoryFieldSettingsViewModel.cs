namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewInventoryFieldSettingsViewModel
    {
        public NewInventoryFieldSettingsViewModel(
            int inventoryTypeId,
            DefaultFieldSettingsModel defaultSettings,
            InventoryDynamicFieldSettingModel newFieldSettingModel)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DefaultSettings = defaultSettings;
            this.NewFieldSettingModel = newFieldSettingModel;
        }

        [NotNull]
        public int InventoryTypeId { get; private set; }

        [NotNull]
        public DefaultFieldSettingsModel DefaultSettings { get; private set; }

        [NotNull]
        public InventoryDynamicFieldSettingModel NewFieldSettingModel { get; private set; }
    }
}