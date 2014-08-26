namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryForUpdate : Inventory
    {
        public InventoryForUpdate(
            int? departmentId,
            int? roomId,
            string name,
            string model,
            string manufacturer,
            string serialNumber,
            string theftMark,
            string barCode,
            DateTime? purchaseDate,
            string info,
            DateTime changeDate,
            int? changeByUserId)
            : base(departmentId, roomId, name, model, manufacturer, serialNumber, theftMark, barCode, purchaseDate, info
                )
        {
            this.ChangeDate = changeDate;
            this.ChangeByUserId = changeByUserId;
        }

        public DateTime ChangeDate { get; private set; }

        [IsId]
        public int? ChangeByUserId { get; private set; }
    }
}