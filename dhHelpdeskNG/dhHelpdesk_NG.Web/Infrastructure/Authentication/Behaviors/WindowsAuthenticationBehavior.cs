using System.Security.Principal;
using System.Web;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class WindowsAuthenticationBehavior : IAuthenticationBehavior
    {
        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var windowsIdentity = ctx.User.Identity as WindowsIdentity;
            if (windowsIdentity != null)
            {
                userIdentity = windowsIdentity.CreateHelpdeskUserIdentity();
            }

            return userIdentity;
        }

        public void SignOut(HttpContextBase ctx)
        {
            // no way to sign out for windows mode
        }

        public string GetLoginUrl()
        {
            return string.Empty;
        }
    }
}

