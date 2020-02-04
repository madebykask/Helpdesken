using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DH.Helpdesk.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional });

			routes.MapRoute(
				"CaseFiles",
				"cases/casefiles/{caseId}/{filename}",
				new { controller = "Cases", action = "CaseFiles", caseId = UrlParameter.Optional, filename = UrlParameter.Optional });

			routes.MapRoute(
				"CaseLogFiles",
				"cases/caselogfiles/{type}/{logId}/{filename}",
				new { controller = "Cases", action = "CaseLogFiles", type = UrlParameter.Optional, logId = UrlParameter.Optional, filename = UrlParameter.Optional });
		}
	}
}