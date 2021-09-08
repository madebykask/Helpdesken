namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFields
    {
        public PlaceFields(
            int? buildingId,
            int? floorId,
            int? roomId,
            string address,
            string postalCode,
            string postalAddress,
            string location,
            string location2)
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

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PostalAddress { get; set; }

        public string Location { get; set; }

        public string Location2 { get; set; }

        public static PlaceFields CreateDefault()
        {
            return new PlaceFields(null, null,null, null, null, null, null, null);
        }
    }
}