namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperatingSystemFieldsModel
    {
        public OperatingSystemFieldsModel()
        {
        }

        public OperatingSystemFieldsModel(
            ConfigurableFieldModel<int?> operatingSystemId,
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

        [NotNull]
        public ConfigurableFieldModel<int?> OperatingSystemId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Version { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ServicePack { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RegistrationCode { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ProductKey { get; set; }
    }
}