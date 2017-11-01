using System;
using System.Diagnostics;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Web;
using System.Web.Http.Routing;
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
            Trace.WriteLine($"SecurityToken current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");

            if (sessionToken.ValidTo.Subtract(sessionToken.ValidFrom) <= TimeSpan.Zero)
                return null;

            //check token max life time value
            if (maxTokenLifeTimeMin > 0 && DateTime.UtcNow.Subtract(sessionToken.ValidFrom) > TimeSpan.FromMinutes(maxTokenLifeTimeMin)) 
            {
                Trace.WriteLine($"SecurityToken: Max lifetime ({maxTokenLifeTimeMin}min) has been reached. Current lifetime: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");
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
                
                Trace.WriteLine($"SecurityToken lifetime updated: {sessionToken.ValidFrom} - {sessionToken.ValidTo}");
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

        private void PerformPassiveSignOut(string currentUrl = null)
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;

            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Delete();
            FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
            var signoutUrl = (WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(fam.Issuer, fam.Realm, null));

            // Check where to return, if not set ACS will use Reply address configured for the RP
            var returnUrl = !string.IsNullOrEmpty(currentUrl) ? currentUrl : (!string.IsNullOrEmpty(fam.Reply) ? fam.Reply : null);

            //new Uri(authModule.Issuer)
            var returnUri = string.IsNullOrEmpty(returnUrl) ? null : new Uri(returnUrl);
            WSFederationAuthenticationModule.FederatedSignOut(new Uri(signoutUrl), returnUri);
        }

        #endregion
    }
}