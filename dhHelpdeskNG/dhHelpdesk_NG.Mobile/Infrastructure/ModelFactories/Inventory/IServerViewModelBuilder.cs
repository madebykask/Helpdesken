namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server;
    using DH.Helpdesk.Mobile.Models.Inventory.OptionsAggregates;

    public interface IServerViewModelBuilder
    {
        ServerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Server.ServerForRead model,
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings);

        ServerViewModel BuildViewModel(
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings,
            int currentCustomerId);
    }
}