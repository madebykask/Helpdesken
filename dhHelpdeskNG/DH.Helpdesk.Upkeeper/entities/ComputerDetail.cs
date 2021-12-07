using System;

namespace upKeeper2Helpdesk.entities
{
	public class ComputerDetail
	{
		public Guid Id { get; set; }
		public Guid Uuid { get; set; }	
		public string SerialNumber { get; set; }
		public Guid? OrganizationId { get; set; }
		public Guid? HardwareId { get; set; }
		public Guid? PlatformId { get; set; }
		public string Name { get; set; }
		public string NewName { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public bool Deleted { get; set; }
		public byte ServiceOSType { get; set; }
		public string ClientVersion { get; set; }
		public string MACAddress { get; set; }
		public string IPAddress { get; set; }
		public string SubnetMask { get; set; }
		public string DefaultGateway { get; set; }
		public string PrimaryDNS { get; set; }
		public string SecondaryDNS { get; set; }
		public string UserName { get; set; }
		public string ServiceOSPath { get; set; }
		public DateTime InstallDate { get; set; }
		public DateTime StartInstallDate { get; set; }
		public DateTime? WarrantyEndDate { get; set; }
		public bool Disabled { get; set; }
		public string TeamViewerId { get; set; }
		public Guid? CategoryId { get; set; }
		public bool Locked { get; set; }
		public string Extra1 { get; set; }
		public string Extra2 { get; set; }
		public string Extra3 { get; set; }
		public string Extra4 { get; set; }
		public string Extra5 { get; set; }
	}
}
