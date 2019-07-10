using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.WebApi.DependencyInjection;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Infrastructure.Owin;
using Microsoft.Owin;

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

            RegisterFilters(builder);

            RegisterMiddleWareComponents(builder);

            var container = builder.Build();
            return container;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterModule(new MapperModule());
            builder.RegisterModule(new AuthorizationModule());
            builder.RegisterModule(new LoggerModule());
            builder.RegisterModule(new WorkContextModule());
            builder.RegisterModule(new CommonModule());
            builder.RegisterModule(new ChangesModule());
            builder.RegisterModule(new MappersModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new ServicesModule());
        }

        private static void RegisterFilters(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerAccessAuthorizationFilter>()
                .AsWebApiAuthorizationFilterFor<BaseApiController>()
                .InstancePerRequest();
            
            builder.RegisterType<UserCasePermissionsFilter>()
                .AsWebApiAuthorizationFilterFor<BaseApiController>()
                .InstancePerRequest();
        }

        public static void RegisterMiddleWareComponents(ContainerBuilder builder)
        {
            builder.RegisterType<GlobalExceptionMiddleware>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<LogRequestMiddleware>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<RequestMiddleware>()
                .AsSelf()
                .InstancePerRequest();
        }
    }
}