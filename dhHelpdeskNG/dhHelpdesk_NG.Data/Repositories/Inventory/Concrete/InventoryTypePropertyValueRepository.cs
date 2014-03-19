namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypePropertyValueRepository : Repository<InventoryTypePropertyValue>, IInventoryTypePropertyValueRepository
    {
        public InventoryTypePropertyValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public void Add(InventorySettings businessModel)
        {
            throw new System.NotImplementedException();
        }

    }
}