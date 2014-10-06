namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer;

    public interface IComputerFieldsSettingsBuilder
    {
        ComputerFieldsSettings BuildViewModel(
            ComputerFieldsSettingsViewModel settings,
            int languageId);
    }
}
