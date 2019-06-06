using System.Web.Routing;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class RouteDataExtensions
    {
        const string ControllerKey = "controller";
        const string ActionKey = "action";

        public static string GetControllerName(this RouteData routeData)
        {
            if (routeData?.Values == null || !routeData.Values.ContainsKey(ControllerKey))
                return null;

            return (routeData.Values[ControllerKey] ?? string.Empty).ToString().ToLower();
        }

        public static void SetControllerValue(this RouteData routeData, string controllerName)
        {
            if (routeData?.Values == null) return;
            routeData.Values[ControllerKey] = controllerName;
        }

        public static string GetActionName(this RouteData routeData)
        {
            if (routeData?.Values == null || !routeData.Values.ContainsKey(ActionKey))
                return null;

            return (routeData.Values[ActionKey] ?? string.Empty).ToString().ToLower();
        }

        public static void SetActionValue(this RouteData routeData, string actionName)
        {
            if (routeData?.Values == null) return;
            routeData.Values[ActionKey] = actionName;
        }
    }
}