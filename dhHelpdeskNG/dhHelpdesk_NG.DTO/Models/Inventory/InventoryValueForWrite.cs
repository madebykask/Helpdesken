namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryValueForWrite
    {
        public InventoryValueForWrite(int inventoryId, int inventoryTypePropertyId, string value)
        {
            this.InventoryId = inventoryId;
            this.InventoryTypePropertyId = inventoryTypePropertyId;
            this.Value = value;
        }

        [IsId]
        public int InventoryId { get; private set; }

        [IsId]
        public int InventoryTypePropertyId { get; private set; }

        public string Value { get; private set; }
    }
}