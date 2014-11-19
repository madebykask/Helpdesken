using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    public class DailyReportController : BaseController
    {
        //
        // GET: /DailyReport/

        public ActionResult Index()
        {
            return View();
        }

    }
}
