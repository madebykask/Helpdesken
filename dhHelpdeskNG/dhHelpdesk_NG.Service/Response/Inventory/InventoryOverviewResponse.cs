namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;

    public class InventoryOverviewResponse
    {
        public InventoryOverviewResponse(List<InventoryOverview> overviews, List<InventoryValue> dynamicData)
        {
            this.Overviews = overviews;
            this.DynamicData = dynamicData;
        }

        public List<InventoryOverview> Overviews { get; private set; }

        public List<InventoryValue> DynamicData { get; private set; }
    }
}