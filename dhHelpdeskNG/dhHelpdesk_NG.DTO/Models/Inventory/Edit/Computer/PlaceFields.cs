namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class PlaceFields
    {
        public PlaceFields(int? building, int? floor, int? room, string address, string postalCode, string postalAddress, string location, string location2)
        {
            this.Building = building;
            this.Floor = floor;
            this.Room = room;
            this.Address = address;
            this.PostalCode = postalCode;
            this.PostalAddress = postalAddress;
            this.Location = location;
            this.Location2 = location2;
        }

        public int? Building { get; set; }

        public int? Floor { get; set; }

        public int? Room { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PostalAddress { get; set; }

        public string Location { get; set; }

        public string Location2 { get; set; }
    }
}