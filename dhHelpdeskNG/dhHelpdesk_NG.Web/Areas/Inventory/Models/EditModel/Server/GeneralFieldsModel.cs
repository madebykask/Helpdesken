namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsModel
    {
        public GeneralFieldsModel()
        {
        }

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

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Description { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Model { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> SerialNumber { get; set; }
    }
}