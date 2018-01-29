using System.Web;
using System.Web.Security;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class FormsAuthenticationBehavior : IAuthenticationBehavior
    {
        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var formsIdentity = ctx.User.Identity as FormsIdentity;

            if (formsIdentity != null)
            {
                userIdentity = new UserIdentity
                {
                    UserId = formsIdentity.Name,
                    //Domain = userDomain,
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
            FormsAuthentication.SignOut();
        }
    }
}