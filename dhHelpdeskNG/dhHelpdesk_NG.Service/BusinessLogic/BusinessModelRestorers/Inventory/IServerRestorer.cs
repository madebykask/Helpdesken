namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;

    public interface IServerRestorer
    {
        void Restore(Server server, Server existingServer, ServerFieldsSettingsProcessing settings);
    }
}