namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Printer;

    public interface IPrinterBuilder
    {
        PrinterForUpdate BuildForUpdate(PrinterViewModel model, OperationContext context);

        PrinterForInsert BuildForAdd(PrinterViewModel model, OperationContext context);
    }
}