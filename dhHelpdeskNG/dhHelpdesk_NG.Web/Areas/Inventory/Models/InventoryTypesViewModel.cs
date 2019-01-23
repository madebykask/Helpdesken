using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain.Inventory;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public class InventoryTypesViewModel
    {
        public InventoryTypeStandardSettings StandardTypeSettings { get; set; }
        public List<ItemOverview> StandardInventoryTypes { get; set; }
        public List<ItemOverview> CustomInventoryTypes { get; set; }
    }
}