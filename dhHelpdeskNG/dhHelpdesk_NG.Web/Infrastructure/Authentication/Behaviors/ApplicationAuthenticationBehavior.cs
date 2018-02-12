using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class ApplicationAuthenticationBehavior : IAuthenticationBehavior
    {
        #region ctor()

        public string GetLoginUrl()
        {
            return FormsAuthentication.LoginUrl ?? "/Login/Login";
        }

        #endregion

        #region CreateUserIdentity

        public UserIdentity SignIn(HttpContextBase ctx)
        {
            // NOTE: Application mode can be used both by forms and windows authentication modes both at the same time or separately!
            // THATS WHY we first check windows identity and if its not authenticated by window module then we shall check forms identity 

            UserIdentity userIdentity = null;
            var identity = ctx.User.Identity;

            if (identity is WindowsIdentity)
            {
                userIdentity = CreateUserIdentity((WindowsIdentity) identity);
            }
            else if (identity is FormsIdentity)
            {
                userIdentity = CreateUserIdentity((FormsIdentity)identity);
            }

            return userIdentity;
        }

        private UserIdentity CreateUserIdentity(WindowsIdentity windowsIdentity)
        {
            // todo: check if we need to separate domain and user name
            return new UserIdentity
            {
                UserId = windowsIdentity.Name,
                //Domain = userDomain, 
                //FirstName = initiator?.FirstName,
                //LastName = initiator?.LastName,
                //EmployeeNumber = string.Empty,
                //Phone = initiator?.Phone,
                //Email = initiator?.Email
            };
        }

        private UserIdentity CreateUserIdentity(FormsIdentity formsIdentity)
        {
            return new UserIdentity
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

        #endregion

        #region SignOut

        public void SignOut(HttpContextBase ctx)
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}