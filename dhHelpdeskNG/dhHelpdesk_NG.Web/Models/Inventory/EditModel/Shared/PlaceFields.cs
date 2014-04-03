namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFields
    {
        public PlaceFields(int? buildingId, int? floorId, int? roomId, ConfigurableFieldModel<string> location)
        {
            this.BuildingId = buildingId;
            this.FloorId = floorId;
            this.RoomId = roomId;
            this.Location = location;
        }

        [IsId]
        public int? BuildingId { get; set; }

        [IsId]
        public int? FloorId { get; set; }

        [IsId]
        public int? RoomId { get; set; }

        public ConfigurableFieldModel<string> Location { get; set; }
    }
}