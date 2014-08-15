namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    public class GeneralFields
    {
        public GeneralFields(string name, string manufacturer, string description, string model, string serialNumber)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Description = description;
            this.Model = model;
            this.SerialNumber = serialNumber;
        }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }

        public static GeneralFields CreateDefault()
        {
            return new GeneralFields(null, null, null, null, null);
        }
    }
}