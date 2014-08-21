namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;

    public interface IServerValidator
    {
        void Validate(ServerForUpdate updatedServer, ServerForRead existingServer, ServerFieldsSettingsProcessing settings);

        void Validate(ServerForInsert newServer, ServerFieldsSettingsProcessing settings);
    }
}