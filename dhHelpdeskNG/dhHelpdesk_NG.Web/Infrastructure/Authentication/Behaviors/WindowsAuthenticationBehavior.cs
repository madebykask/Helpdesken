using System.Security.Principal;
using System.Web;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class WindowsAuthenticationBehavior : IAuthenticationBehavior
    {
        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var windowsPrincipal = ctx.User as WindowsPrincipal;

            if (windowsPrincipal != null)
            {
                var fullName = windowsPrincipal.Identity.Name;
                var userId = fullName.GetUserFromAdPath();

                var userDomain = fullName.GetDomainFromAdPath();
                
                //var initiator = _notifierRepository.GetInitiatorByUserId(userId, customerId, true);

                userIdentity = new UserIdentity(userId)
                {
                    Domain = userDomain,
                    //FirstName = initiator?.FirstName,
                    //LastName = initiator?.LastName,
                    //EmployeeNumber = string.Empty,
                    //Phone = initiator?.Phone,
                    //Email = initiator?.Email
                };
            }

            return userIdentity;
        }

        public void SignOut(HttpContextBase ctx)
        {
        }

        public string GetLoginUrl()
        {
            return string.Empty;
        }
    }
}

