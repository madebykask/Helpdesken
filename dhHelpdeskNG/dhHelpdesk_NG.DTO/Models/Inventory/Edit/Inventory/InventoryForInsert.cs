namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryForInsert : Inventory
    {
        public InventoryForInsert(
            int? departmentId,
            int? roomId,
            string name,
            string model,
            string manufacturer,
            string serialNumber,
            string theftMark,
            int? computerTypeId,
            string barCode,
            DateTime? purchaseDate,
            string info,
            DateTime createdDate,
            int inventoryTypeId,
            int? changeByUserId)
            : base(departmentId, roomId, name, model, manufacturer, serialNumber, theftMark, computerTypeId, barCode, purchaseDate, info
                )
        {
            this.CreatedDate = createdDate;
            this.InventoryTypeId = inventoryTypeId;
            this.ChangeByUserId = changeByUserId;
        }

        public DateTime CreatedDate { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? ChangeByUserId { get; private set; }
    }
}