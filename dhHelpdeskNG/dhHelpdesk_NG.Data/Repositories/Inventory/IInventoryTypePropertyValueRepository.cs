namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Dal.Dal;

    public interface IInventoryTypePropertyValueRepository : INewRepository
    {
        List<InventoryValue> GetData(List<int> ids);
    }
}