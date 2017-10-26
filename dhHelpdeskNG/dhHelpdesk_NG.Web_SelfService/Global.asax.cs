using System;
using System.Diagnostics;
using System.IdentityModel.Services;
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
                federationAuthenticationService.SignOut(Request.Url);
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
                //FederatedAuthentication.WSFederationAuthenticationModule.SignOut();

                //get redirect url after sign in. On Post we need to take original url from UrlReferrer
                var returnUrl = Request.Url;
                if (Request.HttpMethod == "POST" && Request.UrlReferrer != null)
                {
                    returnUrl = Request.UrlReferrer;
                }

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
    }
}