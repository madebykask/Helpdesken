namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;

    public interface IInventoryValueBuilder
    {
        List<InventoryValueForWrite> BuildForWrite(int inventoryId, List<DynamicFieldModel> models);
    }
}