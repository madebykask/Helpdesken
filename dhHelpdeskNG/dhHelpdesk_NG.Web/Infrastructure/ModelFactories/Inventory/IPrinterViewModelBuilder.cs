namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;
    using DH.Helpdesk.Web.Models.Inventory.OptionsAggregates;

    public interface IPrinterViewModelBuilder
    {
        PrinterViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Printer.PrinterForRead model,
            PrinterEditOptions options,
            PrinterFieldsSettingsForModelEdit settings);

        PrinterViewModel BuildViewModel(
            PrinterEditOptions options,
            PrinterFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}