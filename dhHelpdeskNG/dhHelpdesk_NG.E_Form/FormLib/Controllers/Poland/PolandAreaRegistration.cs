using System.Web.Mvc;

namespace ECT.FormLib.Areas.Poland
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
                "Poland_default",
                "Poland/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "ECT.FormLib.Areas.Poland.Controllers" }
            );
        }
    }
}
