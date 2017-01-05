using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Areas.Invoices.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Invoices
{
	public class MvcInvoicesUrlName
	{
		public partial class Overview : MvcUrlNameBase.BaseNameHelper<OverviewController>
		{
			public static string Name => "Overview";

			public static string InvoiceExport
			{
				get { return GetAction(e => e.InvoiceExport(null)); }
			}
		}
	}
}