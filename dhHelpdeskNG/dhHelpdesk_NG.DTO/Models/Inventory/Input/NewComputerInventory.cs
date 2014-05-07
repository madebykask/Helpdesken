namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerInventory
    {
        public ComputerInventory(int computerId, int inventoryId)
        {
            this.ComputerId = computerId;
            this.InventoryId = inventoryId;
        }

        [IsId]
        public int ComputerId { get; private set; }

        [IsId]
        public int InventoryId { get; private set; }
    }
}