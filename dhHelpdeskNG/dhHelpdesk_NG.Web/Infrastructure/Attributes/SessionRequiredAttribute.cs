namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class SessionRequiredAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session == null)
            {
                return false;
            }

            if (HttpContext.Current != null && HttpContext.Current.Session == null)
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary 
                                   {
                                       { "action", "Logout" },
                                       { "controller", "Login" }
                                   });
        }
    }
}