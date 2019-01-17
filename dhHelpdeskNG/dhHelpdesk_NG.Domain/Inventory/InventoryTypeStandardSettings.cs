namespace DH.Helpdesk.Domain.Inventory
{
    public class InventoryTypeStandardSettings : Entity
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }

        public bool ShowPrinters { get; set; }
        public bool ShowWorkstations { get; set; }
        public bool ShowServers { get; set; }

        public virtual Customer Customer { get; set; }
    }
}