namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Web.Mvc;

    public static class UrlExtension
    {
        public static string ActionAbsolute(
                                this UrlHelper url,
                                string actionName, 
                                string controllerName, 
                                object routeValues = null)
        {
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}