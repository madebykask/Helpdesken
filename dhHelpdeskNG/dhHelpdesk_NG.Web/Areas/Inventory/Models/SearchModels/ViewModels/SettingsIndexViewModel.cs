using System.Linq;
using DH.Helpdesk.BusinessData.Models.Inventory.Output;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using System.Collections.Generic;

    public class SettingsIndexViewModel : IndexModel
    {
        public SettingsIndexViewModel(IList<InventoryTypeOverview> inventoryTypes)
        {
            InventoryTypes = inventoryTypes;
        }

        public IList<InventoryTypeOverview> InventoryTypes { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override Tabs Tab
        {
            get { return Tabs.Settings; }
        }
    }
}