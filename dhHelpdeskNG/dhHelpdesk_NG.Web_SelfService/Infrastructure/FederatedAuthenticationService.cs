using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Web;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    public interface IFederatedAuthenticationService
    {
        void SetDefaultSessionDuration(int durationInMinutes);
        void EnableSlidingSessionExpirations();
        void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction);
        void SignOut();
    }

    public class FederatedAuthenticationService : IFederatedAuthenticationService
    {
        public void SetDefaultSessionDuration(int durationInMinutes)
        {
            PassiveSessionConfiguration.ConfigureDefaultSessionDuration(TimeSpan.FromMinutes(durationInMinutes));
        }

        public void EnableSlidingSessionExpirations()
        {
            PassiveModuleConfiguration.EnableSlidingSessionExpirations();
        }

        public void HandleSecurityTokenExceptions(string redirectUrl, Action<SecurityTokenException> handleAction)
        {
            PassiveModuleConfiguration.SuppressSecurityTokenExceptions("~/", handleAction);
        }

        public void SignOut()
        {
            var sam = FederatedAuthentication.SessionAuthenticationModule;
            sam?.SignOut();
        }
    }
}