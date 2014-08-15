namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFields
    {
        // todo
        public PlaceFields(int? roomId, string location)
        {
            this.RoomId = roomId;
            this.Location = location;
        }

        public PlaceFields(int? buildingId, int? floorId, int? roomId, string location)
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

        public string Location { get; set; }

        public static PlaceFields CreateDefault()
        {
            return new PlaceFields(null, null);
        }
    }
}