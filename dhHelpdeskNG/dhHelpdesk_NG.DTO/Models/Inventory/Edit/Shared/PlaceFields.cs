namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class PlaceFields
    {
        public PlaceFields(int? building, int? floor, int? room, string location)
        {
            this.Building = building;
            this.Floor = floor;
            this.Room = room;
            this.Location = location;
        }

        public int? Building { get; set; }

        public int? Floor { get; set; }

        public int? Room { get; set; }

        public string Location { get; set; }
    }
}