namespace DH.Helpdesk.Dal.Repositories.Servers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IServerFieldSettingsRepository : INewRepository
    {
        void Update(ServerFieldsSettings businessModel);

        ServerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId);

        ServerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false);

        ServerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId);

        ServerFieldsSettingsProcessing GetFieldSettingsProcessing(int customerId);
    }
}