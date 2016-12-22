using System.Web.Mvc;

namespace ECT.FormLib.Areas.SouthKorea
{
    public class SouthKoreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SouthKorea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SouthKorea_default",
                "SouthKorea/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "ECT.FormLib.Areas.SouthKorea.Controllers" }
            );
        }
    }
}
