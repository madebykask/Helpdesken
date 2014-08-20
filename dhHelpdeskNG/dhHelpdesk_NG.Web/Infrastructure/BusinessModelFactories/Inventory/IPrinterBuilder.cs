namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;

    public interface IPrinterBuilder
    {
        Printer BuildForUpdate(PrinterViewModel model, OperationContext context);

        Printer BuildForAdd(PrinterViewModel model, OperationContext context);
    }
}