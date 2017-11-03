using System;
using System.IdentityModel.Services;
using System.IO;
using System.Web;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using DH.Helpdesk.Services.Infrastructure;
using log4net;
using log4net.Config;

namespace DH.Helpdesk.SelfService
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog _logger;
        const string SessionIdKey = "_sessionId";

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
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            InitLogging();

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

        #region Logging

        private void InitLogging()
        {
            var filePath = Server.MapPath("~/log4net.config");
            var configFile = new FileInfo(filePath);
            if (configFile.Exists)
            {
                XmlConfigurator.Configure(configFile);
                try
                {
                    var ssoLogger = LogManager.GetLogger("sso");
                    SsoLogger.SetLoggerInstance(ssoLogger);
                }
                catch
                {
                }
            }
        }

        #endregion

        protected void WSFederationAuthenticationModule_SignedIn(object sender, EventArgs e)
        {
            SsoLogger.Debug("WSFederationAuthenticationModule: SignedIn!", Context);
        }

        #region Session Timeout Handling

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var hasSessionRestarted = false;

            var session = Context.Session;
            if (session != null)
            {
                SsoLogger.Debug("Application_AcquireRequestState called.", Context);

                if (session.IsNewSession)
                {
                    var sessionIdInCookie = Request.Cookies[SessionIdKey]?.Value;
                    var sessionIdState = session[SessionIdKey]?.ToString();

                    SsoLogger.Debug(
                        $"New session! SessionIdInCookie:{sessionIdInCookie}, SessionIdInState: {sessionIdState}.",
                        Context);

                    if (!string.IsNullOrEmpty(sessionIdInCookie) && !sessionIdInCookie.Equals(sessionIdState, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSessionRestarted = true;
                        SsoLogger.Debug("Session has been restarted!", Context);
                    }
                }
                
                //set new sessionId value to keep them in sync
                RefreshSessionId();

                var logoutOnSessionTimeout =
                    ManualDependencyResolver.Get<IFederatedAuthenticationSettings>()?.LogoutCustomerOnSessionExpire ?? false;

                if (logoutOnSessionTimeout && IsSsoMode())
                {
                    //read and reset signin flag
                    // signout if session/app has been restarted but user still authenticated!
                    var isAuthenticated = Context.User?.Identity?.IsAuthenticated ?? false;
                    if (hasSessionRestarted && isAuthenticated)
                    {
                        var fedAuthService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();
                        var returnUrl = BuildHomeUrl(Context);

                        SsoLogger.Debug($"Sign out user due user session restart! ReturnUrl: {returnUrl}.", Context);
                        
                        //clear sessionId sync so that the next time session time out logic was ignored
                        ClearSessionId(); 

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
            SsoLogger.Debug(
                $"SessionAuthenticationModule_SessionSecurityTokenReceived called: {token.ValidFrom} - {token.ValidTo}.",
                Context);

            var configuration = ManualDependencyResolver.Get<IFederatedAuthenticationSettings>();
            var federationAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();

            //check if token has expired:
            if (token.ValidTo < DateTime.UtcNow)
            {
                //get redirect url after sign in. On Post we need to take original url from UrlReferrer
                //var returnUrl = Request.Url;
                //if (Request.HttpMethod == "POST" && Request.UrlReferrer != null)
                //{
                //    returnUrl = Request.UrlReferrer;
                //}


                var returnUrl = BuildHomeUrl(Context);

                SsoLogger.Debug(
                    $"Security token lifetime has been expired: ValidTo: {token.ValidTo}, UtcNow: {DateTime.UtcNow}. Signing out. ReturnUrl: {returnUrl}.",
                    Context);
                federationAuthenticationService.SignOut(returnUrl);

                return;
            }

            if (configuration.EnableSlidingExpiration)
            {
                var sam = (SessionAuthenticationModule) sender;
                var refreshedToken =
                    federationAuthenticationService.RefreshSecurityTokenLifeTime(sam, args.SessionToken,
                        configuration.SecurityTokenMaxDuration);

                if (refreshedToken != null)
                {
                    args.SessionToken = refreshedToken;
                    args.ReissueCookie = true;
                }
            }
        }

        #endregion

        #if DEBUG

        protected void Session_OnEnd(object sender, EventArgs e)
        {
            SsoLogger.Debug("Session_OnEnd called!");
        }

        protected void Application_OnEnd(object sender, EventArgs e)
        {
            SsoLogger.Debug("Application_OnEnd called!");
        }

        protected void SessionAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs e)
        {
            SsoLogger.Debug("SessionAuthenticationModule_SessionSecurityTokenCreated called!",Context);
        }

        protected void WSFederationAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs e)
        {
            SsoLogger.Debug("WSFederationAuthenticationModule: SessionSecurityTokenCreated!", Context);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            SsoLogger.Debug("Application_PostAuthenticate called.", Context);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            SsoLogger.Debug("Application_EndRequest called.", Context);
        }

        #endif

        #region Helper Methods

        private static string BuildHomeUrl(HttpContext ctx)
        {
            var httpContext = new HttpContextWrapper(ctx);
            var customerId = httpContext.GetCustomerIdFromCookie();
            var returnUrl =  UrlsHelper.BuildHomeUrl(customerId);
            return returnUrl;
        }

        private static bool IsSsoMode()
        {
            var loginMode = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
            return LoginMode.SSO.Equals(loginMode, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshSessionId()
        {
            //set new sessionId value to keep them in sync
            if (Context.Session != null)
            {
                var sessionGuid = Guid.NewGuid();
                new HttpContextWrapper(Context).SetSessionCookie(SessionIdKey, sessionGuid.ToString());
                Context.Session[SessionIdKey] = sessionGuid.ToString();
            }
        }

        private void ClearSessionId()
        {
            if (Context.Session != null)
            {
                new HttpContextWrapper(Context).SetSessionCookie(SessionIdKey, string.Empty);
                Context.Session[SessionIdKey] = string.Empty;
            }
        }

        #endregion
    }
}