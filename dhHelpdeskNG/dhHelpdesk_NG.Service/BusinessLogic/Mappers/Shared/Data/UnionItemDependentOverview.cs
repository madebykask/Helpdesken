using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Shared.Data
{
	internal class UnionItemDependentOverview : UnionItemOverview
	{
		public int? DependentId { get; set; }
	}
}
