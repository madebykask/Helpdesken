using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Owin;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace DH.Helpdesk.WebApi.Infrastructure.Authentication
{
    public class AccessTokenProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public AccessTokenProvider(string clientId)
        {
            _publicClientId = clientId;
        }
        
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string secret;

            if (!context.TryGetBasicCredentials(out clientId, out secret))
                context.TryGetFormCredentials(out clientId, out secret);
            if (clientId.Equals(_publicClientId,
                StringComparison.InvariantCultureIgnoreCase))
            {
                context.OwinContext.Set(ConfigApi.Constants.OwinContext.ClientId, clientId);
                context.Validated(clientId);
            }
            else
            {
                context.Rejected();
            }

            return Task.FromResult(0);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var requestScope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = requestScope != null ? requestScope.Resolve<IUserService>() : GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUserService)) as IUserService;

            if (userService == null)
            {
                context.SetError("Error!", "Please, try later.");
                context.Rejected();
                return;
            }

            var user = await userService.LoginAsync(context.UserName, context.Password);

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {
                //System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames - use this one if go to OAuth
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserId));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserGroupId.ToString()));
                //var userCustomerIds = userService.GetUserCustomersIds(user.Id);// if new customer is assigned to user, user will need to relgin
                //identity.AddClaim(new Claim(CustomClaimTypes.CustomerIds, string.Join(",", userCustomerIds)));

                // create metadata to pass on to refresh token provider
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { ConfigApi.Constants.OwinContext.ClientId, context.ClientId }
                });

                var ticket = new AuthenticationTicket(identity, props);//by default used MachineKeyDataProtector. use IAppBuilder.SetDataProtectionProvider to change it.
                context.Validated(ticket);
            }
            else
            {
                context.Rejected();
                context.SetError("Invalid user!", "The user name or password is incorrect.");
            }
        }

        public override Task GrantRefreshToken(
            OAuthGrantRefreshTokenContext context)

        {
            var originalClient = context.Ticket.Properties.Dictionary[ConfigApi.Constants.OwinContext.ClientId];
            var currentClient = context.OwinContext.Get<string>(ConfigApi.Constants.OwinContext.ClientId);

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return Task.FromResult(0);
            }

            // chance to change authentication ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            //newIdentity.AddClaim(new Claim("newClaim”, "refreshToken"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult(0);
        }
    }
}