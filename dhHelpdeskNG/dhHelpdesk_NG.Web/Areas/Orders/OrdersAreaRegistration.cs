using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Orders;

namespace DH.Helpdesk.Web.Areas.Orders
{
    public class OrdersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Orders";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            ConfigApiRoutes(context);

            ConfigMvcRoutes(context);

            BundleConfig.RegisterOrdersAreaBundles(BundleTable.Bundles);
        }

        private void ConfigApiRoutes(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "OrdersApiAction",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                name: "OrdersApi",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
                );
        }

        private void ConfigMvcRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Orders_default",
                 AreaName + "/{controller}/{action}/{id}",
                new { area = AreaName, action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}
