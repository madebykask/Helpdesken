using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.OrderAccounts
{
    public class OrderAccountsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OrderAccounts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OrderAccounts_default",
                "OrderAccounts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
