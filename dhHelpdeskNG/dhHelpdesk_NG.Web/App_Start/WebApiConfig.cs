using System.Linq;
using System.Web.Http;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace DH.Helpdesk.Web
{
	public class WebApiConfig
	{
		public const string UrlPrefixRelative = "api";        

        public static void Register(HttpConfiguration config)
		{
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: WebApiUrlName.ActionApiRouteName,
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
                        
        }        
    }
}