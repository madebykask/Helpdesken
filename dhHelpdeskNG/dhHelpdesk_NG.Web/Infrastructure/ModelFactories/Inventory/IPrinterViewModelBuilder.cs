namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;

    public interface IPrinterViewModelBuilder
    {
        PrinterViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Printer.Printer model,
            PrinterEditOptionsResponse options,
            PrinterFieldsSettingsForModelEdit settings);

        PrinterViewModel BuildViewModel(
            PrinterEditOptionsResponse options,
            PrinterFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}