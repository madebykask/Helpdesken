namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PlaceFields
    {
        public PlaceFields(int? building, int? floor, int? room, string location)
        {
            this.Building = building;
            this.Floor = floor;
            this.Room = room;
            this.Location = location;
        }

        [IsId]
        public int? Building { get; set; }

        [IsId]
        public int? Floor { get; set; }

        [IsId]
        public int? Room { get; set; }

        public string Location { get; set; }
    }
}