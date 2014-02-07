namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Web.Mvc;

    public class CaseController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Search()
        {
            return this.View();
        }

        public ActionResult New()
        {
            return this.View();
        }
    }
}
