namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryFieldSettingsRepository
    {
        public InventoryFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Update(InventoryFieldSettings businessModel)
        {
            throw new System.NotImplementedException();
        }

        public InventoryFieldSettings GetFieldSettingsForEdit(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }

        public InventoryFieldSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }

        public InventoryFieldSettingsOverview GetFieldSettingsOverview(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }

        public InventoryFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }
    }
}