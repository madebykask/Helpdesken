namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class PlaceFields
    {
        public PlaceFields(
            string roomName,
            string address,
            string postalCode,
            string postalAddress,
            string location,
            string location2)
        {
            this.RoomName = roomName;
            this.Address = address;
            this.PostalCode = postalCode;
            this.PostalAddress = postalAddress;
            this.Location = location;
            this.Location2 = location2;
        }

        public string RoomName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PostalAddress { get; set; }

        public string Location { get; set; }

        public string Location2 { get; set; }
    }
}