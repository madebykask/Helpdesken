namespace DH.Helpdesk.Domain.Inventory
{
    using global::System;

    public class InventoryTypeGroup : Entity
    {
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int InventoryType_Id { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual InventoryType InventoryType { get; set; }
    }
}
