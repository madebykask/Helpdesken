namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer;

    public interface IPrinterFieldsSettingsBuilder
    {
        PrinterFieldsSettings BuildViewModel(
            PrinterFieldsSettingsViewModel settings,
            int customerId);
    }
}