namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer;

    public interface IComputerFieldsSettingsBuilder
    {
        ComputerFieldsSettings BuildViewModel(
            ComputerFieldsSettingsViewModel settings,
            int languageId);

        WorkstationTabsSettings BuildTabsViewModel(
            WorkstationTabsSettingsModel settings,
            int customerId);
    }
}
