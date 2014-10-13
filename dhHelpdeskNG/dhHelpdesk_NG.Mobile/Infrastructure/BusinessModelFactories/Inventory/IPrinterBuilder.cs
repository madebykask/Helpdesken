namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Printer;

    public interface IPrinterBuilder
    {
        PrinterForUpdate BuildForUpdate(PrinterViewModel model, OperationContext context);

        PrinterForInsert BuildForAdd(PrinterViewModel model, OperationContext context);
    }
}