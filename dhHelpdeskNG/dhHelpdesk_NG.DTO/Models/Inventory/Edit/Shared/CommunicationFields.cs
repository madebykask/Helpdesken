namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class CommunicationFields
    {
        public CommunicationFields(int? networkAdapter, string ipAddress, string macAddress)
        {
            this.NetworkAdapter = networkAdapter;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        public int? NetworkAdapter { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }
    }
}