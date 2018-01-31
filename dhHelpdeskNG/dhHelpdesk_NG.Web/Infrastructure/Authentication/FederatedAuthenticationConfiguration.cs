using System.IdentityModel.Tokens;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;
using DH.Helpdesk.Web.Infrastructure.Logger;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class FederatedAuthenticationConfiguration
    {
        public static void Configure()
        {
            var federatedAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();
            var configuration = ManualDependencyResolver.Get<IAdfsConfiguration>();

            var duration = configuration.SecurityTokenDuration;
            if (duration > 0)
            {
                federatedAuthenticationService.SetDefaultSessionDuration(duration);
            }

            federatedAuthenticationService.HandleSecurityTokenExceptions("~/", HandleSecurityTokenException);
        }

        private static void HandleSecurityTokenException(SecurityTokenException ex)
        {

            var errorMsg = "Unknown security token exception. " + ex.Message;
            LogManager.Session.Error(errorMsg);
        }
    }
}