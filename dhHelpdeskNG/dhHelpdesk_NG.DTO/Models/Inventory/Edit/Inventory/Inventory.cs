namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Inventory : BusinessModel
    {
        private Inventory(ModelStates businessModelState, int inventoryTypeId, int? departmentId, int? roomId, int? changeByUserId, string name, string model, string manufacturer, string serialNumber, string theftMark, string barCode, DateTime? purchaseDate, string info, DateTime? syncChangeDate)
            : base(businessModelState)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.DepartmentId = departmentId;
            this.RoomId = roomId;
            this.ChangeByUserId = changeByUserId;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.SerialNumber = serialNumber;
            this.TheftMark = theftMark;
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
            this.Info = info;
            this.SyncChangeDate = syncChangeDate;
        }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? RoomId { get; private set; }

        [IsId]
        public int? ChangeByUserId { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Model { get; private set; }

        [NotNullAndEmpty]
        public string Manufacturer { get; private set; }

        [NotNullAndEmpty]
        public string SerialNumber { get; private set; }

        [NotNullAndEmpty]
        public string TheftMark { get; private set; }

        [NotNullAndEmpty]
        public string BarCode { get; private set; }

        public DateTime? PurchaseDate { get; private set; }

        public string Info { get; private set; }

        [AllowRead(ModelStates.Created | ModelStates.ForEdit)]
        public DateTime CreatedDate { get; private set; }

        [AllowRead(ModelStates.Updated | ModelStates.ForEdit)]
        public DateTime ChangeDate { get; private set; }

        public DateTime? SyncChangeDate { get; private set; }

        public static Inventory CreateNew(int inventoryTypeId, int? departmentId, int? roomId, int? changeByUserId, string name, string model, string manufacturer, string serialNumber, string theftMark, string barCode, DateTime? purchaseDate, string info, DateTime? syncChangeDate, DateTime createdDate)
        {
            var businessModel = new Inventory(
                ModelStates.Created,
                inventoryTypeId,
                departmentId,
                roomId,
                changeByUserId,
                name,
                model,
                manufacturer,
                serialNumber,
                theftMark,
                barCode,
                purchaseDate,
                info,
                syncChangeDate) { CreatedDate = createdDate };

            return businessModel;
        }

        public static Inventory CreateUpdated(int inventoryTypeId, int id, int? departmentId, int? roomId, int? changeByUserId, string name, string model, string manufacturer, string serialNumber, string theftMark, string barCode, DateTime? purchaseDate, string info, DateTime? syncChangeDate, DateTime changedDate)
        {
            var businessModel = new Inventory(
                ModelStates.Updated,
                inventoryTypeId,
                departmentId,
                roomId,
                changeByUserId,
                name,
                model,
                manufacturer,
                serialNumber,
                theftMark,
                barCode,
                purchaseDate,
                info,
                syncChangeDate) { Id = id, ChangeDate = changedDate };

            return businessModel;
        }

        public static Inventory CreateForEdit(int inventoryTypeId, int id, int? departmentId, int? roomId, int? changeByUserId, string name, string model, string manufacturer, string serialNumber, string theftMark, string barCode, DateTime? purchaseDate, string info, DateTime? syncChangeDate, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new Inventory(
                ModelStates.ForEdit,
                inventoryTypeId,
                departmentId,
                roomId,
                changeByUserId,
                name,
                model,
                manufacturer,
                serialNumber,
                theftMark,
                barCode,
                purchaseDate,
                info,
                syncChangeDate) { Id = id, CreatedDate = createdDate, ChangeDate = changedDate };

            return businessModel;
        }
    }
}