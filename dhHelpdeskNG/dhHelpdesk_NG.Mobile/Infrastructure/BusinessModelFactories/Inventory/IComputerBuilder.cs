namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer;

    public interface IComputerBuilder
    {
        ComputerForUpdate BuildForUpdate(ComputerViewModel model, OperationContext contex);

        ComputerForInsert BuildForAdd(ComputerViewModel model, OperationContext context);
    }
}