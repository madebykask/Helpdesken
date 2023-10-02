using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Ninject.Web.WebApi;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(DH.Helpdesk.Web.App_Start.Startup))]
namespace DH.Helpdesk.Web.App_Start
{
    public partial class Startup
    {
        // The Client ID is used by the application to uniquely identify itself to Microsoft identity platform.
        string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"] != null ?
                          System.Configuration.ConfigurationManager.AppSettings["ClientId"] : "";

        // RedirectUri is the URL where the user will be redirected to after they sign in.
        string redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"] != null ?
                             System.Configuration.ConfigurationManager.AppSettings["RedirectUri"] : "";

        // Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
        static string tenant = System.Configuration.ConfigurationManager.AppSettings["Tenant"] != null ?
                               System.Configuration.ConfigurationManager.AppSettings["Tenant"] : "";

        static string auth = System.Configuration.ConfigurationManager.AppSettings["Authority"] != null ?
                               System.Configuration.ConfigurationManager.AppSettings["Authority"] : "";
        // Authority is the URL for authority, composed by Microsoft identity platform endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
        string authority = String.Format(System.Globalization.CultureInfo.InvariantCulture, auth, tenant);

        string microsoftLogin = System.Configuration.ConfigurationManager.AppSettings["MicrosoftLogin"] != null ?
        System.Configuration.ConfigurationManager.AppSettings["MicrosoftLogin"] : "";
        public void Configuration(IAppBuilder app)
        {
            System.Web.Helpers.AntiForgeryConfig.RequireSsl = true;

            var config = new HttpConfiguration();
             app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            var cookieOptions = new CookieAuthenticationOptions
            {
                //SameSiteMode.None should be always with Secure = true in chrome https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-5.0
                CookieSameSite = SameSiteMode.None,
                CookieSecure = CookieSecureOption.Always,
                CookieName ="HDCookie",
                //SlidingExpiration = true,
                //ExpireTimeSpan = TimeSpan.FromHours(2.0)
                //LoginPath = new PathString("/Login/Login")
            };
            app.UseCookieAuthentication(cookieOptions);
            if (!string.IsNullOrEmpty(microsoftLogin) && microsoftLogin == "1")
            {
                app.UseOpenIdConnectAuthentication(
                    new OpenIdConnectAuthenticationOptions
                    {
                        // Sets the ClientId, authority, RedirectUri as obtained from web.config
                        ClientId = clientId,
                        Authority = authority,
                        RedirectUri = redirectUri,
                        // PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
                        PostLogoutRedirectUri = redirectUri,
                        Scope = OpenIdConnectScope.OpenIdProfile,
                        // ResponseType is set to request the id_token - which contains basic information about the signed-in user
                        ResponseType = OpenIdConnectResponseType.IdToken,
                        //SignInAsAuthenticationType = cookieOptions.CookieName,
                        UseTokenLifetime = false,
                        // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                        // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
                        // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter
                        TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = false // This is a simplification
                        },
                        // OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
                        Notifications = new OpenIdConnectAuthenticationNotifications
                        {
                            SecurityTokenValidated = n =>
                            {
                                // Set persistent cookie, 
                                n.AuthenticationTicket.Properties.IsPersistent = true;
                                // and the expiration
                                n.AuthenticationTicket.Properties.ExpiresUtc = DateTime.Now.AddHours(48);
                                //n.AuthenticationTicket.Properties.ExpiresUtc = DateTime.Now.AddMinutes(2);
                                return Task.CompletedTask;
                            },
                            AuthenticationFailed = OnAuthenticationFailed
                        },
                        
                        // Just for debug purpose to see cookies values
                        // CookieManager = new SameSiteCookieManager(new SystemWebCookieManager())
                    }
                );
            }
            config.Filters.Add(new AuthorizeAttributeExtended());
            WebApiConfig.Register(config);
            ConfigureAuth(app);
            var kernel = NinjectWebCommon.Bootstrapper.Kernel;                        
            config.DependencyResolver = new NinjectDependencyResolver(kernel);            
            app.UseWebApi(config);
        }
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }
    }       
}