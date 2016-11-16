using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Shared
{
	public class ItemDependentOverview : ItemOverview
	{
		public ItemDependentOverview(string name, string value, string dependentId = null) : base(name, value)
		{
			DependentId = dependentId;
		}

		public string DependentId { get; set; }
	}
}
