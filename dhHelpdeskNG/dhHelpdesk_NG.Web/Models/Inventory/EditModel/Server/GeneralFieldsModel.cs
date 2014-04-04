namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    public class GeneralFieldsModel
    {
        public GeneralFieldsModel(
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> manufacturer,
            ConfigurableFieldModel<string> description,
            ConfigurableFieldModel<string> model,
            ConfigurableFieldModel<string> serialNumber)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Description = description;
            this.Model = model;
            this.SerialNumber = serialNumber;
        }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        public ConfigurableFieldModel<string> Description { get; set; }

        public ConfigurableFieldModel<string> Model { get; set; }

        public ConfigurableFieldModel<string> SerialNumber { get; set; }
    }
}