namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryDynamicFieldSettingsRepository : Repository<Domain.Inventory.InventoryTypeProperty>, IInventoryDynamicFieldSettingsRepository
    {
        public InventoryDynamicFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryDynamicFieldSetting businessModel)
        {
            throw new System.NotImplementedException();
        }

        public void Update(InventoryDynamicFieldSetting businessModel)
        {
            throw new System.NotImplementedException();
        }

        public InventoryDynamicFieldSetting GetFieldSettingsForEdit(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }

        public InventoryDynamicFieldSettingForModelEdit GetFieldSettingsForModelEdit(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }

        public IList<InventoryDynamicFieldSettingOverview> GetFieldSettingsOverview(int customerId, int inventoryTypeId)
        {
            throw new System.NotImplementedException();
        }
    }
}