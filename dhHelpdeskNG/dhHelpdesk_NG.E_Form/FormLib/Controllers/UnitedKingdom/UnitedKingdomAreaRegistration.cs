using System.Web.Mvc;

namespace DH.Helpdesk.EForm.FormLib.Areas.UnitedKingdom
{
    public class UnitedKingdomAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UnitedKingdom";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UnitedKingdom_default",
                "UnitedKingdom/{controller}/{action}/{id}",
                new { action = "New", id = UrlParameter.Optional },
                new[] { "DH.Helpdesk.EForm.FormLib.Areas.UnitedKingdom.Controllers" }
            );
        }
    }
}
