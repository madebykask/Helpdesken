namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Mvc;

    public sealed class ReportController : UserInteractionController
    {
        private readonly IReportModelFactory reportModelFactory;

        private readonly IReportService reportService;

        private readonly IReportsBuilder reportsBuilder;

        public ReportController(
            IMasterDataService masterDataService, 
            IReportModelFactory reportModelFactory, 
            IReportService reportService, 
            IReportsBuilder reportsBuilder)
            : base(masterDataService)
        {
            this.reportModelFactory = reportModelFactory;
            this.reportService = reportService;
            this.reportsBuilder = reportsBuilder;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = this.reportModelFactory.GetReportsOptions();
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetReportOptions(ReportsOptions searchModel)
        {
            var filters = searchModel != null
                  ? searchModel.ExtractFilters()
                  : SessionFacade.FindPageFilters<ReportsFilterModel>(PageName.Reports);

            SessionFacade.SavePageFilters(PageName.Reports, filters);

            switch ((ReportType)filters.ReportId)
            {
                case ReportType.RegistratedCasesDay:
                    var options = this.reportService.GetRegistratedCasesDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/RegistratedCasesDay",
                                this.reportModelFactory.GetRegistratedCasesDayOptionsModel(options));
            }

            return null;
        }

        [HttpGet]
        public UnicodeFileContentResult GetRegistratedCasesDayReport(
                                    int? department, 
                                    string caseTypes, 
                                    int? workingGroup, 
                                    int? administrator,
                                    DateTime period)
        {
            var caseTypeArr = caseTypes != null ? 
                    caseTypes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() : 
                    new int[0];

            var data = this.reportService.GetRegistratedCasesDayData(
                                    this.OperationContext.CustomerId,
                                    department,
                                    caseTypeArr,
                                    workingGroup,
                                    administrator,
                                    period);

            var report = this.reportsBuilder.GetRegistratedCasesDayReport(data, period);

            return new UnicodeFileContentResult(report, string.Empty);
        }
    }
}
