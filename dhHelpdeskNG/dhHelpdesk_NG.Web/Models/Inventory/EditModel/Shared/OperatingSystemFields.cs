namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFields
    {
        public OperatingSystemFields(
            int? operatingSystemFieldSetting,
            ConfigurableFieldModel<string> versionFieldSetting,
            ConfigurableFieldModel<string> servicePackSystemFieldSetting,
            ConfigurableFieldModel<string> registrationCodeSystemFieldSetting,
            ConfigurableFieldModel<string> productKeyFieldSetting)
        {
            this.OperatingSystemId = operatingSystemFieldSetting;
            this.Version = versionFieldSetting;
            this.ServicePack = servicePackSystemFieldSetting;
            this.RegistrationCode = registrationCodeSystemFieldSetting;
            this.ProductKey = productKeyFieldSetting;
        }

        [IsId]
        public int? OperatingSystemId { get; set; }

        public ConfigurableFieldModel<string> Version { get; set; }

        public ConfigurableFieldModel<string> ServicePack { get; set; }

        public ConfigurableFieldModel<string> RegistrationCode { get; set; }

        public ConfigurableFieldModel<string> ProductKey { get; set; }
    }
}