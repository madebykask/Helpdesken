namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSettingOverviewWithType
    {
        public InventoryDynamicFieldSettingOverviewWithType(
            int inventoryTypeId,
            List<InventoryDynamicFieldSettingOverview> inventoryDynamicFieldSettingOverviews)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryDynamicFieldSettingOverviews = inventoryDynamicFieldSettingOverviews;
        }

        public int InventoryTypeId { get; private set; }

        [NotNull]
        public List<InventoryDynamicFieldSettingOverview> InventoryDynamicFieldSettingOverviews { get; private set; }
    }
}