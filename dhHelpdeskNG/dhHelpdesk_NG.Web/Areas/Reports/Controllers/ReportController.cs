namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.OldComponents;
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
            int customerId = this.OperationContext != null
                                 ? this.OperationContext.CustomerId
                                 : SessionFacade.CurrentCustomer.Id;

            var reports = this.reportService.GetAvailableCustomerReports(customerId);
            var model = this.reportModelFactory.GetReportsOptions(reports);
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

                case ReportType.LeadtimeFinishedCases:
                    var leadtimeFinishedCases = this.reportService.GetLeadtimeFinishedCasesOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/LeadtimeFinishedCases",
                                this.reportModelFactory.GetLeadtimeFinishedCasesOptionsModel(leadtimeFinishedCases));

                case ReportType.LeadtimeActiveCases:
                    var leadtimeActiveCases = this.reportService.GetLeadtimeActiveCasesOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/LeadtimeActiveCases",
                                this.reportModelFactory.GetLeadtimeActiveCasesOptionsModel(leadtimeActiveCases));

                case ReportType.FinishingCauseCustomer:
                    var finishingCauseCustomer = this.reportService.GetFinishingCauseCustomerOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/FinishingCauseCustomer",
                                this.reportModelFactory.GetFinishingCauseCustomerOptionsModel(finishingCauseCustomer));
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
            try
            {
                var filters = options != null
                                        ? options.GetFilter()
                                        : SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator);

                SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, filters);
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

                var model = this.reportGeneratorModelFactory.GetReportGeneratorModel(data, filters.SortField);

                if (options != null && options.IsExcel)
                {
                    var excel = this.excelBuilder.GetReportGeneratorExcel(model);
                    return new UnicodeFileContentResult(excel, this.excelBuilder.GetExcelFileName(ReportType.ReportGenerator));
                }

                return this.PartialView("Reports/ReportGenerator", model);
            }
            catch (Exception)
            {
                SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, ReportGeneratorFilterModel.CreateDefault());
                throw;
            }            
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetLeadtimeFinishedCasesReport(LeadtimeFinishedCasesOptionsModel options)
        {
            var data = this.reportService.GetLeadtimeFinishedCasesData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.CaseTypeId,
                                            options.WorkingGroupIds,
                                            (GlobalEnums.RegistrationSource)options.RegistrationSourceId,
                                            options.PeriodFrom,
                                            options.PeriodUntil,
                                            options.LeadTimeId,
                                            options.IsShowDetails);

            var model = this.reportModelFactory.GetLeadtimeFinishedCasesModel(data, options.IsShowDetails);

            return this.PartialView("Reports/LeadtimeFinishedCases", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetLeadtimeActiveCasesReport(LeadtimeActiveCasesOptionsModel options)
        {
            const int HighHours = 2;
            const int MediumDays = 2;
            const int LowDays = 5;

            var data = this.reportService.GetLeadtimeActiveCasesData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.CaseTypeId,
                                            HighHours, 
                                            MediumDays, 
                                            LowDays);

            var model = this.reportModelFactory.GetLeadtimeActiveCasesModel(
                                            data,
                                            HighHours,
                                            MediumDays,
                                            LowDays);

            return this.PartialView("Reports/LeadtimeActiveCases", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult GetFinishingCauseCustomerReport(FinishingCauseCustomerOptionsModel options)
        {
            var data = this.reportService.GetFinishingCauseCustomerData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.WorkingGroupIds,
                                            options.CaseTypeId,
                                            options.AdministratorId,
                                            options.PeriodFrom,
                                            options.PeriodUntil);

            var model = this.reportModelFactory.GetFinishingCauseCustomerModel(data, this.OperationContext.CustomerId);

            if (options.IsExcel)
            {
                var excel = this.excelBuilder.GetFinishingCauseCustomerExcel(model);
                return new UnicodeFileContentResult(excel, this.excelBuilder.GetExcelFileName(ReportType.FinishingCauseCustomer));
            }

            return this.PartialView("Reports/FinishingCauseCustomer", model);
        }
    }
}
