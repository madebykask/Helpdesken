namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Inventory;

    public class InventoryTypeGroupRepository : Repository<InventoryTypeGroup>, IInventoryTypeGroupRepository
    {
        public InventoryTypeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<TypeGroupModel> Find(int inventoryTypeId)
        {
            var anonymus =
                this.DbSet
                    .Where(x => x.InventoryType_Id == inventoryTypeId)
                    .Select(c => new { c.Name, c.Id, c.SortOrder })
                    .ToList();

            var models = anonymus.Select(c => new TypeGroupModel(c.Id, c.SortOrder, c.Name)).OrderBy(x => x.SortOrder).ToList();

            return models;
        }

        public void DeleteByInventoryTypeId(int inventoryTypeId)
        {
            var models = this.DbSet.Where(x => x.InventoryType_Id == inventoryTypeId).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }
    }
}