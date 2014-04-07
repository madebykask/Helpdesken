namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFields
    {
        public CommunicationFields(int? networkAdapterId, string ipAddress, string macAddress)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        [IsId]
        public int? NetworkAdapterId { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }
    }
}