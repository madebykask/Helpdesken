namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsModel
    {
        public CommunicationFieldsModel()
        {
        }

        public CommunicationFieldsModel(
            ConfigurableFieldModel<int?> networkAdapterId,
            ConfigurableFieldModel<string> ipAddress,
            ConfigurableFieldModel<string> macAddress,           
            ConfigurableFieldModel<bool> isRas,
            ConfigurableFieldModel<string> novellClient)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.IsRAS = isRas;
            this.NovellClient = novellClient;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> NetworkAdapterId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> IPAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> MacAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> IsRAS { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> NovellClient { get; set; }
    }
}