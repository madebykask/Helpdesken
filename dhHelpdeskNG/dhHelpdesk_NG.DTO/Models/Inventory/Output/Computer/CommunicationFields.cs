namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFields
    {
        public CommunicationFields(
            int? networkAdapterId,
            string ipAddress,
            string macAddress,
            bool ras,
            string novellClient)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.RAS = ras;
            this.NovellClient = novellClient;
        }

        [IsId]
        public int? NetworkAdapterId { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }

        public bool RAS { get; set; }

        public string NovellClient { get; set; }
    }
}