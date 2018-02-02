using System.Diagnostics;
using System.IdentityModel.Tokens;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services.Authentication;

namespace DH.Helpdesk.SelfService
{
    public static class FederatedAuthenticationConfiguration
    {
        public static void Configure()
        {
            var federatedAuthenticationService = ManualDependencyResolver.Get<IFederatedAuthenticationService>();
            var configuration = ManualDependencyResolver.Get<IFederatedAuthenticationSettings>();

            var duration = configuration.SecurityTokenDuration;
            if (duration > 0)
            {
                federatedAuthenticationService.SetDefaultSessionDuration(duration);
            }

            if (configuration.HandleSecurityTokenExceptions)
            {
                federatedAuthenticationService.HandleSecurityTokenExceptions("~/", HandleSecurityTokenException);
            }
        }

        private static void HandleSecurityTokenException(SecurityTokenException ex)
        {
            var errorMsg = "Unknown security token exception. " + ex.Message;
            ErrorGenerator.MakeError(errorMsg);
            SsoLogger.Debug(errorMsg);
        }
    }
}