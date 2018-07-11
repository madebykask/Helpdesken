using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Helpdesk.WebApi.Config.IdentityServer;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(TestUserStore users, IIdentityServerInteractionService interaction, IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider, IEventService events)
        {
            _users = users;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginInputModel model, [FromServices] ITokenService tokenService, [FromServices] IdentityServerOptions options)
        {

            //var request = new TokenCreationRequest();

            //var claimIdentity = new ClaimsIdentity(new GenericIdentity(model.Username, "jwt"));
            //claimIdentity.AddClaim(new Claim(ClaimTypes.Name, ""));
            //claimIdentity.AddClaim(new Claim("sub", ""));
            //claimIdentity.AddClaim(new Claim("auth_time", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString()));
            //request.Subject = new ClaimsPrincipal(claimIdentity);
            //request.IncludeAllIdentityClaims = true;
            //request.ValidatedRequest = new ValidatedRequest { Subject = request.Subject };
            //request.ValidatedRequest.SetClient(await _clientStore.FindClientByIdAsync("js"));
            //request.Resources = new Resources(Settings.GetIdentityResources(), Settings.GetApiResources());
            //request.ValidatedRequest.Options = options;
            ////request.ValidatedRequest.ClientClaims = //IdentityUser.AdditionalClaims;
            //var token = await tokenService.CreateAccessTokenAsync(request);
            //token.Issuer = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
            //var tokenValue = await tokenService.CreateSecurityTokenAsync(token);
            //return Ok(tokenValue);

            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (_users.ValidateCredentials(model.Username, model.Password))
            {
                var user = _users.FindByUsername(model.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));


                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (model.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                    };
                };

                // issue authentication cookie with subject ID and username
                await HttpContext.SignInAsync(user.SubjectId, user.Username, props);



                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Ok(new
                {
                    returnUrl = model.ReturnUrl
                });
            

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/");
                }

                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            return BadRequest();

        }

    }
}