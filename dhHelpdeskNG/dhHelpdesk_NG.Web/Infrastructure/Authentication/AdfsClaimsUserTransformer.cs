using System;
using System.Security.Claims;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public interface IClaimsUserTransformer
    {
        UserIdentity Transform(ClaimsPrincipal principal);
    }

    public class AdfsClaimsUserTransformer : IClaimsUserTransformer
    {
        private readonly IAdfsClaimsSettings _claimsSettings;

        #region ctor()

        public AdfsClaimsUserTransformer(IAdfsClaimsSettings claimsSettings)
        {
            _claimsSettings = claimsSettings;
        }

        #endregion

        public UserIdentity Transform(ClaimsPrincipal principal)
        {
            var userIdentity = new UserIdentity();

            foreach (var claim in principal.Claims)
            {
                var pureType = claim.GetTypeName()?.Trim();
                if (string.IsNullOrWhiteSpace(pureType))
                    continue;

                var value = claim.Value;

                if (pureType.Equals(_claimsSettings.ClaimDomain, StringComparison.OrdinalIgnoreCase))
                    userIdentity.Domain = value;

                if (pureType.Equals(_claimsSettings.ClaimUserId, StringComparison.OrdinalIgnoreCase))
                    userIdentity.UserId = value;

                if (pureType.Equals(_claimsSettings.ClaimEmployeeNumber, StringComparison.OrdinalIgnoreCase))
                    userIdentity.EmployeeNumber = value;

                if (pureType.Equals(_claimsSettings.ClaimFirstName, StringComparison.OrdinalIgnoreCase))
                    userIdentity.FirstName = value;

                if (pureType.Equals(_claimsSettings.ClaimLastName, StringComparison.OrdinalIgnoreCase))
                    userIdentity.LastName = value;

                if (pureType.Equals(_claimsSettings.ClaimEmail, StringComparison.OrdinalIgnoreCase))
                    userIdentity.Email = value;

                if (pureType.Equals(_claimsSettings.ClaimPhone, StringComparison.OrdinalIgnoreCase))
                    userIdentity.Phone = value;
            }

            return userIdentity;
        }
    }
}