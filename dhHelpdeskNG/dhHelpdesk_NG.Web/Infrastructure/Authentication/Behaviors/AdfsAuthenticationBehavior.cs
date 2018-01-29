using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using DH.Helpdesk.BusinessData.Models.ADFS.Input;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.Repositories.ADFS;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Configuration;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public interface IAuthenticationBehavior
    {
        UserIdentity CreateUserIdentity(HttpContextBase ctx);
        void SignOut(HttpContextBase ctx);
    }

    public class AdfsAuthenticationBehavior : IAuthenticationBehavior
    {
        private readonly IApplicationConfiguration _appConfiguration;
        private readonly IAdfsConfiguration _adfsConfiguration;
        private readonly IFederatedAuthenticationService _federatedAuthenticationService;
        private readonly IADFSRepository _adfsRepository;
        private readonly AdfsClaimsUserTransformer _adfsClaimsUserTransformer;

        public AdfsAuthenticationBehavior(IApplicationConfiguration appConfiguration, 
            IAdfsConfiguration adfsConfiguration, 
            IFederatedAuthenticationService federatedAuthenticationService,
            IADFSRepository adfsRepository)
        {
            _appConfiguration = appConfiguration;
            _adfsConfiguration = adfsConfiguration;
            _federatedAuthenticationService = federatedAuthenticationService;
            _adfsRepository = adfsRepository;
            _adfsClaimsUserTransformer = new AdfsClaimsUserTransformer(adfsConfiguration);
        }

        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var principal = ctx.User as ClaimsPrincipal;
            if (principal != null)
            {
                userIdentity = _adfsClaimsUserTransformer.Transform(principal);

                if (userIdentity != null && _adfsConfiguration.SsoLog)
                {
                    LogSsoSession(principal);
                }
            }

            return userIdentity;
        }

        public void SignOut(HttpContextBase ctx)
        {
            _federatedAuthenticationService.SignOut(ctx.Request.Url?.ToString());
        }

        #region Private Methods

        private void LogSsoSession(ClaimsPrincipal principal)
        {
            var formattedClaims = FormatPrincipalClaims(principal);
            var netId = principal.Identity.Name;
            var ssoLog = new NewSSOLog()
            {
                ApplicationId = _appConfiguration.ApplicationId,
                NetworkId = netId,
                ClaimData = formattedClaims,
                CreatedDate = DateTime.Now
            };

            _adfsRepository.SaveSSOLog(ssoLog);
            _adfsRepository.Commit();
        }

        private string FormatPrincipalClaims(ClaimsPrincipal principal)
        {
            var claimDataBld = new StringBuilder();
            const string emptyType = "Undefined";
            var isFirst = true;

            foreach (var claim in principal.Claims)
            {
                var pureType = claim.GetTypeName()?.CleanSpaceAndLowStr();
                var typeNameFormatted = string.IsNullOrEmpty(pureType) ? emptyType : pureType;

                if (!isFirst)
                    claimDataBld.Append(" , ");

                claimDataBld.Append($"[ {typeNameFormatted} : {claim.Value} ] , ");
                isFirst = false;
            }

            var output = claimDataBld.ToString();
            return output;
        }

        #endregion
    }
}