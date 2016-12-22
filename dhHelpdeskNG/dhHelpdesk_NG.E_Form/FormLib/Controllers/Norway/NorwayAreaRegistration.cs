using System.Web.Mvc;

namespace ECT.FormLib.Areas.Norway
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
                new[] { "ECT.FormLib.Areas.Norway.Controllers" }
            );
        }
    }
}
