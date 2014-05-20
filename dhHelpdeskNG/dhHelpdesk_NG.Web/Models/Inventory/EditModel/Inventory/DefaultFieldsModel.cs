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
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? BuildingId { get; set; }

        [IsId]
        public int? FloorId { get; set; }

        [IsId]
        public int? RoomId { get; set; }

        [IsId]
        public int? ChangeByUserId { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; private set; }

        public ConfigurableFieldModel<string> Model { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manufacturer { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> SerialNumber { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> TheftMark { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> BarCode { get; private set; }

        public ConfigurableFieldModel<DateTime?> PurchaseDate { get; private set; }

        public ConfigurableFieldModel<string> ComputerName { get; private set; }

        public ConfigurableFieldModel<string> Info { get; private set; }
    }
}