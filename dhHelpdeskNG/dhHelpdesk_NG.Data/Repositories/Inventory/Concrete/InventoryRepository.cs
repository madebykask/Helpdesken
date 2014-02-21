namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryRepository : Repository, IInventoryRepository
    {
        public InventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
