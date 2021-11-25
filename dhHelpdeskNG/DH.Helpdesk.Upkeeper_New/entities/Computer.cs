using System;
using System.Collections.Generic;

namespace upKeeper2Helpdesk_New.entities
{
	public class Computer
	{
		public int Customer_Id { get; set; }
		public int? Computer_Id { get; set; }
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string Manufacturer { get; set; }
		public int? ComputerModel_Id { get; set; }
		public int? RAM_Id { get; set; }
		public int? Processor_Id { get; set; }
		public int? OS_Id { get; set; }
		public int? NIC_Id { get; set; }
		public string BIOSVersion { get; set; }
		public string HardwareId { get; set; }
		public string MACAddress { get; set; }
		public string Location2 { get; set; }

		public int? Domain_Id { get; set; }

		public DateTime ScanDate { get; set; }
		public DateTime? ScrapDate { get; set; }

		public User ClientInformation { get; set; }
		public List<Software> Software { get; set; }
		public List<Hotfix> Hotfix { get; set; }
		public List<LogicalDrive> LogicalDrives { get; set; }
	}
}
