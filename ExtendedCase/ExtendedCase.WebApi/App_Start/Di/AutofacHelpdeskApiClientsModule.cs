using Autofac;
using ExtendedCase.HelpdeskApiClient;
using ExtendedCase.HelpdeskApiClient.Interfaces;
using ExtendedCase.WebApi.Infrastructure.TokenProviders;

namespace ExtendedCase.WebApi.Di
{
    public class AutofacHelpdeskApiClientsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //token provider
            builder.RegisterType<HttpApiTokenProvider>()
                .As<IApiTokenProvider>()
                .InstancePerRequest();

            //api clients
            builder.RegisterType<HelpdeskApiClientsFactory>()
                .As<IHelpdeskApiClientsFactory>()
                .InstancePerRequest();

            builder.Register(c => c.Resolve<IHelpdeskApiClientsFactory>().CreateCaseApiClient())
                .As<IHelpdeskCaseApiClient>()
                .InstancePerDependency();
        }
    }
}