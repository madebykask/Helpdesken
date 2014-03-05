namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Computer
{
    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(string buildingName, string floorName, string roomName, string address, string postalCode, string postalAddress, string location, string location2)
        {
            this.BuildingName = buildingName;
            this.FloorName = floorName;
            this.RoomName = roomName;
            this.Address = address;
            this.PostalCode = postalCode;
            this.PostalAddress = postalAddress;
            this.Location = location;
            this.Location2 = location2;
        }

        public string BuildingName { get; set; }

        public string FloorName { get; set; }

        public string RoomName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PostalAddress { get; set; }

        public string Location { get; set; }

        public string Location2 { get; set; }
    }
}