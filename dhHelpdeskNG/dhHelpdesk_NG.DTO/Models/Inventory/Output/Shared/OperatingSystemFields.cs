namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class OperatingSystemFields
    {
        public OperatingSystemFields(
            string operatingSystemName, 
            string version, 
            string servicePackSystem, 
            string registrationCodeSystem, 
            string productKey)
        {
            this.OperatingSystemName = operatingSystemName;
            this.Version = version;
            this.ServicePack = servicePackSystem;
            this.RegistrationCode = registrationCodeSystem;
            this.ProductKey = productKey;
        }

        public string OperatingSystemName { get; set; }

        public string Version { get; set; }

        public string ServicePack { get; set; }

        public string RegistrationCode { get; set; }

        public string ProductKey { get; set; }
    }
}