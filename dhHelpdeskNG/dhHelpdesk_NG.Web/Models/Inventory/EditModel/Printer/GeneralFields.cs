namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    public class GeneralFields
    {
        public GeneralFields(
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> manufacturer,
            ConfigurableFieldModel<string> model,
            ConfigurableFieldModel<string> serial)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Serial = serial;
        }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        public ConfigurableFieldModel<string> Model { get; set; }

        public ConfigurableFieldModel<string> Serial { get; set; }
    }
}