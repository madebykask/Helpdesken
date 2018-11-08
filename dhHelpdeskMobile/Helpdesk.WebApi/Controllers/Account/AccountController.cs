using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Helpdesk.WebApi.Config.IdentityServer;
using Helpdesk.WebApi.Models.Account;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Helpdesk.WebApi.Controllers.Account
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        //private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly ILogger<AccountController> _logger;
        private readonly IPersistedGrantService _persistedGrantService;

        public AccountController(TestUserStore users, ILoggerFactory loggerFactory,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IPersistedGrantService persistedGrantService,
            //IAuthenticationSchemeProvider schemeProvider,
            IEventService events)
        {
            _users = users;
            _interaction = interaction;
            _clientStore = clientStore;
           // _schemeProvider = schemeProvider;
            _events = events;
            _persistedGrantService = persistedGrantService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login([FromBody]LoginInputModel model, [FromServices] ITokenService tokenService, [FromServices] IdentityServerOptions options)
        public async Task<IActionResult> Login(LoginInputModel model)
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
            if (ModelState.IsValid)
            {
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
                    }

                    ;

                    // issue authentication cookie with subject ID and username
                    await HttpContext.SignInAsync(user.SubjectId, user.Username, props);



                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect( model.ReturnUrl);


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
            }

            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            var subjectId = HttpContext.User.Identity.GetSubjectId();

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;
                try
                {
                    //await _signInManager.SignOutAsync();
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties { RedirectUri = url });
                }
                catch(NotSupportedException)
                {
                }
            }

            // delete authentication cookie
            await HttpContext.SignOutAsync();
            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            var vm = new LoggedOutViewModel
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                SignOutIframeUrl = logout?.SignOutIFrameUrl
            };

            await _persistedGrantService.RemoveAllGrantsAsync(subjectId, "js");

            return Redirect(Settings.HostUrl + @"\Home\Index");//TODO: change to client url
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                }
            }

            return new LoginViewModel
            {
                EnableLocalLogin = allowLocal,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint
            };
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }


    }
}