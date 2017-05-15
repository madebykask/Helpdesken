using System.Web.Http;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc.Invoices;

namespace DH.Helpdesk.Web.Areas.Invoices
{
    public class InvoicesAreaRegistration : AreaRegistration
    {
        public override string AreaName => MvcInvoicesUrlName.Name;

        public override void RegisterArea(AreaRegistrationContext context)
        {
            ConfigApiRoutes(context);

            ConfigMvcRoutes(context);
        }

        private void ConfigApiRoutes(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "InvoicesApiAction",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}/{action}"
                );

            context.Routes.MapHttpRoute(
                name: "InvoicesApi",
                routeTemplate: AreaName + "/" + WebApiConfig.UrlPrefixRelative + "/{controller}"
                );
        }

        private void ConfigMvcRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Invoices_default",
                 AreaName + "/{controller}/{action}/{id}",
                new { area = AreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}