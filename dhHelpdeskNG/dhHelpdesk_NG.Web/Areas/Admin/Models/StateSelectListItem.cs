using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
	public class StateSelectListItem : SelectListItem
	{
		public bool Active { get; set; }
	}
}