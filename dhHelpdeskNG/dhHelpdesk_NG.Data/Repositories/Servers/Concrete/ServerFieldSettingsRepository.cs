namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerFieldSettingsRepository : Repository<Domain.Servers.ServerFieldSettings>, IServerFieldSettingsRepository
    {
        public ServerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Update(ServerFieldsSettings businessModel)
        {
            throw new System.NotImplementedException();
        }

        public ServerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public ServerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public ServerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }
    }
}