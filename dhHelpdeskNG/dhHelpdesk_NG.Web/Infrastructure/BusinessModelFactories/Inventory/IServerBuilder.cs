using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server;

    public interface IServerBuilder
    {
        ServerForUpdate BuildForUpdate(ServerViewModel model, OperationContext context);

        ServerForInsert BuildForAdd(ServerViewModel model, OperationContext context, ComputerFile computerFile);
    }
}