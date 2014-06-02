namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer;

    public interface IPrinterFieldsSettingsViewModelBuilder
    {
        PrinterFieldsSettingsViewModel BuildViewModel(PrinterFieldsSettings settings);
    }
}