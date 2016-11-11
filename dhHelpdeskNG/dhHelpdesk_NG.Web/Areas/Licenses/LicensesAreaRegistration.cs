using System.Web.Http;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Licenses
{
    public class LicensesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Licenses";
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
				name: "LicensesApiAction",
				routeTemplate: AreaName + "/api/{controller}/{action}"
				);

			context.Routes.MapHttpRoute(
				name: "LicensesApi",
				routeTemplate: AreaName + "/api/{controller}"
				);
		}

		private void ConfigMvcRoutes(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Licenses_default",
				 AreaName + "/{controller}/{action}/{id}",
				new { area = AreaName, action = "Index", id = UrlParameter.Optional });
		}
    }
}
