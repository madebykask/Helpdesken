namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Inventory;

    public interface IInventoryModelBuilder
    {
        InventoryForUpdate BuildForUpdate(InventoryViewModel model, OperationContext contex);

        InventoryForInsert BuildForAdd(InventoryViewModel model, OperationContext context);
    }
}
