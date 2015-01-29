namespace DH.Helpdesk.Web.Areas.Reports
{
    using System.Web.Mvc;

    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { area = this.AreaName, action = "Index", id = UrlParameter.Optional });
        }
    }
}
