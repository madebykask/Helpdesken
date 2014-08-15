namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFields
    {
        public CommunicationFields(
            int? networkAdapterId,
            string ipAddress,
            string macAddress,
            bool IsRas,
            string novellClient)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.IsRAS = IsRas;
            this.NovellClient = novellClient;
        }

        [IsId]
        public int? NetworkAdapterId { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }

        public bool IsRAS { get; set; }

        public string NovellClient { get; set; }

        public static CommunicationFields CreateDefault()
        {
            return new CommunicationFields(null, null, null, false, null);
        }
    }
}