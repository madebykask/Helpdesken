using System;
using System.Security.Claims;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public static class ClaimsExtensions
    {
        public static string GetTypeName(this Claim claim)
        {
            if (claim == null)
                return string.Empty;

            var type = claim.Type;
            if (string.IsNullOrWhiteSpace(type))
                return null;

            int lastIndex = type.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);

            var claimTypeName = lastIndex > -1
                ? type.Substring(lastIndex + 1)
                : string.Empty;

            return claimTypeName;

        }

        public static string FindFirstOrEmpty(this ClaimsIdentity identity, string claimType)
        {
            var claim = identity.FindFirst(claimType);
            return claim == null ? string.Empty : claim.Value;
        }
    }
}