namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;

    public class InventoriesFieldSettingsOverviewResponse
    {
        public InventoriesFieldSettingsOverviewResponse(
            List<InventoryFieldSettingsOverviewWithType> inventoryFieldSettingsOverviews,
            List<InventoryDynamicFieldSettingOverviewWithType> inventoryDynamicFieldSettingOverviews)
        {
            this.InventoryFieldSettingsOverviews = inventoryFieldSettingsOverviews;
            this.InventoryDynamicFieldSettingOverviews = inventoryDynamicFieldSettingOverviews;
        }

        public List<InventoryFieldSettingsOverviewWithType> InventoryFieldSettingsOverviews { get; set; }

        public List<InventoryDynamicFieldSettingOverviewWithType> InventoryDynamicFieldSettingOverviews { get; set; }
    }
}