using System.Web.Http;
using System.Web.Mvc;

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
		}

		private void ConfigApiRoutes(AreaRegistrationContext context)
		{
			context.Routes.MapHttpRoute(
				name: "OrdersApiAction",
				routeTemplate: AreaName + "/api/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "OrdersApi",
				routeTemplate: AreaName + "/api/{controller}"
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
