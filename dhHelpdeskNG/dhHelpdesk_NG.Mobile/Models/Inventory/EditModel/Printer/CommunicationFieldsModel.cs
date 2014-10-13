namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Printer
{
    public class CommunicationFieldsModel
    {
        public CommunicationFieldsModel()
        {
        }

        public CommunicationFieldsModel(
            ConfigurableFieldModel<string> networkAdapterName,
            ConfigurableFieldModel<string> ipAddress,
            ConfigurableFieldModel<string> macAddress)
        {
            this.NetworkAdapterName = networkAdapterName;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
        }

        public ConfigurableFieldModel<string> NetworkAdapterName { get; set; }

        public ConfigurableFieldModel<string> IPAddress { get; set; }

        public ConfigurableFieldModel<string> MacAddress { get; set; }
    }
}