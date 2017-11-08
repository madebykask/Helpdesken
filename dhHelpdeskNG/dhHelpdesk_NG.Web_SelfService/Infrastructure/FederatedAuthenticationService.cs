using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Web;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    public interface IFederatedAuthenticationService
    {
        void SetDefaultSessionDuration(int durationInMinutes);
        SessionSecurityToken RefreshSecurityTokenLifeTime(SessionAuthenticationModule sam, SessionSecurityToken sessionToken, int maxTokenLifeTimeMin = 0);
        void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction);
        void SignOut(string returnUrl);
    }

    public class FederatedAuthenticationService : IFederatedAuthenticationService
    {
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
            SsoLogger.Debug($"SecurityToken current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");

            if (sessionToken.ValidTo.Subtract(sessionToken.ValidFrom) <= TimeSpan.Zero)
                return null;

            //check token max life time value
            if (maxTokenLifeTimeMin > 0 && DateTime.UtcNow.Subtract(sessionToken.ValidFrom) > TimeSpan.FromMinutes(maxTokenLifeTimeMin)) 
            {
                SsoLogger.Debug($"SecurityToken: Max lifetime ({maxTokenLifeTimeMin}min) has been reached. Current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");
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

                SsoLogger.Debug($"SecurityToken lifetime updated: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");
            }

            return refreshedToken;
        }

        #endregion

        public void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction)
        {
            PassiveModuleConfiguration.SuppressSecurityTokenExceptions("~/", handleAction);
        }

        public void SignOut(string returnUrl)
        {
            SessionFacade.ClearSession();

            //var sam = FederatedAuthentication.SessionAuthenticationModule;
            //sam?.SignOut();
            
            //sign out with FAM
            //FederatedAuthentication.WSFederationAuthenticationModule.SignOut();
            
            PerformPassiveSignOut(returnUrl);
        }

        #region Helper Methods

        private void PerformPassiveSignOut(string returnUrl = null)
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;

            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Delete();
            FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();

            //build sign out urls
            var signoutUrl = (WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(fam.Issuer, fam.Realm, null));
            var signOutUri = new Uri(signoutUrl);
            var returnUri = BuildSignOutReturnUri(returnUrl, fam.Realm);

            SsoLogger.Debug($"WSFedAuth Signout(). SignOutUrl: {signOutUri.OriginalString}, ReturnUrl: {returnUri.OriginalString}");
            WSFederationAuthenticationModule.FederatedSignOut(signOutUri, returnUri);
        }

        private Uri BuildSignOutReturnUri(string returnUrl, string realmUrl)
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