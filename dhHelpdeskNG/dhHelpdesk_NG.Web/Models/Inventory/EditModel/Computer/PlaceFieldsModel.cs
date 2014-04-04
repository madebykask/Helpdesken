namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsModel
    {
        public PlaceFieldsModel(
            int? buildingId,
            int? floorId,
            int? roomId,
            ConfigurableFieldModel<string> address,
            ConfigurableFieldModel<string> postalCode,
            ConfigurableFieldModel<string> postalAddress,
            ConfigurableFieldModel<string> location,
            ConfigurableFieldModel<string> location2)
        {
            this.BuildingId = buildingId;
            this.FloorId = floorId;
            this.RoomId = roomId;
            this.Address = address;
            this.PostalCode = postalCode;
            this.PostalAddress = postalAddress;
            this.Location = location;
            this.Location2 = location2;
        }

        [IsId]
        public int? BuildingId { get; set; }

        [IsId]
        public int? FloorId { get; set; }

        [IsId]
        public int? RoomId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Address { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> PostalCode { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> PostalAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Location { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Location2 { get; set; }
    }
}