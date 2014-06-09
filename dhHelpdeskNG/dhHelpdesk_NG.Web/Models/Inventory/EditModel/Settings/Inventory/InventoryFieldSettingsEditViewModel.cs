namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsEditViewModel
    {
        public InventoryFieldSettingsEditViewModel()
        {
        }

        public InventoryFieldSettingsEditViewModel(
            InventoryTypeModel inventoryTypeModel,
            InventoryFieldSettingsViewModel inventoryFieldSettingsViewModel)
        {
            this.InventoryTypeModel = inventoryTypeModel;
            this.InventoryFieldSettingsViewModel = inventoryFieldSettingsViewModel;
        }

        [NotNull]
        public InventoryTypeModel InventoryTypeModel { get; set; }

        [NotNull]
        public InventoryFieldSettingsViewModel InventoryFieldSettingsViewModel { get; set; }
    }
}