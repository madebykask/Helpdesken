namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSettingViewModel
    {
        public InventoryDynamicFieldSettingViewModel()
        {
        }

        public InventoryDynamicFieldSettingViewModel(
            InventoryDynamicFieldSettingModel inventoryDynamicFieldSettingModel,
            SelectList inventoryTypeGroups,
            SelectList fieldTypes)
        {
            this.InventoryDynamicFieldSettingModel = inventoryDynamicFieldSettingModel;
            this.InventoryTypeGroups = inventoryTypeGroups;
            this.FieldTypes = fieldTypes;
        }

        [NotNull]
        public InventoryDynamicFieldSettingModel InventoryDynamicFieldSettingModel { get; set; }

        [NotNull]
        public SelectList InventoryTypeGroups { get; set; }

        [NotNull]
        public SelectList FieldTypes { get; set; }
    }
}