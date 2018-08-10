using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection;

namespace DH.Helpdesk.WebApi
{
    public static partial class WebApiConfig
    {
        public static IContainer ConfigDiContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            RegisterServices(builder);

            var container = builder.Build();
            return container;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            
            builder.RegisterModule(new AuthorizationModule());
            builder.RegisterModule(new LoggerModule());
            builder.RegisterModule(new WorkContextModule());
            builder.RegisterModule(new CommonModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new ServicesModule());
        }
    }
}