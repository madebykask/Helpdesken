using System.Security.Principal;
using System.Web;
using System.Web.Security;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class MixedModeAuthenticationBehavior : IAuthenticationBehavior
    {
        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            //1. first check forms authentication identity
            UserIdentity userIdentity = null;
            var formsIdentity = ctx.User.Identity as FormsIdentity;
            if (formsIdentity != null)
            {
                userIdentity = formsIdentity.CreateHelpdeskUserIdentity();
            }

            //2. then try create windows identity
            if (userIdentity == null)
            {
                var windowsIdentity = ctx.User.Identity as WindowsIdentity;
                if (windowsIdentity != null)
                {
                    userIdentity = windowsIdentity.CreateHelpdeskUserIdentity();
                }
            }

            return userIdentity;
        }

        public string GetLoginUrl()
        {
            return "/"; // return home page to display windows login page first, if cancelled user will be redirected by auth filter to site login page 
        }

        public void SignOut(HttpContextBase ctx)
        {
            FormsAuthentication.SignOut();
        }
    }
}