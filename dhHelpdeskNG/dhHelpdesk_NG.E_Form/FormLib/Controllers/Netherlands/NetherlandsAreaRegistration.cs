using System.Web.Mvc;

namespace ECT.FormLib.Areas.Netherlands
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
                new { action = "New", id = UrlParameter.Optional },
                new[] { "ECT.FormLib.Areas.Netherlands.Controllers" }
            );
        }
    }
}
