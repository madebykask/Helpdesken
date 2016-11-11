using System.Web.Http;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Inventory
{
    public class InventoryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Inventory";
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
				name: "InventoryApiAction",
				routeTemplate: AreaName + "/api/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "InventoryApi",
				routeTemplate: AreaName + "/api/{controller}"
				);
		}

		private void ConfigMvcRoutes(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Inventory_default",
				 AreaName + "/{controller}/{action}/{id}",
				new { area = AreaName, action = "Index", id = UrlParameter.Optional }
			);
		}
    }
}
