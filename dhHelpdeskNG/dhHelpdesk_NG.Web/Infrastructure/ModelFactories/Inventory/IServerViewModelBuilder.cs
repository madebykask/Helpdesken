namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;

    public interface IServerViewModelBuilder
    {
        ServerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Server.Server model,
            ServerEditOptionsResponse options,
            ServerFieldsSettingsForModelEdit settings);

        ServerViewModel BuildViewModel(
            ServerEditOptionsResponse options,
            ServerFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}