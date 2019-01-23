using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;

    public interface IComputerBuilder
    {
        ComputerForUpdate BuildForUpdate(ComputerViewModel model, OperationContext contex);

        ComputerForInsert BuildForAdd(ComputerViewModel model, OperationContext context, ComputerFile computerFile);
    }
}