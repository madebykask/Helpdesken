﻿namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class WorkstationFields
    {
        public WorkstationFields(
            string computerName,
            string manufacturer,
            string computerModelName,
            string serialNumber,
            string biosVersionFieldSetting,
            DateTime? biosDate,
            string theftmarkFieldSetting,
            string carePackNumber,
            string computerTypeName,
            string locationFieldSetting)
        {
            this.ComputerName = computerName;
            this.Manufacturer = manufacturer;
            this.ComputerModelName = computerModelName;
            this.SerialNumber = serialNumber;
            this.BIOSVersion = biosVersionFieldSetting;
            this.BIOSDate = biosDate;
            this.Theftmark = theftmarkFieldSetting;
            this.CarePackNumber = carePackNumber;
            this.ComputerTypeName = computerTypeName;
            this.Location = locationFieldSetting;
        }

        public string ComputerName { get; set; }

        public string Manufacturer { get; set; }

        public string ComputerModelName { get; set; }

        public string SerialNumber { get; set; }

        public string BIOSVersion { get; set; }

        public DateTime? BIOSDate { get; set; }

        public string Theftmark { get; set; }

        public string CarePackNumber { get; set; }

        public string ComputerTypeName { get; set; }

        public string Location { get; set; }
    }
}