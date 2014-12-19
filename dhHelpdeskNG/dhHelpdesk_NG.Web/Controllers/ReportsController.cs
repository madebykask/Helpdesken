namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports;
    using DH.Helpdesk.Web.Infrastructure.Print;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Reports;

    public sealed class ReportsController : UserInteractionController
    {
        private readonly IReportsModelFactory reportsModelFactory;

        private readonly IReportsService reportsService;

        private readonly IReportsHelper reportsHelper;

        public ReportsController(
            IMasterDataService masterDataService, 
            IReportsModelFactory reportsModelFactory, 
            IReportsService reportsService, 
            IReportsHelper reportsHelper)
            : base(masterDataService)
        {
            this.reportsModelFactory = reportsModelFactory;
            this.reportsService = reportsService;
            this.reportsHelper = reportsHelper;
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

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult ReportsContent(SearchModel searchModel)
        {
            var filters = searchModel != null
                  ? searchModel.ExtractFilters()
                  : SessionFacade.FindPageFilters<ReportsFilter>(PageName.Reports);

            SessionFacade.SavePageFilters(PageName.Reports, filters);

            switch ((ReportType)filters.ReportId)
            {
                case ReportType.RegistratedCasesCaseType:                    
                    return this.PartialView(
                                "RegistratedCasesCaseTypeOptions", 
                                this.reportsModelFactory.CreateRegistratedCasesCaseTypeOptions(this.OperationContext));
                case ReportType.RegistratedCasesDay:                    
                    return this.PartialView(
                                "RegistratedCasesDayOptions", 
                                this.reportsModelFactory.CreateRegistratedCasesDayOptions(this.OperationContext));
                case ReportType.AverageSolutionTime:                    
                    return this.PartialView(
                                "AverageSolutionTimeOptions",
                                this.reportsModelFactory.CreateAverageSolutionTimeOptions(this.OperationContext));
                case ReportType.CaseSatisfaction:
                    return this.PartialView(
                                "SurveySatisfactionOptions",
                                this.reportsModelFactory.CreateCaseSatisfactionOptions(this.OperationContext));
            }

            return null;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult RegistratedCasesCaseType(RegistratedCasesCaseTypeOptions options)
        {
            var model = this.reportsModelFactory.CreateRegistratedCasesCaseTypeReport(options, this.OperationContext);

            if (options.IsPrint)
            {
                return new PrintPdfResult(model, "RegistratedCasesCaseTypePrint");
            }

            return this.PartialView("RegistratedCasesCaseTypeView", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult RegistratedCasesDay(RegistratedCasesDayOptions options)
        {
            var model = this.reportsModelFactory.CreateRegistratedCasesDayReport(options, this.OperationContext);

            if (options.IsPrint)
            {
                return new PrintPdfResult(model, "RegistratedCasesDayPrint");
            }

            return this.PartialView("RegistratedCasesDayView", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult AverageSolutionTime(AverageSolutionTimeOptions options)
        {
            var model = this.reportsModelFactory.CreateAverageSolutionTimeReport(options, this.OperationContext);

            if (options.IsPrint)
            {
                return new PrintPdfResult(model, "AverageSolutionTimePrint");
            }

            return this.PartialView("AverageSolutionTimeView", model);
        }

        [HttpGet]
        public FileContentResult GetReportImage(string objectId, string fileName)
        {
            return new FileContentResult(
                                this.reportsHelper.GetReportImageFromCache(objectId, fileName),
                                MimeHelper.GetMimeType(fileName));
        }

        [HttpPost]
        public ActionResult CaseSatisfactionReport(CaseSatisfactionOptions options)
        {
            var model = this.reportsModelFactory.CreateCaseSatisfactionReport(options, this.OperationContext);
            return this.View("CaseSatisfactionReport", model);
        }
    }
}