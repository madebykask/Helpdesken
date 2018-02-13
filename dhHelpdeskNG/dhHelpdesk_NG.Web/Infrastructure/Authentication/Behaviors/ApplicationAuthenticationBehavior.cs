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

        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var identity = ctx.User.Identity as FormsIdentity;

            if (identity != null)
            {
                userIdentity = CreateUserIdentity((FormsIdentity)identity);
            }

            return userIdentity;
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