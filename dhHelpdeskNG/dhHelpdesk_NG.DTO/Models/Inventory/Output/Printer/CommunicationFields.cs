namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    public class CommunicationFields
    {
        public CommunicationFields(string networkAdapterName, string ipAddress, string macAddress)
        {
            this.NetworkAdapterName = networkAdapterName;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        public string NetworkAdapterName { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }
    }
}