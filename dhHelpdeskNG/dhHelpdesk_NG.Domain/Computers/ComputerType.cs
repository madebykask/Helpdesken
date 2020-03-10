namespace DH.Helpdesk.Domain.Computers
{
    using Inventory;
    using global::System;

    public class ComputerType : Entity
    {
        public int Customer_Id { get; set; }
        public string ComputerTypeDescription { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? InventoryType_Id { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Inventory inventory { get; set; }
    }
}
