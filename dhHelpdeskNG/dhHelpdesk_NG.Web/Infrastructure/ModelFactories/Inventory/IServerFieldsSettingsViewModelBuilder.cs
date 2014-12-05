namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server;

    public interface IServerFieldsSettingsViewModelBuilder
    {
        ServerFieldsSettingsViewModel BuildViewModel(ServerFieldsSettings settings, List<ItemOverview> langauges, int langaugeId);
    }
}