namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerInventoryRepository : Repository, IComputerInventoryRepository
    {
        public ComputerInventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewComputerInventory businessModel)
        {
            var entity = new ComputerInventory
                             {
                                 Computer_Id = businessModel.ComputerId,
                                 Inventory_Id = businessModel.InventoryId
                             };

            this.DbContext.ComputerInventories.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.ComputerInventories.Find(id);
            this.DbContext.ComputerInventories.Remove(entity);
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
    }
}