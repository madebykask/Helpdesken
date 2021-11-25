using System;
using System.Collections.Generic;
using System.Text;

namespace upKeeper2Helpdesk_New.entities
{
	public class LogicalDrive
	{
		public int DriveType { get; set; }
		public string DriveLetter { get; set; }
		public double TotalBytes { get; set; }
		public double FreeBytes { get; set; }
	}
}
