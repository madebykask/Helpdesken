using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Orders
{
    public class OrdersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Orders";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Orders_default",
                "Orders/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
