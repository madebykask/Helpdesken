namespace DH.Helpdesk.Domain.Inventory
{
    public class InventoryTypePropertyValue
    {
        public int Inventory_Id { get; set; }
        public int InventoryTypeProperty_Id { get; set; }
        public string Value { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual InventoryTypeProperty InventoryTypeProperty { get; set; }
    }
}
