namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
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
            ConfigurableFieldModel<string> macAddress)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> NetworkAdapterId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> IPAddress { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> MacAddress { get; set; }
    }
}