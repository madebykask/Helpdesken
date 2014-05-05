namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingsOverviewWithType
    {
        public InventoryFieldSettingsOverviewWithType(
            int inventoryTypeId,
            string inventoryTypeName,
            InventoryFieldSettingsOverview inventoryFieldSettingsOverview)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryTypeName = inventoryTypeName;
            this.InventoryFieldSettingsOverview = inventoryFieldSettingsOverview;
        }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [NotNullAndEmpty]
        public string InventoryTypeName { get; private set; }

        [NotNull]
        public InventoryFieldSettingsOverview InventoryFieldSettingsOverview { get; private set; }
    }
}