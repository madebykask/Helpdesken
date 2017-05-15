//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using DH.Helpdesk.Web.Areas.Invoices.Controllers.WebApi;

//namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Invoices
//{
//	public partial class WebApiInvoicesUrlName
//	{
//		public static string Name = "Invoices";

//		public class Invoices : WebApiUrlNameBase.BaseNameHelper<InvoicesApiController>
//		{
//			public static string GetInvoicesOverviewList
//			{
//				get { return GetAction(e => e.GetInvoicesOverviewList(null)); }
//			}

//			public static string SaveInvoiceValues
//			{
//				get { return GetAction(e => e.SaveInvoiceValues(null)); }
//			}

//			public static string InvoiceAction
//			{
//				get { return GetAction(e => e.InvoiceAction(null)); }
//			}
//		}
//	}
//}