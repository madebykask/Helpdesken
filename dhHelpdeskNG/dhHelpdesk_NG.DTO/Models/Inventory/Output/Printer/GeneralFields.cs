namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    public class GeneralFields
    {
        public GeneralFields(string name, string manufacturer, string model, string serialNumber)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.SerialNumber = serialNumber;
        }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }
    }
}