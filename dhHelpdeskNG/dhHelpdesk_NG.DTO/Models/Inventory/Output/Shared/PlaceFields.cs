namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class PlaceFields
    {
        public PlaceFields(string roomName, string location)
        {
            this.RoomName = roomName;
            this.Location = location;
        }

        public string RoomName { get; set; }

        public string Location { get; set; }
    }
}