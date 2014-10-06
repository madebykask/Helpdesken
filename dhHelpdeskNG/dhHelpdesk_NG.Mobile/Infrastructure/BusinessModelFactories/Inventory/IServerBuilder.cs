namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server;

    public interface IServerBuilder
    {
        ServerForUpdate BuildForUpdate(ServerViewModel model, OperationContext context);

        ServerForInsert BuildForAdd(ServerViewModel model, OperationContext context);
    }
}