using System.Diagnostics;
using System.IdentityModel.Tokens;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Infrastructure;

namespace DH.Helpdesk.SelfService
{
    public static class FederatedAuthenticationConfiguration
    {
        public static void Configure()
        {
            var federatedAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();

            var configuration = new FederatedAuthenticationSettings();
            var duration = configuration.SecurityTokenDuration;
            if (duration > 0)
            {
                federatedAuthenticationService.SetDefaultSessionDuration(duration);
            }

            if (configuration.EnableSlidingExpiration)
            {
                federatedAuthenticationService.EnableSlidingSessionExpirations();
            }

            if (configuration.HandleSecurityTokenExceptions)
            {
                federatedAuthenticationService.HandleSecurityTokenExceptions("~/", HandleSecurityTokenException);
            }
        }

        private static void HandleSecurityTokenException(SecurityTokenException ex)
        {
            var errorMsg = "Unknown security tokenk error. " + ex.Message;
            ErrorGenerator.MakeError(errorMsg);
            
            //since there's no logger in the project - log error with the trace.
            Trace.Write(errorMsg, "Error");
        }
    }
}