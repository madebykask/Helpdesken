namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsViewModel
    {
        public InventoryFieldSettingsViewModel()
        {
        }

        public InventoryFieldSettingsViewModel(
            NewInventoryDynamicFieldSettingViewModel newDynamicFieldViewModel,
            DefaultFieldSettingsModel defaultSettings,
            List<InventoryDynamicFieldSettingViewModel> inventoryDynamicFieldViewModelSettings)
        {
            this.NewDynamicFieldViewModel = newDynamicFieldViewModel;
            this.DefaultSettings = defaultSettings;
            this.InventoryDynamicFieldViewModelSettings = inventoryDynamicFieldViewModelSettings;
        }

        [NotNull]
        public NewInventoryDynamicFieldSettingViewModel NewDynamicFieldViewModel { get; set; }

        [NotNull]
        public DefaultFieldSettingsModel DefaultSettings { get; set; }

        [NotNull]
        public List<InventoryDynamicFieldSettingViewModel> InventoryDynamicFieldViewModelSettings { get; set; }
    }
}