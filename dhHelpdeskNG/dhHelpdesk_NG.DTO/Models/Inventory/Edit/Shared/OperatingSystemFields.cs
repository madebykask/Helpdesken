namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFields
    {
        public OperatingSystemFields(
            int? operatingSystemId,
            string version,
            string servicePackSystem,
            string registrationCodeSystem,
            string productKey)
        {
            this.OperatingSystemId = operatingSystemId;
            this.Version = version;
            this.ServicePack = servicePackSystem;
            this.RegistrationCode = registrationCodeSystem;
            this.ProductKey = productKey;
        }

        [IsId]
        public int? OperatingSystemId { get; set; }

        public string Version { get; set; }

        public string ServicePack { get; set; }

        public string RegistrationCode { get; set; }

        public string ProductKey { get; set; }

        public static OperatingSystemFields CreateDefault()
        {
            return new OperatingSystemFields(null, null, null, null, null);
        }
    }
}