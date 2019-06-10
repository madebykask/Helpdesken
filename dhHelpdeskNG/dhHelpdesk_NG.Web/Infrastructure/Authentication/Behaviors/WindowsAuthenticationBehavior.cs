using System.Security.Principal;
using System.Web;
using System.Web.Security;
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
            // in windows mode a user can use login page to login with helpdesk credentials
            FormsAuthentication.SignOut();
        }

        public string GetLoginUrl()
        {
            return "/"; // for windows mode - redirect to any secured page so that a windows login would should up. Login page is not secured - anonymous;
        }
    }
}

