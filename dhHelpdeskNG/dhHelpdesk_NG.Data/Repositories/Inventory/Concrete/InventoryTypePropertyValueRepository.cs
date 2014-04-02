namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypePropertyValueRepository : Repository<Domain.Inventory.InventoryTypePropertyValue>, IInventoryTypePropertyValueRepository
    {
        public InventoryTypePropertyValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<InventoryValue> GetData(List<int> ids)
        {
            var entities = this.DbSet.Where(x => ids.Contains(x.Inventory_Id)).ToList();

            var overviews = entities.Select(
                x => new InventoryValue(x.Inventory_Id, x.InventoryTypeProperty_Id, x.Value)).ToList();

            return overviews;
        }
    }
}