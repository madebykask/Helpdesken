namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Server
{
    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(string serverName, string manufacturer, string description, string computerModel, string serialNumber)
        {
            this.ServerName = serverName;
            this.Manufacturer = manufacturer;
            this.Description = description;
            this.ComputerModel = computerModel;
            this.SerialNumber = serialNumber;
        }

        public string ServerName { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public string ComputerModel { get; set; }

        public string SerialNumber { get; set; }
    }
}