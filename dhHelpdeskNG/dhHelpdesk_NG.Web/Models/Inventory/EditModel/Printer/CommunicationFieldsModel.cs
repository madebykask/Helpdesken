namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsModel
    {
        public CommunicationFieldsModel(
            ConfigurableFieldModel<string> networkAdapterName,
            ConfigurableFieldModel<string> ipAddress,
            ConfigurableFieldModel<string> macAddress)
        {
            this.NetworkAdapterName = networkAdapterName;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        [IsId]
        public ConfigurableFieldModel<string> NetworkAdapterName { get; set; }

        public ConfigurableFieldModel<string> IPAddress { get; set; }

        public ConfigurableFieldModel<string> MacAddress { get; set; }
    }
}