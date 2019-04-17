using DH.Helpdesk.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models
{
	public class FeatureToggleModel
	{
		public FeatureToggleTypes ToggleType { get; set; }
		public bool Active { get; set; }
		public string Description { get; set; }
		public DateTime ChangeDate { get; set; }
	}
}
