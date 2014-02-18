namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewComputerInventory : INewBusinessModel
    {
        public NewComputerInventory(int computerId, int inventoryId)
        {
            this.ComputerId = computerId;
            this.InventoryId = inventoryId;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ComputerId { get; set; }

        [IsId]
        public int InventoryId { get; set; }
    }
}