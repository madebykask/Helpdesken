using System.Diagnostics;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using ExtendedCase.Common;
using Common.Logging;
using Common.Logging.ClientLogger;
using ExtendedCase.HelpdeskApiClient.Configuration;
using ExtendedCase.Logic.Services.Mappers;
using ExtendedCase.WebApi.Controllers;
using ExtendedCase.WebApi.Infrastructure.Configuration;
using ExtendedCase.WebApi.Infrastructure.Filters;

namespace ExtendedCase.WebApi.Di
{
    public class AutofacWebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //see registration rules here http://docs.autofac.org/en/latest/register/index.html

            RegisterHttpContext(builder);

            RegisterComponents(builder);

            //register autofac webapi filters
            RegisterAutofacFilters(builder);

            //register next module components
            RegisterChildrenComponents(builder);
        }

        private void RegisterHttpContext(ContainerBuilder builder)
        {
            builder.RegisterHttpRequestMessage(GlobalConfiguration.Configuration);
            builder.Register(c =>
              HttpContext.Current != null ?
                  new HttpContextWrapper(HttpContext.Current) :
                    c.Resolve<System.Net.Http.HttpRequestMessage>().Properties["MS_HttpContext"])
                .As<HttpContextBase>()
                .InstancePerRequest();
        }

        private static void RegisterComponents(ContainerBuilder builder)
        {
            //builder.RegisterType<Repo>().As<IRepo>().InstancePerRequest();
            builder.RegisterType<JsonSerializeService>().As<IJsonSerializeService>().SingleInstance();
            builder.RegisterType<ModelToEntitiesMapper>().As<IModelToEntitiesMapper>().SingleInstance();
            builder.RegisterType<EntitiesToModelMapper>().As<IEntitiesToModelMapper>().SingleInstance();

            builder.RegisterType<DefaultErrorFormatter>().As<IErrorMessageFormatter>().SingleInstance();
            builder.RegisterType<ClientLogMessageFormatter>().As<IClientLogMessageFormatter>().SingleInstance();
            builder.Register(c => new Log4NetLogger(c.Resolve<IErrorMessageFormatter>())).As<ILogger>().SingleInstance();
            builder.RegisterType<Log4NetClientLogger>().As<IClientLogger>().SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();

            builder.Register((c) => c.Resolve<IConfigurationProvider>().HelpdeskApiSettings)
                .As<IHelpdeskApiSettings>()
                .InstancePerDependency();
        }

        private static void RegisterAutofacFilters(ContainerBuilder builder)
        {
            //todo: specify actions which require token access
            builder.RegisterType<TokensInterceptionFilter>()
                   .AsWebApiActionFilterFor<ExtendedCaseApiControllerBase>()
                   .InstancePerRequest();
        }

        private static void RegisterChildrenComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacHelpdeskApiClientsModule>();
            builder.RegisterModule<AutofacLogicModule>();
            builder.RegisterModule<AutofacMapperModule>();
        }
    }
}