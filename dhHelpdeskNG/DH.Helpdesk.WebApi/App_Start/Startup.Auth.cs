using System;
using System.Web.Http.Dependencies;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace DH.Helpdesk.WebApi
{
    public partial class Startup
    {
        private static void ConfigureAuth(IAppBuilder app, IDependencyResolver resolver)
        {
            //using simple bearer token because over https it provides the same security as cookies. if no https is used tokens should be encrypted.
            //for better security we should use dedicated openid/oauth2 middleware (for example identityServer4)
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(ConfigApi.Constants.TokenEndPoint),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),//TODO: Move to config
                ApplicationCanDisplayErrors = true,
                Provider = resolver.GetService(typeof(AccessTokenProvider)) as IOAuthAuthorizationServerProvider, //new AccessTokenProvider(ConfigApi.Constants.PublicClientId),
                RefreshTokenProvider = resolver.GetService(typeof(RefreshTokenProvider)) as IAuthenticationTokenProvider
            };
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            app.UseOAuthBearerAuthentication
            (
                new OAuthBearerAuthenticationOptions
                {
                    Provider = new OAuthBearerAuthenticationProvider(),
                    //AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AuthenticationType = OAuthDefaults.AuthenticationType
                }
            );

        }
    }


}