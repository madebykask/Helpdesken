namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports;

    public sealed class ReportsController : BaseController
    {
        private readonly IReportsModelFactory reportsModelFactory;

        public ReportsController(
            IMasterDataService masterDataService, 
            IReportsModelFactory reportsModelFactory)
            : base(masterDataService)
        {
            this.reportsModelFactory = reportsModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var model = this.reportsModelFactory.CreateIndexModel();
            return this.View(model);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Reports()
        {
            return this.PartialView();
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Search()
        {
            var filters = SessionFacade.FindPageFilters<ReportsFilter>(PageName.Reports);
            if (filters == null)
            {
                filters = ReportsFilter.CreateDefault();
                SessionFacade.SavePageFilters(PageName.Reports, filters);
            }
            return null;
        } 
    }
}