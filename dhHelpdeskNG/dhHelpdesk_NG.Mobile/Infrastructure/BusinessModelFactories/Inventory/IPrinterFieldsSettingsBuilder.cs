namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Printer;

    public interface IPrinterFieldsSettingsBuilder
    {
        PrinterFieldsSettings BuildViewModel(
            PrinterFieldsSettingsViewModel settings,
            int customerId);
    }
}