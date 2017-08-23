using System.Web.Http;

namespace DH.Helpdesk.Web
{
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using DH.Helpdesk.Common.Logger;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Binders;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Infrastructure.Logger;

    using Microsoft.Practices.ServiceLocation;

    public class MvcApplication : HttpApplication
    {
        private readonly IConfiguration configuration = ManualDependencyResolver.Get<IConfiguration>();
        
        protected void Application_Start()
        {           
            AreaRegistration.RegisterAllAreas();
            //MARK: Remove old Api
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            ViewEngineInit();

			RegisterLocalizedAttributes();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //MARK: Remove old Api
            //FilterConfig.RegisterWebApiGlobalFilters(GlobalConfiguration.Configuration.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterBinders();
            ProcessStartupTasks();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			JsonFormatConfig.ConfigWebApi();
			JsonFormatConfig.ConfigMVC();

            ECT.FormLib.FormLibSetup.Setup();
            ECT.FormLib.FormLibSetup.SetupRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
            //System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;//uncomment this if XFrameOptions is added in web.config headers            
            // ECT.FormLib.FormLibSetup.Setup(); todo

            //#if DEBUG
            //    BundleTable.EnableOptimizations = false;
            //#else
            //    BundleTable.EnableOptimizations = true;
            //#endif
        }

        private void ViewEngineInit()
        {
            // Clear all registered view engines
            ViewEngines.Engines.Clear();

            // Add back in just the Razor view engine
            var viewEngine = new RazorViewEngine
            {
                ViewLocationCache = new DefaultViewLocationCache(TimeSpan.FromHours(24))
            };

            ViewEngines.Engines.Add(viewEngine);
        }

        protected void Application_PostAuthorizeRequest()
		{
            //MARK: Remove old Api
            //if (IsWebApiRequest())
            //{
            //	HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            //}
        }

        //MARK: Remove old Api
        //private bool IsWebApiRequest()
        //{
        //	return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Contains(@"/" + WebApiConfig.UrlPrefixRelative + @"/");
        //}        

        protected void Application_BeginRequest(object sender, EventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = this.configuration.Application.DefaultCulture;
		}

#if !DEBUG
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;

            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            var currentAction = " ";

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();

            var controller = new ErrorController(ManualDependencyResolver.Get<IMasterDataService>());
            var routeData = new RouteData();
            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;

                    // others if any
                    default:
                        action = "Index";
                        break;
                }
            }

            if (ex is BusinessLogicException)
            {
                action = "BusinessLogicError";
            }

            var workContext = ManualDependencyResolver.Get<IWorkContext>();
            var guid = Guid.NewGuid();
            LogManager.Error.Error(new ErrorContext(
                                        guid,
                                        ex,
                                        currentController,
                                        currentAction,
                                        httpContext,
                                        workContext).ToString());


            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new DH.Helpdesk.Web.Models.Error.HandleErrorInfoGuid(ex, currentController, currentAction, guid);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
#endif

		/// <summary>
		/// The register binders.
		/// </summary>
		private static void RegisterBinders()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
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