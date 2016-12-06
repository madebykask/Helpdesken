using System.Web.Http;

namespace DH.Helpdesk.Web.Areas.Reports
{
    using System.Web.Mvc;

    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
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
				name: "ReportsApiAction",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "ReportsApi",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
				);
		}

		private void ConfigMvcRoutes(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Reports_default",
				 AreaName + "/{controller}/{action}/{id}",
				new { area = AreaName, action = "Index", id = UrlParameter.Optional }
			);
		}

    }
}
