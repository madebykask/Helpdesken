using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DH.Helpdesk.Web
{
	public class WebApiConfig
	{
		public const string UrlPrefixRelative = "api";
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			// Configure Web API to use only bearer token authentication.
			//config.SuppressDefaultHostAuthentication();
			//config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: UrlPrefixRelative +"/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			//remove xml support, only json needed.
			config.Formatters.Remove(config.Formatters.XmlFormatter);
		}
	}
}