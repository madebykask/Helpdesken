using System.Security.Principal;
using System.Web;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class MixedModeAuthenticationBehavior : ApplicationAuthenticationBehavior
    {
        protected override UserIdentity CreateUserIdentityInner(HttpContextBase ctx)
        {
            //1. try to create with base application behavior
            var userIdentity = base.CreateUserIdentityInner(ctx);

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
    }
}