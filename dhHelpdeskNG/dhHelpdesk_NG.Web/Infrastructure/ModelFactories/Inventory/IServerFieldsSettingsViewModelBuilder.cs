namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server;

    public interface IServerFieldsSettingsViewModelBuilder
    {
        ServerFieldsSettingsViewModel BuildViewModel(ServerFieldsSettings settings);
    }
}