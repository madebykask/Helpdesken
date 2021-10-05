using System.Web;
using System.Web.Security;
using DH.Helpdesk.Common.Types;
using System.Security.Claims;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class ApplicationAuthenticationBehavior : IAuthenticationBehavior
    {
        #region GetLoginUrl

        public string GetLoginUrl()
        {
            return FormsAuthentication.LoginUrl ?? "/Login/Login";
        }

        #endregion

        #region CreateUserIdentity

        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            var userIdentity = CreateUserIdentityInner(ctx);
            return userIdentity;
        }

        protected virtual UserIdentity CreateUserIdentityInner(HttpContextBase ctx)
        {
            UserIdentity userIdentity = null;
            var identity = ctx.User.Identity as ClaimsIdentity;
            userIdentity = new UserIdentity()
            {
                UserId = identity.Name,
                Domain = ""
                //FirstName = user?.FirstName,
                //LastName = user?.SurName,
                //EmployeeNumber = employeeNum,
                //Phone = user?.Phone,
                //Email = user?.Email ?? emailAddress
            };

            //if (identity != null)
            //{
            //    userIdentity = identity.CreateHelpdeskUserIdentity();
            //}
            return userIdentity;
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