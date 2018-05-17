namespace DH.Helpdesk.Domain.Inventory
{
    using DH.Helpdesk.Common.Collections;

    using global::System;

    public class InventoryTypeProperty : Entity, INamedObject
    {
        public int InventoryType_Id { get; set; }
        public int? InventoryTypeGroup_Id { get; set; }
        public int PropertyPos { get; set; }
        public int PropertyType { get; set; }
        public int PropertySize { get; set; }
        public int Show { get; set; }
        public int ShowInList { get; set; }
        public string PropertyDefault { get; set; }
        public string PropertyValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string XMLTag { get; set; }
        public int ReadOnly { get; set; }

        public virtual InventoryType InventoryType { get; set; }

        public virtual InventoryTypeGroup InventoryTypeGroup { get; set; }

        public string GetName()
        {
            return this.PropertyType.ToString();
        }
    }
}
