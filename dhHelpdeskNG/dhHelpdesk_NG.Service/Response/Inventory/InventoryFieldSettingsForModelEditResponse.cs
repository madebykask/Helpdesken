namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;

    public class InventoryFieldSettingsForModelEditResponse
    {
        public InventoryFieldSettingsForModelEditResponse(
            InventoryFieldSettingsForModelEdit inventoryFieldSettingsForModelEdit,
            List<InventoryDynamicFieldSettingForModelEdit> inventoryDynamicFieldSettingForModelEditData)
        {
            this.InventoryFieldSettingsForModelEdit = inventoryFieldSettingsForModelEdit;
            this.InventoryDynamicFieldSettingForModelEditData = inventoryDynamicFieldSettingForModelEditData;
        }

        public InventoryFieldSettingsForModelEdit InventoryFieldSettingsForModelEdit { get; set; }

        public List<InventoryDynamicFieldSettingForModelEdit> InventoryDynamicFieldSettingForModelEditData { get; set; }
    }
}