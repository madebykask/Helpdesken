namespace DH.Helpdesk.Domain.Inventory
{
    using Computers;
    using global::System;

    public class Inventory : Entity
    {
        public int? ChangedByUser_Id { get; set; }
        public int? Department_Id { get; set; }
        public int InventoryType_Id { get; set; }
        public int? Room_Id { get; set; }
        public string BarCode { get; set; }
        public string Info { get; set; }
        public string InventoryModel { get; set; }
        public string InventoryName { get; set; }
        public string Manufacturer { get; set; }
        public string SerialNumber { get; set; }
        public string TheftMark { get; set; }
        public int? ComputerType_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }

        public virtual InventoryType InventoryType { get; set; }
        public virtual Room Room { get; set; }
        public virtual Department Department { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ComputerType ComputerType { get; set; }
    }
}
