using System.Web.Http;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.OrderAccounts
{
    public class OrderAccountsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OrderAccounts";
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
				name: "OrderAccountsApiAction",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "OrderAccountsApi",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
				);
		}

		private void ConfigMvcRoutes(AreaRegistrationContext context)
		{
			context.MapRoute(
				"OrderAccounts_default",
				 AreaName + "/{controller}/{action}/{id}",
				new { area = AreaName, action = "Index", id = UrlParameter.Optional }
			);
		}

    }
}
