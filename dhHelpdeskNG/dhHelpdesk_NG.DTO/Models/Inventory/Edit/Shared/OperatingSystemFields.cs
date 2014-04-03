namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFields
    {
        public OperatingSystemFields(
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

        [IsId]
        public int? OperatingSystemId { get; set; }

        public string Version { get; set; }

        public string ServicePack { get; set; }

        public string RegistrationCode { get; set; }

        public string ProductKey { get; set; }
    }
}