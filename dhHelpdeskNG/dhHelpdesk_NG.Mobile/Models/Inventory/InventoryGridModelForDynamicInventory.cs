namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryGridModelForDynamicInventory
    {
        public InventoryGridModelForDynamicInventory(InventoryGridModel inventoryGridModel, int parentId)
        {
            this.InventoryGridModel = inventoryGridModel;
            this.ParentId = parentId;
        }

        [NotNull]
        public InventoryGridModel InventoryGridModel { get; set; }

        [IsId]
        public int ParentId { get; set; }
    }
}