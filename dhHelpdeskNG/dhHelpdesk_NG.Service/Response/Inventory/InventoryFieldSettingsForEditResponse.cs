namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;

    public class InventoryFieldSettingsForEditResponse
    {
        public InventoryFieldSettingsForEditResponse(
            InventoryFieldSettings inventoryFieldSettings,
            List<InventoryDynamicFieldSetting> inventoryDynamicFieldSettings)
        {
            this.InventoryFieldSettings = inventoryFieldSettings;
            this.InventoryDynamicFieldSettings = inventoryDynamicFieldSettings;
        }

        public InventoryFieldSettings InventoryFieldSettings { get; set; }

        public List<InventoryDynamicFieldSetting> InventoryDynamicFieldSettings { get; set; }
    }
}