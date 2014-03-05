namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Shared
{
    public class PlaceFieldsSettings
    {
        public PlaceFieldsSettings(string buildingName, string floorName, string roomName, string location)
        {
            this.BuildingName = buildingName;
            this.FloorName = floorName;
            this.RoomName = roomName;
            this.Location = location;
        }

        public string BuildingName { get; set; }

        public string FloorName { get; set; }

        public string RoomName { get; set; }

        public string Location { get; set; }
    }
}