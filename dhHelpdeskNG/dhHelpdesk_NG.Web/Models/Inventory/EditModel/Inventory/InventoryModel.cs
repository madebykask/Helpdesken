namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryModel
    {
        public InventoryModel(ConfigurableFieldModel<string> computerName)
        {
            this.ComputerName = computerName;
        }

        public InventoryModel(
            int id,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changeDate,
            int inventoryTypeId,
            int? departmentId,
            int? buildingId,
            int? floorId,
            int? roomId,
            int? changeByUserId,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> model,
            ConfigurableFieldModel<string> manufacturer,
            ConfigurableFieldModel<string> serialNumber,
            ConfigurableFieldModel<string> theftMark,
            ConfigurableFieldModel<string> barCode,
            ConfigurableFieldModel<DateTime?> purchaseDate,
            ConfigurableFieldModel<string> computerName,
            ConfigurableFieldModel<string> info)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.ChangeDate = changeDate;
            this.InventoryTypeId = inventoryTypeId;
            this.DepartmentId = departmentId;
            this.BuildingId = buildingId;
            this.FloorId = floorId;
            this.RoomId = roomId;
            this.ChangeByUserId = changeByUserId;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.SerialNumber = serialNumber;
            this.TheftMark = theftMark;
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
            this.ComputerName = computerName;
            this.Info = info;
        }

        [IsId]
        public int Id { get; set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangeDate { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? BuildingId { get; set; }

        [IsId]
        public int? FloorId { get; set; }

        [IsId]
        public int? RoomId { get; set; }

        [IsId]
        public int? ChangeByUserId { get; private set; }

        [NotNullAndEmpty]
        public ConfigurableFieldModel<string> Name { get; private set; }

        public ConfigurableFieldModel<string> Model { get; private set; }

        [NotNullAndEmpty]
        public ConfigurableFieldModel<string> Manufacturer { get; private set; }

        [NotNullAndEmpty]
        public ConfigurableFieldModel<string> SerialNumber { get; private set; }

        [NotNullAndEmpty]
        public ConfigurableFieldModel<string> TheftMark { get; private set; }

        [NotNullAndEmpty]
        public ConfigurableFieldModel<string> BarCode { get; private set; }

        public ConfigurableFieldModel<DateTime?> PurchaseDate { get; private set; }

        public ConfigurableFieldModel<string> ComputerName { get; private set; }

        public ConfigurableFieldModel<string> Info { get; private set; }
    }
}