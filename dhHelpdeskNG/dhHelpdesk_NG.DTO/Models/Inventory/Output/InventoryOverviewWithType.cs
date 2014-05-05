namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using System.Collections.Generic;

    public class InventoryOverviewWithType
    {
        public InventoryOverviewWithType(int inventoryTypeId, List<InventoryOverview> inventoryOverviews)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryOverviews = inventoryOverviews;
        }

        public int InventoryTypeId { get; private set; }

        public List<InventoryOverview> InventoryOverviews { get; private set; }
    }
}