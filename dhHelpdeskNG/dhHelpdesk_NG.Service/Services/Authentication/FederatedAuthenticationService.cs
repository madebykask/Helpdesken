using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Web;
using DH.Helpdesk.Common.Logger;
using log4net;
using Thinktecture.IdentityModel.Web;

namespace DH.Helpdesk.Services.Services.Authentication
{
    public interface IFederatedAuthenticationService
    {
        void SetDefaultSessionDuration(int durationInMinutes);
        SessionSecurityToken RefreshSecurityTokenLifeTime(SessionAuthenticationModule sam, SessionSecurityToken sessionToken, int maxTokenLifeTimeMin = 0);
        void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction);
        void SignOut(string returnUrl, bool localOnly = false);

        string GetSignInUrl();
        string GetSignOutUrl();
    }

    public class FederatedAuthenticationService : IFederatedAuthenticationService
    {
        private readonly ILoggerService _logger;

        #region ctor

        public FederatedAuthenticationService()
        {
        }

        public FederatedAuthenticationService(ILoggerService logger)
        {
            _logger = logger;
        }

        #endregion

        #region SetDefaultSessionDuration

        public void SetDefaultSessionDuration(int durationInMinutes)
        {
            PassiveSessionConfiguration.ConfigureDefaultSessionDuration(TimeSpan.FromMinutes(durationInMinutes));
            PassiveModuleConfiguration.OverrideWSFedTokenLifetime();
        }

        #endregion

        #region RefreshToken

        public SessionSecurityToken RefreshSecurityTokenLifeTime(SessionAuthenticationModule sam, SessionSecurityToken sessionToken, int maxTokenLifeTimeMin = 0)
        {
            _logger.Debug($"SecurityToken current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");

            if (sessionToken.ValidTo.Subtract(sessionToken.ValidFrom) <= TimeSpan.Zero)
                return null;

            //check token max life time value
            if (maxTokenLifeTimeMin > 0 && DateTime.UtcNow.Subtract(sessionToken.ValidFrom) > TimeSpan.FromMinutes(maxTokenLifeTimeMin)) 
            {
                _logger.Debug($"SecurityToken: Max lifetime ({maxTokenLifeTimeMin}min) has been reached. Current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");
                return null;
            }

            SessionSecurityToken refreshedToken = null;

            if (sessionToken.ValidTo > DateTime.UtcNow)
            {
                //get duration from SessionSecurityTokenHandler
                var handler = sam.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[typeof(SessionSecurityToken)] as SessionSecurityTokenHandler;
                var duration = handler.TokenLifetime;

                refreshedToken =
                      new SessionSecurityToken(
                          sessionToken.ClaimsPrincipal,
                          sessionToken.Context,
                          sessionToken.ValidFrom,
                          DateTime.UtcNow.Add(duration))
                      {
                          IsPersistent = sessionToken.IsPersistent,
                          IsReferenceMode = sessionToken.IsReferenceMode
                      };

                _logger.Debug($"SecurityToken lifetime updated: {refreshedToken.ValidFrom} - {refreshedToken.ValidTo}");
            }

            return refreshedToken;
        }

        #endregion

        public void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction)
        {
            PassiveModuleConfiguration.SuppressSecurityTokenExceptions("~/", handleAction);
        }

        public void SignOut(string returnUrl, bool localOnly = false)
        {
            DoLocalSignOut();

            if (!localOnly)
            {
                var fam = FederatedAuthentication.WSFederationAuthenticationModule;

                //build sign out urls
                var signoutUrl = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(fam.Issuer, fam.Realm, null);
                var signOutUri = new Uri(signoutUrl);
                var returnUri = BuildReturnUri(returnUrl, fam.Realm);

                _logger.Debug($"WSFedAuth Signout(). SignOutUrl: {signOutUri.OriginalString}, ReturnUrl: {returnUri.OriginalString}");
                WSFederationAuthenticationModule.FederatedSignOut(signOutUri, returnUri);
            }
        }

        #region Helper Methods

        private void DoLocalSignOut()
        {
            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Delete();
            FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();

            WSFederationAuthenticationModule fedAuthenticationModule = FederatedAuthentication.WSFederationAuthenticationModule;
            fedAuthenticationModule.SignOut(false);
        }

        public string GetSignInUrl()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var msg = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm);

            var url = msg.WriteQueryString();
            return url;
        }

        public string GetSignOutUrl()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            var signOutRequestMessage = new SignOutRequestMessage(new Uri(fam.Issuer), fam.Realm);

            var url = signOutRequestMessage.WriteQueryString();
            return url;
        }

        private Uri BuildReturnUri(string returnUrl, string realmUrl)
        {
            var returnUri = string.IsNullOrEmpty(returnUrl) ? null : new Uri(returnUrl, System.UriKind.RelativeOrAbsolute);
            if (returnUri != null && returnUri.IsAbsoluteUri)
            {
                //convert to relative
                returnUri = returnUri.MakeRelativeUri(new Uri(realmUrl));
            }
            
            var absoluteUri = returnUri != null ? new Uri($"{realmUrl.TrimEnd('/')}/{returnUri.OriginalString.TrimStart('/')}") : new Uri(realmUrl);
            return absoluteUri;
        }

        #endregion
    }
}