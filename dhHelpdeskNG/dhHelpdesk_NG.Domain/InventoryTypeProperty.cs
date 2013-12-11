using System;

namespace dhHelpdesk_NG.Domain
{
    public class InventoryTypeProperty : Entity
    {
        public int InventoryType_Id { get; set; }
        public int PropertyPos { get; set; }
        public int PropertyType { get; set; }
        public int PropertySize { get; set; }
        public int Show { get; set; }
        public int ShowInList { get; set; }
        public string PropertyDefault { get; set; }
        public string PropertyValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual InventoryType InventoryType { get; set; }
    }
}
