namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    public class GeneralFields
    {
        public GeneralFields(string name, string manufacturer, string model, string serial)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Serial = serial;
        }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Serial { get; set; }
    }
}