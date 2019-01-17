using System;

namespace DH.Helpdesk.Domain.Inventory
{
    public class InventoryType : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public string XMLElement { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
