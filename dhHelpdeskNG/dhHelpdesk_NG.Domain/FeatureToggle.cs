using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain
{
	public class FeatureToggle
	{
		public string StrongName { get; set; }
		public bool Active { get; set; }
		public string Description { get; set; }
		public DateTime ChangeDate { get; set; }
	}
}
