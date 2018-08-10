using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;
using Microsoft.Owin.Security.OAuth;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
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