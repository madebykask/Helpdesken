namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class WorkstationFields
    {
        public WorkstationFields(
            string computerName,
            string manufacturer,
            int? computerModelId,
            string serialNumber,
            string biosVersionFieldSetting,
            string biosDate,
            string theftmarkFieldSetting,
            string carePackNumber,
            int? computerTypeId,
            string locationFieldSetting)
        {
            this.ComputerName = computerName;
            this.Manufacturer = manufacturer;
            this.ComputerModelId = computerModelId;
            this.SerialNumber = serialNumber;
            this.BIOSVersion = biosVersionFieldSetting;
            this.BIOSDate = biosDate;
            this.Theftmark = theftmarkFieldSetting;
            this.CarePackNumber = carePackNumber;
            this.ComputerTypeId = computerTypeId;
            this.Location = locationFieldSetting;
        }

        public string ComputerName { get; set; }

        public string Manufacturer { get; set; }

        public int? ComputerModelId { get; set; }

        public string SerialNumber { get; set; }

        public string BIOSVersion { get; set; }

        public string BIOSDate { get; set; }

        public string Theftmark { get; set; }

        public string CarePackNumber { get; set; }

        public int? ComputerTypeId { get; set; }

        public string Location { get; set; }
    }
}