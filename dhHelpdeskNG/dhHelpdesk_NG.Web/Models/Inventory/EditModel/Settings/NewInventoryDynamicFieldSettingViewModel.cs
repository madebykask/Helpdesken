namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewInventoryDynamicFieldSettingViewModel
    {
        public NewInventoryDynamicFieldSettingViewModel()
        {

        }

        public NewInventoryDynamicFieldSettingViewModel(
            NewInventoryDynamicFieldSettingModel inventoryDynamicFieldSettingModel,
            SelectList inventoryTypeGroups)
        {
            this.InventoryDynamicFieldSettingModel = inventoryDynamicFieldSettingModel;
            this.InventoryTypeGroups = inventoryTypeGroups;
        }

        [NotNull]
        public NewInventoryDynamicFieldSettingModel InventoryDynamicFieldSettingModel { get; set; }

        [NotNull]
        public SelectList InventoryTypeGroups { get; set; }
    }
}