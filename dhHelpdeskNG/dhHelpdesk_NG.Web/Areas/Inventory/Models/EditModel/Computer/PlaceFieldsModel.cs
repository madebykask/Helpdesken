namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFieldsModel
    {
        public PlaceFieldsModel()
        {
        }

        public PlaceFieldsModel(
            ConfigurableFieldModel<int?> buildingId,
            ConfigurableFieldModel<int?> floorId,
            ConfigurableFieldModel<int?> roomId,
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

        [NotNull]
        public ConfigurableFieldModel<int?> BuildingId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> FloorId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> RoomId { get; set; }

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