namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsModel
    {
        public OperatingSystemFieldsModel(
            int? operatingSystemId,
            ConfigurableFieldModel<string> version,
            ConfigurableFieldModel<string> servicePackSystem,
            ConfigurableFieldModel<string> registrationCodeSystem,
            ConfigurableFieldModel<string> productKey)
        {
            this.OperatingSystemId = operatingSystemId;
            this.Version = version;
            this.ServicePack = servicePackSystem;
            this.RegistrationCode = registrationCodeSystem;
            this.ProductKey = productKey;
        }

        [IsId]
        public int? OperatingSystemId { get; set; }

        public ConfigurableFieldModel<string> Version { get; set; }

        public ConfigurableFieldModel<string> ServicePack { get; set; }

        public ConfigurableFieldModel<string> RegistrationCode { get; set; }

        public ConfigurableFieldModel<string> ProductKey { get; set; }
    }
}