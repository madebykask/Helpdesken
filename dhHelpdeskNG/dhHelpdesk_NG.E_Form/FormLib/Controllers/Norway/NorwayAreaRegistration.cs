using System.Web.Mvc;

namespace DH.Helpdesk.EForm.FormLib.Areas.Norway
{
    public class NorwayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Norway";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Norway_default",
                "Norway/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers" }
            );
        }
    }
}
