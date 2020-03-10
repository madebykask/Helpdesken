namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryForRead : Inventory
    {
        public InventoryForRead(
            int id,
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
            DateTime changeDate,
            string[] workstations,
            int inventoryTypeId,
            string inventoryTypeName,
            int? buildingId,
            int? floorId,
            DateTime? syncChangeDate)
            : base(departmentId, roomId, name, model, manufacturer, serialNumber, theftMark, computerTypeId, barCode, purchaseDate, info)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.ChangeDate = changeDate;
            this.Workstations = workstations;
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryTypeName = inventoryTypeName;
            this.BuildingId = buildingId;
            this.FloorId = floorId;
            this.SyncChangeDate = syncChangeDate;
        }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangeDate { get; private set; }

        public string[] Workstations { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        public string InventoryTypeName { get; private set; }

        [IsId]
        public int? BuildingId { get; private set; }

        [IsId]
        public int? FloorId { get; private set; }

        public DateTime? SyncChangeDate { get; private set; }
    }
}