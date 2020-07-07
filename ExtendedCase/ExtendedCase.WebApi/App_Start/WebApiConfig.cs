using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Logging;
using ExtendedCase.Logic;
using ExtendedCase.Logic.Di;
using ExtendedCase.WebApi.Di;
using ExtendedCase.WebApi.ExceptionHandling;
using ExtendedCase.WebApi.Infrastructure.Cors;

namespace ExtendedCase.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Cors
            SetCors(config);

            // Web API configuration and services
            RegisterDiContainer(config);

            // Web API routes
            RegisterRoutes(config);

            //Formatters
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Services
            RegisterServices(config);

            //Filters
            RegisterFilters(config);
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void RegisterDiContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            //Register app components
            builder.RegisterModule<AutofacWebModule>();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(HttpConfiguration config)
        {
            var logger = config.DependencyResolver.GetService(typeof(ILogger)) as ILogger;
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger(logger));
            config.Services.Replace(typeof(IExceptionHandler), new GuidExceptionHandler(logger));
        }

        private static void RegisterFilters(HttpConfiguration config)
        {
        }

        private static void SetCors(HttpConfiguration config)
        {
            //var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.SetCorsPolicyProviderFactory(new WebApiCorsPolicyProviderFactory());
            config.EnableCors();
        }
    }
}