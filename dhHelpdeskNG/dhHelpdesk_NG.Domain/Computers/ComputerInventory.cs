namespace DH.Helpdesk.Domain.Computers
{
    public class ComputerInventory : Entity
    {
        public int Computer_Id { get; set; }

        public int Inventory_Id { get; set; }

        public virtual Computer Computer {get; set; }
    }
}