namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports;

    public sealed class ReportsController : UserInteractionController
    {
        private readonly IReportsModelFactory reportsModelFactory;

        private readonly IReportsService reportsService;

        public ReportsController(
            IMasterDataService masterDataService, 
            IReportsModelFactory reportsModelFactory, 
            IReportsService reportsService)
            : base(masterDataService)
        {
            this.reportsModelFactory = reportsModelFactory;
            this.reportsService = reportsService;
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

            var searchData = this.reportsService.GetSearchData(this.OperationContext);
            var model = this.reportsModelFactory.CreateSearchModel(filters, searchData);
            return this.PartialView(model);
        } 
    }
}