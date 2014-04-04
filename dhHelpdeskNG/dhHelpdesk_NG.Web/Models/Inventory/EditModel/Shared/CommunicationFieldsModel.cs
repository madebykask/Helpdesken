namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsModel
    {
        public CommunicationFieldsModel(
            int? networkAdapterId,
            ConfigurableFieldModel<string> ipAddress,
            ConfigurableFieldModel<string> macAddress)
        {
            this.NetworkAdapterId = networkAdapterId;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        [IsId]
        public int? NetworkAdapterId { get; set; }

        public ConfigurableFieldModel<string> IPAddress { get; set; }

        public ConfigurableFieldModel<string> MacAddress { get; set; }
    }
}