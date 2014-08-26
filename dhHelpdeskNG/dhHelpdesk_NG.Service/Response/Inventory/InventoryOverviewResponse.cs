namespace DH.Helpdesk.Services.Response.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;

    public class InventoryOverviewResponse
    {
        public InventoryOverviewResponse(BusinessData.Models.Inventory.Edit.Inventory.InventoryForRead model, List<InventoryValue> dynamicData)
        {
            this.Inventory = model;
            this.DynamicData = dynamicData;
        }

        public BusinessData.Models.Inventory.Edit.Inventory.InventoryForRead Inventory { get; private set; }

        public List<InventoryValue> DynamicData { get; private set; }
    }
}