using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Domain.Inventory;

namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    public interface IInventoryTypeStandardSettingsRepository : INewRepository
    {
        InventoryTypeStandardSettings GetCustomerSettings(int customerId);
    }
}