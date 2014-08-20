namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;

    public interface IPrinterValidator
    {
        void Validate(Printer updatedPrinter, Printer existingPrinter, PrinterFieldsSettingsProcessing settings);

        void Validate(Printer newPrinter, PrinterFieldsSettingsProcessing settings);
    }
}