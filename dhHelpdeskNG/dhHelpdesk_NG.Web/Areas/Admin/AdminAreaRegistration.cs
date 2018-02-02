using System.Web.Http;

namespace DH.Helpdesk.Web.Areas.Admin
{
	using System.Web.Mvc;

	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "admin";
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
				name: "AdminApiAction",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "AdminApi",
				routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
				);
		}

		private void ConfigMvcRoutes(AreaRegistrationContext context)
		{
            context.MapRoute(
                "Admin_default",
                AreaName + "/{controller}/{action}/{id}",
                new { area = AreaName, action = "Index", id = UrlParameter.Optional });
        }
	}
}
