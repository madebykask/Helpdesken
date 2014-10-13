namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Inventory;

    public class InventoryValueBuilder : IInventoryValueBuilder
    {
        public List<InventoryValueForWrite> BuildForWrite(int inventoryId, List<DynamicFieldModel> models)
        {
            var businessModels =
                models.Select(
                    model => new InventoryValueForWrite(inventoryId, model.InventoryTypePropertyId, model.Value))
                    .ToList();

            return businessModels;
        }
    }
}