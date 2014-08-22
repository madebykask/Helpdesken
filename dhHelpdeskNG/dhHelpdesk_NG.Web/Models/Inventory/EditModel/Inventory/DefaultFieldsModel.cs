namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldsModel
    {
        public DefaultFieldsModel()
        {
        }

        public DefaultFieldsModel(
            ConfigurableFieldModel<int?> departmentId,
            int? buildingId,
            int? floorId,
            ConfigurableFieldModel<int?> roomId,
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

        public ConfigurableFieldModel<int?> DepartmentId { get; set; }

        [IsId]
        public int? BuildingId { get; set; }

        [IsId]
        public int? FloorId { get; set; }

        public ConfigurableFieldModel<int?> RoomId { get; set; }

        [IsId]
        public int? ChangeByUserId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Model { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> SerialNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> TheftMark { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> BarCode { get; set; }

        public ConfigurableFieldModel<DateTime?> PurchaseDate { get; set; }

        public ConfigurableFieldModel<string> ComputerName { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }
    }
}