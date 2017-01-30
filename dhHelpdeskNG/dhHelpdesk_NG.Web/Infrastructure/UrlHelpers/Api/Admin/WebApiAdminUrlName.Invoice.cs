using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Areas.Admin.Controllers.Api;

namespace DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Admin
{
	public partial class WebApiAdminUrlName
	{
		public static string Name = "Admin";

		public class Invoice : WebApiUrlNameBase.BaseNameHelper<InvoiceApiController>
		{
			public static string GetArticleProductAreaList
			{
				get { return GetAction(e => e.GetArticleProductAreaList(null)); }
			}
		}
	}
}