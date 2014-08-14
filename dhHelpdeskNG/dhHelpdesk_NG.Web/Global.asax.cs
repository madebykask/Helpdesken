namespace DH.Helpdesk.Web
{
    using System;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.Common.Logger;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Binders;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Infrastructure.Logger;

    using Microsoft.Practices.ServiceLocation;

    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IConfiguration configuration = ManualDependencyResolver.Get<IConfiguration>();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
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

        /// <summary>
        /// The register binders.
        /// </summary>
        private static void RegisterBinders()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
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
            RegisterBinders();
            ProcessStartupTasks();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = this.configuration.Application.DefaultCulture;
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

        /// <summary>
        /// process startup tasks.
        /// </summary>
        private static void ProcessStartupTasks()
        {
            ILoggerService errorLoggerService = LogManager.Error;
            foreach (var task in ServiceLocator.Current.GetAllInstances<IStartUpTask>())
            {
                try
                {
                    if (task != null && task.IsEnabled)
                    {
                        task.Configure();
                    }
                }
                catch (Exception ex)
                {
                    errorLoggerService.Error(ex);
                }
            }
        }
    }
}