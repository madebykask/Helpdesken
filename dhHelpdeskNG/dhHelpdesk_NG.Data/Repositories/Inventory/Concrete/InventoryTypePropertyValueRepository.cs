namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypePropertyValueRepository : Repository<InventoryTypeProperty>, IInventoryTypePropertyValueRepository
    {
        public InventoryTypePropertyValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryDynamicFieldSetting businessModel)
        {
            throw new System.NotImplementedException();
        }

        public void Add(InventorySettings businessModel)
        {
            throw new System.NotImplementedException();
        }

        public void Update(InventorySettings businessModel)
        {
            throw new System.NotImplementedException();
        }
    }
}