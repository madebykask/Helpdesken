namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerFieldSettingsRepository : Repository<Domain.Computers.ComputerFieldSettings>, IComputerFieldSettingsRepository
    {
        public ComputerFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Update(ComputerFieldsSettings businessModel)
        {
            throw new System.NotImplementedException();
        }

        public ComputerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public ComputerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public ComputerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }
    }
}