namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Linq;

    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    using ComputerInventory = DH.Helpdesk.BusinessData.Models.Inventory.Input.ComputerInventory;

    public class ComputerInventoryRepository : Repository<Domain.Computers.ComputerInventory>, IComputerInventoryRepository
    {
        public ComputerInventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerInventory businessModel)
        {
            var entity = new Domain.Computers.ComputerInventory
                             {
                                 Computer_Id = businessModel.ComputerId,
                                 Inventory_Id = businessModel.InventoryId
                             };

            this.DbContext.ComputerInventories.Add(entity);
        }

        public void DeleteById(int computerId, int inventoryId)
        {
            var entity = this.DbSet.Single(x => x.Computer_Id == computerId && x.Inventory_Id == inventoryId);
            this.DbSet.Remove(entity);
        }

        public void DeleteByComputerId(int computerId)
        {
            var entities = this.DbContext.ComputerInventories.Where(x => x.Computer_Id == computerId).ToList();
            entities.ForEach(x => this.DbContext.ComputerInventories.Remove(x));
        }

        public void DeleteByInventoryId(int inventoryId)
        {
            var entities = this.DbContext.ComputerInventories.Where(x => x.Inventory_Id == inventoryId).ToList();
            entities.ForEach(x => this.DbContext.ComputerInventories.Remove(x));
        }

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.DbSet.Where(x => x.Inventory.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }
    }
}