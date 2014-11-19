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
        public DailyReportController(            
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            //
        }
        //
        // GET: /DailyReport/
        

        public ActionResult Index()
        {
            return View();
        }

    }
}
