using System;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Logger;

namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly ILoggerService _logger = LogManager.Session;
        private string _userPermission;

        #region ctor()

        public CustomAuthorize()
        {
        }

        public CustomAuthorize(string userPermissions)
        {
            UserPermsissions = userPermissions;
        }

        #endregion

        public string UserPermsissions
        {
            get
            {
                return this._userPermission ?? string.Empty;
            }

            set
            {
                this._userPermission = value;
            }
        }
        
        #region Methods

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            _logger.Debug("AuthorizeCore: request is unauthorised.");

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
            _logger.Debug("AuthorizeCore: checking request.");

            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (httpContext.Session == null)
            {
                return false;
            }
            
            var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
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