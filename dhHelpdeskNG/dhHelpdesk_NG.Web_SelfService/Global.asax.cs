using System;
using System.Diagnostics;
using System.IdentityModel.Services;
using System.Web;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using DH.Helpdesk.Services.Infrastructure;

namespace DH.Helpdesk.SelfService
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // No need to load all view engines
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
           
            ECT.FormLib.FormLibSetup.Setup();
            ECT.FormLib.FormLibSetup.SetupRoutes(RouteTable.Routes);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var loginMode = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
            if (loginMode.Equals(LoginMode.SSO, StringComparison.OrdinalIgnoreCase))
            {
                FederatedAuthenticationConfiguration.Configure();
            }
        }

        #region Session Timeout Handling

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            const string SessionIdKey = "_sessionId";
            var hasSessionRestarted = false;

            var session = Context.Session;
            if (session != null)
            {
                if (session.IsNewSession)
                {
                    var sessionIdInCookie = Request.Cookies[SessionIdKey]?.Value;
                    var sessionIdState = session[SessionIdKey]?.ToString();
                    
                    if (!string.IsNullOrEmpty(sessionIdInCookie) && !sessionIdInCookie.Equals(sessionIdState, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSessionRestarted = true;
                    }
                        
                    //set new sessionId value to keep them in sync
                    var sessionGuid = Guid.NewGuid();
                    new HttpContextWrapper(Context).SetSessionCookie(SessionIdKey, sessionGuid.ToString());
                    session[SessionIdKey] = sessionGuid.ToString();
                }

                var logoutOnSessionTimeout = ManualDependencyResolver.Get<IFederatedAuthenticationSettings>()?.LogoutCustomerOnSessionExpire ?? false;
                if (logoutOnSessionTimeout && IsSsoMode())
                {
                    // signout if session/app has been restarted but user still authenticated!
                    var isAuthenticated = Context.User?.Identity?.IsAuthenticated ?? false;
                    if (hasSessionRestarted && isAuthenticated)
                    {
                        var fedAuthService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();
                        var returnUrl = BuildHomeUrl(Context);
                        fedAuthService?.SignOut(returnUrl);
                    }
                }
            }
        }

        #endregion

        #region SSO token lifetime handling

        protected void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs args)
        {
            var token = args.SessionToken;
            Trace.WriteLine($"SessionAuthenticationModule_SessionSecurityTokenReceived called: {token.ValidFrom} - {token.ValidTo}");

            var configuration = ManualDependencyResolver.Get<IFederatedAuthenticationSettings>();
            var federationAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();

            //check if token has expired:
            if (token.ValidTo < DateTime.UtcNow)
            {
                Trace.WriteLine($"Security token lifetime has been expired: ValidTo: {token.ValidTo}, UtcNow: {DateTime.UtcNow}. Siging out.");
                //FederatedAuthentication.WSFederationAuthenticationModule.SignOut();

                //get redirect url after sign in. On Post we need to take original url from UrlReferrer
                //var returnUrl = Request.Url;
                //if (Request.HttpMethod == "POST" && Request.UrlReferrer != null)
                //{
                //    returnUrl = Request.UrlReferrer;
                //}


                var returnUrl = BuildHomeUrl(Context);
                federationAuthenticationService.SignOut(returnUrl);

                return;
            }

            if (configuration.EnableSlidingExpiration)
            {
                var sam = (SessionAuthenticationModule) sender;
                var refreshedToken = 
                    federationAuthenticationService.RefreshSecurityTokenLifeTime(sam, args.SessionToken, configuration.SecurityTokenMaxDuration);

                if (refreshedToken != null)
                {
                    args.SessionToken = refreshedToken;
                    args.ReissueCookie = true;
                }
            }
        }

        #endregion

        #region Helper Methods

        private static string BuildHomeUrl(HttpContext ctx)
        {
            var httpContext = new HttpContextWrapper(ctx);
            var customerId = httpContext.GetCustomerIdFromCookie();
            var returnUrl = ctx.Request.BuildHomeUrl(customerId);
            return returnUrl;
        }

        private static bool IsSsoMode()
        {
            var loginMode = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
            return LoginMode.SSO.Equals(loginMode, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}