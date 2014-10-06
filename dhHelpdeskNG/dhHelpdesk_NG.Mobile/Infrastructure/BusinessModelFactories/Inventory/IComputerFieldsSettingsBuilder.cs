namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Computer;

    public interface IComputerFieldsSettingsBuilder
    {
        ComputerFieldsSettings BuildViewModel(
            ComputerFieldsSettingsViewModel settings,
            int languageId);
    }
}
