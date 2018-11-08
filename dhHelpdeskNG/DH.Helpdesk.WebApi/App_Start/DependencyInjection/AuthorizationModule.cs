using Autofac;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Config;

namespace DH.Helpdesk.WebApi.DependencyInjection
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccessTokenProvider>()
                .WithParameter(new TypedParameter(typeof(string), ConfigApi.Constants.PublicClientId));
            builder.RegisterType<RefreshTokenProvider>();

        }
    }
}