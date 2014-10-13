namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Inventory;

    public interface IInventoryValueBuilder
    {
        List<InventoryValueForWrite> BuildForWrite(int inventoryId, List<DynamicFieldModel> models);
    }
}