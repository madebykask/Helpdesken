namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer;

    public interface IPrinterFieldsSettingsViewModelBuilder
    {
        PrinterFieldsSettingsViewModel BuildViewModel(PrinterFieldsSettings settings, List<ItemOverview> langauges, int langaugeId);
    }
}