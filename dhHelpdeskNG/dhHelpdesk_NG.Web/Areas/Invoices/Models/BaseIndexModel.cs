using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public abstract class BaseIndexModel
	{
		public abstract IndexModelType Type { get; }

		public bool ShowFiles { get; set; }
	}
}