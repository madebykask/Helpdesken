namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;

    public class InventoryFieldSettingsOverviewResponse
    {
        public InventoryFieldSettingsOverviewResponse(
            InventoryFieldSettingsOverview inventoryFieldSettingsOverview,
            List<InventoryDynamicFieldSettingOverview> inventoryDynamicFieldSettingOverviews)
        {
            this.InventoryFieldSettingsOverview = inventoryFieldSettingsOverview;
            this.InventoryDynamicFieldSettingOverviews = inventoryDynamicFieldSettingOverviews;
        }

        public InventoryFieldSettingsOverview InventoryFieldSettingsOverview { get; set; }

        public List<InventoryDynamicFieldSettingOverview> InventoryDynamicFieldSettingOverviews { get; set; }
    }
}
