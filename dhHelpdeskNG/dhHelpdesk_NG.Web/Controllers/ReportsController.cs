namespace DH.Helpdesk.Web.Controllers
{
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
                    var response = this.reportsService.GetRegistratedCasesCaseTypeResponse(this.OperationContext);
                    var model = this.reportsModelFactory.CreateRegistratedCasesCaseTypeModel(response, this.OperationContext);
                    return this.PartialView("RegistratedCasesCaseType", model);
            }

            return null;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult RegistratedCasesCaseType(RegistratedCasesCaseTypeModel model)
        {
            string objectId;
            string fileName;
            if (!this.reportsHelper.CreateRegistratedCasesCaseTypeReport(model, out objectId, out fileName))
            {
                return new EmptyResult();
            }

            if (model.IsPrint)
            {
                fileName = this.reportsHelper.GetReportPathFromCache(objectId, fileName);
            }

            var response = this.reportsService.GetRegistratedCasesCaseTypeResponsePrint(
                                                    this.OperationContext,
                                                    model.WorkingGroupIds,
                                                    model.CaseTypeIds,
                                                    model.ProductAreaId);
            var reportModel = this.reportsModelFactory.CreateRegistratedCasesCaseTypeReportModel(
                                                    objectId,
                                                    fileName,
                                                    model,
                                                    response);

            if (model.IsPrint)
            {
                return new PrintPdfResult(reportModel, "RegistratedCasesCaseTypePrint");
            }

            return this.PartialView("RegistratedCasesCaseTypeReport", reportModel);
        }

        [HttpGet]
        public FileContentResult GetReportImage(string objectId, string fileName)
        {
            return new FileContentResult(
                                this.reportsHelper.GetReportImageFromCache(objectId, fileName),
                                MimeHelper.GetMimeType(fileName));
        }
    }
}