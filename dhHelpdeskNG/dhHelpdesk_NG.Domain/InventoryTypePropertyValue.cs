using System;

namespace dhHelpdesk_NG.Domain
{
    public class InventoryTypePropertyValue : Entity
    {
        public int Inventory_Id { get; set; }
        public int InventoryTypeProperty_Id { get; set; }
        public string Name { get; set; }
    }
}
