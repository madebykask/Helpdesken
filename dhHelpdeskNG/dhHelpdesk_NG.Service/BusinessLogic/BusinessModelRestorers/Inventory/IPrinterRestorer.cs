namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;

    public interface IPrinterRestorer
    {
        void Restore(PrinterForUpdate printer, PrinterForRead existingPrinter, PrinterFieldsSettingsProcessing settings);
    }
}