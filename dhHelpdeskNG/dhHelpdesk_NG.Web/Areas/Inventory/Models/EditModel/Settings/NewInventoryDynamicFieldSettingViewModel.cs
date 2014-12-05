namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings
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
            SelectList inventoryTypeGroups,
            SelectList fieldTypes)
        {
            this.InventoryDynamicFieldSettingModel = inventoryDynamicFieldSettingModel;
            this.InventoryTypeGroups = inventoryTypeGroups;
            this.FieldTypes = fieldTypes;
        }

        [NotNull]
        public NewInventoryDynamicFieldSettingModel InventoryDynamicFieldSettingModel { get; set; }

        [NotNull]
        public SelectList InventoryTypeGroups { get; set; }

        [NotNull]
        public SelectList FieldTypes { get; set; }
    }
}