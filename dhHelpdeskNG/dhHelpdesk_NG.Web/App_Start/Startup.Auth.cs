using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using DH.Helpdesk.Dal.Repositories;

namespace DH.Helpdesk.Web.App_Start
{    

    public partial class Startup
    {        

        public void ConfigureAuth(IAppBuilder app)
        {            
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                ApplicationCanDisplayErrors = true,
                Provider = new SimpleAuthorizationServerProvider()
            };
            
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.UseOAuthBearerAuthentication
            (
                new OAuthBearerAuthenticationOptions
                {
                    Provider = new OAuthBearerAuthenticationProvider()
                }
            );

        }
    }

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {           
            context.Validated();            
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var _userService = Services.Infrastructure.ManualDependencyResolver.Get<IUserRepository>();

            var _user = await _userService.GetByUserIdAsync(context.UserName, context.Password);

            if (_user != null && !string.IsNullOrEmpty(_user.UserId))
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Sid, _user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, _user.UserGroupId.ToString()));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid user!");
                context.Rejected();
            }

        }
    }

}