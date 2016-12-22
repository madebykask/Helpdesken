using System.Web.Mvc;

namespace ECT.FormLib.Areas.Global
{
    public class GlobalAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Global";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Global_default",
                "Global/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "ECT.FormLib.Areas.Global.Controllers" }
            );
        }
    }
}
