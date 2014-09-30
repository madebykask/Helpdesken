using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Licenses
{
    public class LicensesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Licenses";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Licenses_default",
                "Licenses/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
