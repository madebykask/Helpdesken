namespace DH.Helpdesk.Domain.Inventory
{
    public class InventoryTypePropertyValue : Entity
    {
        public int Inventory_Id { get; set; }
        public int InventoryTypeProperty_Id { get; set; }
        public string Name { get; set; }
    }
}
