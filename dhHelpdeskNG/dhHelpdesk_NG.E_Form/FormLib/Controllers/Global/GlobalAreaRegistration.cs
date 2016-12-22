using System.Web.Mvc;

namespace DH.Helpdesk.EForm.FormLib.Areas.Global
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
                new[] { "DH.Helpdesk.EForm.FormLib.Areas.Global.Controllers" }
            );
        }
    }
}
