using System;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    public class GdprAccessAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            bool isAuthenticated = httpContext.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                return false;
            }

            var userId = SessionFacade.CurrentUser.Id;
            var privacyAccessService = DependencyResolver.Current.GetService<IGDPRDataPrivacyAccessService>();
            var access = privacyAccessService.GetUserWithPrivacyPermissionsByUserId(userId);

            return access != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Success = false,
                        Error = "You are not authorised",
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("~/Error/Unathorized");
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
            }
        }
    }
}