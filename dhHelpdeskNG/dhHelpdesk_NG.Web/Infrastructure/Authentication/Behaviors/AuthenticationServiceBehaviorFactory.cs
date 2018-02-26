using System;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Dal.Repositories.ADFS;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public interface IAuthenticationServiceBehaviorFactory
    {
        IAuthenticationBehavior Create(LoginMode mode);
    }

    public class AuthenticationServiceBehaviorFactory : IAuthenticationServiceBehaviorFactory
    {
        private readonly IApplicationConfiguration _appConfiguration;
        private readonly IAdfsConfiguration _adfsConfiguration;
        private readonly IFederatedAuthenticationService _federatedAuthenticationService;
        private readonly IADFSRepository _adfsRepository;

        #region ctor()

        public AuthenticationServiceBehaviorFactory(IApplicationConfiguration appConfiguration,
            IAdfsConfiguration adfsConfiguration,
            IFederatedAuthenticationService federatedAuthenticationService,
            IADFSRepository adfsRepository)
        {
            _appConfiguration = appConfiguration;
            _adfsConfiguration = adfsConfiguration;
            _federatedAuthenticationService = federatedAuthenticationService;
            _adfsRepository = adfsRepository;
        }

        #endregion

        public IAuthenticationBehavior Create(LoginMode mode)
        {
            if (mode == LoginMode.Application)
            {
                return new ApplicationAuthenticationBehavior();
            }
            else if (mode == LoginMode.SSO)
            {
                return new AdfsAuthenticationBehavior(_appConfiguration, _adfsConfiguration, _federatedAuthenticationService, _adfsRepository);
            }
            else if (mode == LoginMode.Windows)
            {
              return new WindowsAuthenticationBehavior();
            }

            throw new NotSupportedException($"Login mode '{mode}' is not supported");
        }
    }
}