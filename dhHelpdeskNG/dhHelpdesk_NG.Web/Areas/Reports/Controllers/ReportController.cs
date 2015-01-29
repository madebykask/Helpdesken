namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class ReportController : BaseController
    {
        private readonly IReportModelFactory reportModelFactory;

        public ReportController(
            IMasterDataService masterDataService, 
            IReportModelFactory reportModelFactory)
            : base(masterDataService)
        {
            this.reportModelFactory = reportModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public JsonResult Reports()
        {
            var model = this.reportModelFactory.GetReportsOptions();

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
