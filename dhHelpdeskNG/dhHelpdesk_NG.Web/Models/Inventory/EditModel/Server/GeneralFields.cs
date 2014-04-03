namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    public class GeneralFields
    {
        public GeneralFields(
            ConfigurableFieldModel<string> serverName,
            ConfigurableFieldModel<string> manufacturer,
            ConfigurableFieldModel<string> description,
            ConfigurableFieldModel<string> computerModel,
            ConfigurableFieldModel<string> serialNumber)
        {
            this.ServerName = serverName;
            this.Manufacturer = manufacturer;
            this.Description = description;
            this.ComputerModel = computerModel;
            this.SerialNumber = serialNumber;
        }

        public ConfigurableFieldModel<string> ServerName { get; set; }

        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        public ConfigurableFieldModel<string> Description { get; set; }

        public ConfigurableFieldModel<string> ComputerModel { get; set; }

        public ConfigurableFieldModel<string> SerialNumber { get; set; }
    }
}