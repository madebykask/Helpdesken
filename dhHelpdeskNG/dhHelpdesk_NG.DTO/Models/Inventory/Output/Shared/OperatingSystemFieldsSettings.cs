namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class OperatingSystemFieldsSettings
    {
        public OperatingSystemFieldsSettings(
            int? operatingSystemFieldSetting, 
            string versionFieldSetting, 
            string servicePackSystemFieldSetting, 
            string registrationCodeSystemFieldSetting, 
            string productKeyFieldSetting)
        {
            this.OperatingSystemId = operatingSystemFieldSetting;
            this.Version = versionFieldSetting;
            this.ServicePack = servicePackSystemFieldSetting;
            this.RegistrationCode = registrationCodeSystemFieldSetting;
            this.ProductKey = productKeyFieldSetting;
        }

        public int? OperatingSystemId { get; set; }

        public string Version { get; set; }

        public string ServicePack { get; set; }

        public string RegistrationCode { get; set; }

        public string ProductKey { get; set; }
    }
}