using DH.Helpdesk.Common.Constants;
using System;

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
