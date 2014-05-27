namespace DH.Helpdesk.Domain.Computers
{
    using DH.Helpdesk.Domain.Inventory;

    public class ComputerInventory
    {
        public int Computer_Id { get; set; }

        public int Inventory_Id { get; set; }

        public virtual Computer Computer {get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}