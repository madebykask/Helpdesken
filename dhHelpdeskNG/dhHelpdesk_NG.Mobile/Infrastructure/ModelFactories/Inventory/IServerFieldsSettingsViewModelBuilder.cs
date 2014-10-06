namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Server;

    public interface IServerFieldsSettingsViewModelBuilder
    {
        ServerFieldsSettingsViewModel BuildViewModel(ServerFieldsSettings settings, List<ItemOverview> langauges, int langaugeId);
    }
}