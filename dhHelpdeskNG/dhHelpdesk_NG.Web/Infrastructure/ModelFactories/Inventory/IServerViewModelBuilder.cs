namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using System.Collections.Generic;

    public interface IServerViewModelBuilder
    {
        ServerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Server.ServerForRead model,
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings,
            List<string> fileUploadWhiteList);

        ServerViewModel BuildViewModel(
            ServerEditOptions options,
            ServerFieldsSettingsForModelEdit settings,
            int currentCustomerId,
            List<string> fileUploadWhiteList);
    }
}