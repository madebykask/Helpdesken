namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypePropertyRepository : Repository, IInventoryTypePropertyRepository
    {
        public InventoryTypePropertyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}