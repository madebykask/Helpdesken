namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;

    public interface IServerValidator
    {
        void Validate(Server updatedServer, Server existingServer, ServerFieldsSettingsProcessing settings);

        void Validate(Server newServer, ServerFieldsSettingsProcessing settings);
    }
}