using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.Services.Services;

    public class DailyReportController : BaseController
    {
        //
        // GET: /DailyReport/

        public DailyReportController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
