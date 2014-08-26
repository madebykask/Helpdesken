namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;

    public interface IInventoryModelBuilder
    {
        InventoryForUpdate BuildForUpdate(InventoryViewModel model, OperationContext contex);

        InventoryForInsert BuildForAdd(InventoryViewModel model, OperationContext context);
    }
}
