using System;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private string userPermission;

        public string UserPermsissions
        {
            get
            {
                return this.userPermission ?? string.Empty;
            }

            set
            {
                this.userPermission = value;
            }
        }
        
        #region Methods

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Error/Unathorized");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            //todo: implement ajax request handling
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (httpContext.Session == null)
            {
                httpContext.Response.Redirect("~/login/login");
                return false;
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.Roles != string.Empty)
            {
                foreach (string userRole in this.Roles.Split(','))
                {
                    if (GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole))
                    {
                        return true;
                    }
                }

                return false;
            }

            if (this.UserPermsissions != string.Empty)
            {
                foreach (string userPermission in this.UserPermsissions.Split(','))
                {
                    if (GeneralExtensions.UserHasPermission(SessionFacade.CurrentUser, userPermission))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// NO any specific ACL politic is set
            if (this.Roles == string.Empty && this.UserPermsissions == string.Empty)
            {
                return true;
            }
            

            return false;
        }

        #endregion
    }
}