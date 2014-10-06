namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Server;

    public interface IServerFieldsSettingsBuilder
    {
        ServerFieldsSettings BuildViewModel(
            ServerFieldsSettingsViewModel settings,
            int customerId);
    }
}