using System;
using System.Web.Mvc;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            return View();
        }

    }
}
