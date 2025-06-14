﻿namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server;

    public interface IServerFieldsSettingsBuilder
    {
        ServerFieldsSettings BuildViewModel(
            ServerFieldsSettingsViewModel settings,
            int customerId);
    }
}