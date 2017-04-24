using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Orders;

namespace DH.Helpdesk.Web.Areas.Orders
{
    public class CaseAreaRegistration : AreaRegistration
    {
        public override string AreaName => "WebApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            ConfigApiRoutes(context);                        
        }

        private void ConfigApiRoutes(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "CaseApiAction",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                name: "CaseApi",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
                );
        }       

    }
}
