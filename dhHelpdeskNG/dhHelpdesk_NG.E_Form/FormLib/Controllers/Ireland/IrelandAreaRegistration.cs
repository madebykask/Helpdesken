using System.Web.Mvc;

namespace DH.Helpdesk.EForm.FormLib.Areas.Ireland
{
    public class IrelandAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ireland";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ireland_default",
                "Ireland/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "DH.Helpdesk.EForm.FormLib.Areas.Ireland.Controllers" }
            );
        }
    }
}
