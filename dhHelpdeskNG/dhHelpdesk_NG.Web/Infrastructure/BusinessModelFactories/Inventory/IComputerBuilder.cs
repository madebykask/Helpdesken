namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;

    public interface IComputerBuilder
    {
        Computer BuildForUpdate(ComputerViewModel model);

        Computer BuildForAdd(ComputerViewModel model, OperationContext context);
    }
}