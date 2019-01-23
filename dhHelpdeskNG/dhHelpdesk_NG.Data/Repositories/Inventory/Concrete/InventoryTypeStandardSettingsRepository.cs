using System.Linq;
using DH.Helpdesk.Dal.Dal;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Inventory;

namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    public class InventoryTypeStandardSettingsRepository : 
        Repository<InventoryTypeStandardSettings>, 
        IInventoryTypeStandardSettingsRepository
    {
        public InventoryTypeStandardSettingsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public InventoryTypeStandardSettings GetCustomerSettings(int customerId)
        {
            return DbContext.InventoryTypeStandardSettings.FirstOrDefault(x => x.Customer_Id == customerId);
        }
    }
}