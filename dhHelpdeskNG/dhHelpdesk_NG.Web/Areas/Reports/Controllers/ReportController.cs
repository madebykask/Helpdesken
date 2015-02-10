namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class ReportController : UserInteractionController
    {
        private readonly IReportModelFactory reportModelFactory;

        private readonly IReportGeneratorModelFactory reportGeneratorModelFactory;

        private readonly IReportService reportService;

        private readonly IReportsBuilder reportsBuilder;

        private readonly IPrintBuilder printBuilder;

        private readonly IExcelBuilder excelBuilder;

        public ReportController(
            IMasterDataService masterDataService, 
            IReportModelFactory reportModelFactory, 
            IReportService reportService, 
            IReportsBuilder reportsBuilder, 
            IPrintBuilder printBuilder, 
            IExcelBuilder excelBuilder, 
            IReportGeneratorModelFactory reportGeneratorModelFactory)
            : base(masterDataService)
        {
            this.reportModelFactory = reportModelFactory;
            this.reportService = reportService;
            this.reportsBuilder = reportsBuilder;
            this.printBuilder = printBuilder;
            this.excelBuilder = excelBuilder;
            this.reportGeneratorModelFactory = reportGeneratorModelFactory;
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
                    var registratedCasesDayOptions = this.reportService.GetRegistratedCasesDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/RegistratedCasesDay",
                                this.reportModelFactory.GetRegistratedCasesDayOptionsModel(registratedCasesDayOptions));

                case ReportType.CaseTypeArticleNo:
                    var caseTypeArticleNoOptions = this.reportService.GetCaseTypeArticleNoOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/CaseTypeArticleNo",
                                this.reportModelFactory.GetCaseTypeArticleNoOptionsModel(caseTypeArticleNoOptions));

                case ReportType.CaseSatisfaction:
                    return this.PartialView(
                                "Options/SurveySatisfactionOptions",
                                this.reportModelFactory.CreateCaseSatisfactionOptions(this.OperationContext));

                case ReportType.ReportGenerator:
                    var reportGeneratorFilters = SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator)
                                                 ?? ReportGeneratorFilterModel.CreateDefault();

                    SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, reportGeneratorFilters);
                    var reportGeneratorOptions = this.reportService.GetReportGeneratorOptions(this.OperationContext.CustomerId, this.OperationContext.LanguageId);
                    return this.PartialView(
                                "Options/ReportGenerator",
                                this.reportGeneratorModelFactory.GetReportGeneratorOptionsModel(reportGeneratorOptions, reportGeneratorFilters));
            }

            return null;
        }

        [HttpGet]
        public UnicodeFileContentResult GetRegistratedCasesDayReport(
                                    int? department, 
                                    string caseTypes, 
                                    int? workingGroup, 
                                    int? administrator,
                                    DateTime? period)
        {
            var data = this.reportService.GetRegistratedCasesDayData(
                                    this.OperationContext.CustomerId,
                                    department,
                                    caseTypes.GetIntValues(),
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this.reportsBuilder.GetRegistratedCasesDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult GetCaseTypeArticleNoReport(CaseTypeArticleNoOptionsModel options)
        {
            if (options.IsPrint)
            {
                var printData = this.reportService.GetCaseTypeArticleNoPrintData(
                                    this.OperationContext.CustomerId,
                                    options.DepartmentIds,
                                    options.WorkingGroupIds,
                                    options.CaseTypeIds,
                                    options.ProductAreaId,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    options.ShowCasesId);

                var print = this.printBuilder.GetCaseTypeArticleNoPrint(
                                    printData,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    options.ShowCasesId,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);

                return new UnicodeFileContentResult(print, this.printBuilder.GetPrintFileName(ReportType.CaseTypeArticleNo));
            }

            var data = this.reportService.GetCaseTypeArticleNoData(
                                    this.OperationContext.CustomerId,
                                    options.DepartmentIds,
                                    options.WorkingGroupIds,
                                    options.CaseTypeIds,
                                    options.ProductAreaId,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    options.ShowCasesId);

            if (options.IsExcel)
            {
                var excel = this.excelBuilder.GetCaseTypeArticleNoExcel(
                                    data,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);
                return new UnicodeFileContentResult(excel, this.excelBuilder.GetExcelFileName(ReportType.CaseTypeArticleNo));
            }

            var model = this.reportModelFactory.GetCaseTypeArticleNoModel(
                                    data,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);

            return this.PartialView("Reports/CaseTypeArticleNo", model);
        }

        [HttpGet]
        public FileContentResult GetReportImage(string objectId, string fileName)
        {
            return new FileContentResult(
                                this.reportsBuilder.GetReportImageFromCache(objectId, fileName),
                                MimeHelper.GetMimeType(fileName));
        }

        [HttpPost]
        public ActionResult CaseSatisfactionReport(CaseSatisfactionOptions options)
        {
            var model = this.reportModelFactory.CreateCaseSatisfactionReport(options, this.OperationContext);
            return this.View("Reports/CaseSatisfactionReport", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult GetReportGeneratorReport(ReportGeneratorOptionsModel options)
        {
            var filters = options != null
                        ? options.GetFilter()
                        : SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.OrdersOrders);

            SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);
            var data = this.reportService.GetReportGeneratorData(
                                this.OperationContext.CustomerId,
                                this.OperationContext.LanguageId,
                                filters.FieldIds,
                                filters.DepartmentIds,
                                filters.WorkingGroupIds,
                                filters.CaseTypeId,
                                filters.PeriodFrom,
                                filters.PeriodUntil,
                                string.Empty,
                                filters.SortField,
                                filters.RecordsOnPage);

            if (options != null && options.IsExcel)
            {
                var excel = this.excelBuilder.GetReportGeneratorExcel(data);
                return new UnicodeFileContentResult(excel, this.excelBuilder.GetExcelFileName(ReportType.ReportGenerator));
            }

            var model = this.reportGeneratorModelFactory.GetReportGeneratorModel(data, filters.SortField);

            return this.PartialView("Reports/ReportGenerator", model);
        }
    }
}
