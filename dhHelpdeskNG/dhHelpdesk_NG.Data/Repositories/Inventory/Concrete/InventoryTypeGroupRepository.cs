namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypeGroupRepository : Repository, IInventoryTypeGroupRepository
    {
        public InventoryTypeGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<ItemOverview> FindOverviews(int inventoryTypeId)
        {
            var anonymus =
                this.DbContext.InventoryTypeGroups
                    .Where(x => x.InventoryType_Id == inventoryTypeId)
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}