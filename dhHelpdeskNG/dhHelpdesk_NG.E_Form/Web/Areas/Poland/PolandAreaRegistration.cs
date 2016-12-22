using System.Web.Mvc;

namespace ECT.Web.Areas.Poland
{
    public class PolandAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Poland";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Poland_action",
                "poland/action/{action}/{id}",
                new { controller = "action", action = "index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Poland_default",
                "poland/{controller}/{action}/{id}",
                new { action = "new", id = UrlParameter.Optional },
                new[] { "ECT.Web.Areas.Poland.Controllers" }
            );
        }
    }
}
