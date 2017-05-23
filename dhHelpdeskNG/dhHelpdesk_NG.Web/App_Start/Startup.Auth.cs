using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using DH.Helpdesk.Dal.Repositories;
using Microsoft.Owin.Security.Infrastructure;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;

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
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                ApplicationCanDisplayErrors = true,
                Provider = new AccessTokenProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
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

    public class AccessTokenProvider : OAuthAuthorizationServerProvider
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

    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {                        
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(8));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
            if (context.Ticket != null &&
                context.Ticket.Properties.ExpiresUtc.HasValue &&
                context.Ticket.Properties.ExpiresUtc.Value.LocalDateTime < DateTime.Now)
            {            
                context.OwinContext.Set("custom.ExpriredToken", true);
            }
        }
    }


    public class AuthorizeAttributeExtended : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var tokenHasExpired = false;
            var owinContext = OwinHttpRequestMessageExtensions.GetOwinContext(actionContext.Request);            
            if (owinContext != null)
            {
                tokenHasExpired = owinContext.Environment.ContainsKey("oauth.token_expired");
            }

            if (tokenHasExpired)
            {
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        error = "invalid_token",
                        error_message = "The Token has expired"
                    });
            }
            else
            {                
                actionContext.Response = new AuthenticationFailureMessage("unauthorized", actionContext.Request,
                    new
                    {
                        
                        error = "invalid_request",
                        error_message = "The Token is invalid"
                    });
            }
        }
    }

    public class AuthenticationFailureMessage : HttpResponseMessage
    {
        public AuthenticationFailureMessage(string reasonPhrase, HttpRequestMessage request, object responseMessage)
            : base(HttpStatusCode.Unauthorized)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

            Content = new ObjectContent<object>(responseMessage, jsonFormatter);            
            RequestMessage = request;
            ReasonPhrase = reasonPhrase;            
        }
    }

 
}