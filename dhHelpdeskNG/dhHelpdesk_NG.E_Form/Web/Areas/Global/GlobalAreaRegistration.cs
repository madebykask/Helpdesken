using System.Web.Mvc;

namespace ECT.Web.Areas.Global
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
                new { action = "new", id = UrlParameter.Optional },
                new[] { "ECT.Web.Areas.Global.Controllers" }
            );
        }
    }
}
