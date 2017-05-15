using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Areas.Invoices.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Invoices
{
	public partial class MvcInvoicesUrlName
	{
		public partial class Overview : MvcUrlNameBase.BaseNameHelper<OverviewController>
		{
			public static string Index
			{
				get { return GetAction(e => e.Index()); }
			}

			public static string Files
			{
				get { return GetAction(e => e.Files()); }
			}

			public static string InvoiceExport
			{
				get { return GetAction(e => e.InvoiceExport(null)); }
			}

			public static string InvoiceFile
			{
				get { return GetAction(e => e.InvoiceFile(Guid.Empty)); }
			}

		    public static string GetInvoicesOverviewList
		    {
		        get { return GetAction(e => e.GetInvoicesOverviewList(null)); }
		    }

		    public static string SaveInvoiceValues
		    {
		        get { return GetAction(e => e.SaveInvoiceValues(null)); }
		    }

		    public static string InvoiceAction
		    {
		        get { return GetAction(e => e.InvoiceAction(null)); }
		    }
        }
	}
}