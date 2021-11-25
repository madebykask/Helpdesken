using System;
using System.Collections.Generic;
using System.Text;

namespace upKeeper2Helpdesk_New.entities
{
	public class Hardware
	{
		public List<ComputerProperty> Properties { get; set; }
		public List<ComputerProperty> Disks { get; set; }
		public List<ComputerProperty> NetworkAdapters { get; set; }
	}
}
