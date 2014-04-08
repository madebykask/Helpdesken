namespace DH.Helpdesk.Web
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                // Route name
                "{controller}/{action}/{id}",
                new { area = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // No need to load all view engines
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RegisterLocalizedAttributes();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            var clientCulture = this.Request.UserLanguages.FirstOrDefault() ?? ApplicationDefaultParameters.Culture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(clientCulture);
        }

        private static void RegisterLocalizedAttributes()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(LocalizedRequiredAttribute),
                typeof(RequiredAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(LocalizedStringLengthAttribute),
                typeof(StringLengthAttributeAdapter));
        }
    }
}