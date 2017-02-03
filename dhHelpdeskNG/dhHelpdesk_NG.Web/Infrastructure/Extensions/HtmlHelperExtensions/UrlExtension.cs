using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Api.Orders;

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

        public static string ApiDefault(this UrlHelper url, string controllerName)
        {
            return url.HttpRouteUrl(WebApiUrlName.DefaultRouteName,
              new { controller = controllerName });
        }

        public static string ApiAction(this UrlHelper url, string actionName, string controllerName)
        {
            return url.HttpRouteUrl(WebApiUrlName.ActionApiRouteName,
            new { controller = controllerName, action = actionName });
        }

        public static string ApiAction(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            var dictionaryRoutes = routeValues.ObjectToDictionary();
            dictionaryRoutes.Add("controller", controllerName);
            dictionaryRoutes.Add("action", actionName);

            return url.HttpRouteUrl(WebApiUrlName.ActionApiRouteName, dictionaryRoutes);
        }

        public static string ApiOrdersAction(this UrlHelper url, string actionName, string controllerName)
        {
            return url.HttpRouteUrl("OrdersApiAction",
            new { controller = controllerName, action = actionName});
        }

        public static string ApiOrdersAction(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            var dictionaryRoutes = routeValues.ObjectToDictionary();
            dictionaryRoutes.Add("controller", controllerName);
            dictionaryRoutes.Add("action", actionName);

            return url.HttpRouteUrl("OrdersApiAction", dictionaryRoutes);
        }

    }
}