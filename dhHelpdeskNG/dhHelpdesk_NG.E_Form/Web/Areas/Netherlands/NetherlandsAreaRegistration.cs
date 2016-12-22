using System.Web.Mvc;

namespace ECT.Web.Areas.Netherlands
{
    public class NetherlandsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Netherlands";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Netherlands_default",
                "Netherlands/{controller}/{action}/{id}",
                new { action = "new", id = UrlParameter.Optional },
                new[] { "ECT.Web.Areas.Netherlands.Controllers" }
            );
        }
    }
}
