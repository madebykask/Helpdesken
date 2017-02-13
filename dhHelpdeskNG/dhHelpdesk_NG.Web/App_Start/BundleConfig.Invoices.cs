using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Invoices;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Orders;

namespace DH.Helpdesk.Web
{
	public partial class BundleConfig
	{
		public partial struct ScriptNames
		{
			public struct Invoices
			{
				public const string Overview = ("~/bundles/invoices/overview");
				public const string Files = ("~/bundles/invoices/files");
			}
		}

		public static void RegisterInvoicesAreaBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle(ScriptNames.Invoices.Overview).Include(
						"~/Areas/" + MvcInvoicesUrlName.Name + "/Scripts/Pages/invoicesOverview.js"));
			bundles.Add(new ScriptBundle(ScriptNames.Invoices.Files).Include(
						"~/Areas/" + MvcInvoicesUrlName.Name + "/Scripts/Pages/invoiceFiles.js"));

		}
	}
}