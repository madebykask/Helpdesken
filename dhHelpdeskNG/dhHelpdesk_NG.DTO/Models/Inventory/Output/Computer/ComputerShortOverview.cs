namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class ComputerShortOverview
    {
        public ComputerShortOverview(
            int id,
            string computerName,
            string manufacturer,
            string computerModelName,
            string serialNumber,
            string biosVersion,
            DateTime? biosDate,
            string operatingSystemName,
            string servicePack,
            string proccesorName,
            string ramName,
            string networkAdapterName,
            string ipAddress,
            string macAddress,
            bool ras,
            string info)
        {
            this.Id = id;
            this.ComputerName = computerName;
            this.Manufacturer = manufacturer;
            this.ComputerModelName = computerModelName;
            this.SerialNumber = serialNumber;
            this.BIOSVersion = biosVersion;
            this.BIOSDate = biosDate;
            this.OperatingSystemName = operatingSystemName;
            this.ServicePack = servicePack;
            this.ProccesorName = proccesorName;
            this.RAMName = ramName;
            this.NetworkAdapterName = networkAdapterName;
            this.IPAddress = ipAddress;
            this.MacAddress = macAddress;
            this.RAS = ras;
            this.Info = info;
        }

        public int Id { get; private set; }

        public string ComputerName { get; private set; }

        public string Manufacturer { get; private set; }

        public string ComputerModelName { get; private set; }

        public string SerialNumber { get; private set; }

        public string BIOSVersion { get; private set; }

        public DateTime? BIOSDate { get; private set; }

        public string OperatingSystemName { get; private set; }

        public string ServicePack { get; private set; }

        public string ProccesorName { get; private set; }

        public string RAMName { get; private set; }

        public string NetworkAdapterName { get; private set; }

        public string IPAddress { get; private set; }

        public string MacAddress { get; private set; }

        public bool RAS { get; private set; }

        public string Info { get; private set; }
    }
}