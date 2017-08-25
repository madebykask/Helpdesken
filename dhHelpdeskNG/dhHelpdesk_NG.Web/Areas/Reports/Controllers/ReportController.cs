using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
using DH.Helpdesk.Web.Infrastructure.Attributes;

namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.IO;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator;
    using DH.Helpdesk.Web.Areas.Reports.Models.ReportService;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Shared;

    using Microsoft.Reporting.WebForms;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.BusinessData.Enums.Case;

    public sealed class ReportController : UserInteractionController
    {
        private readonly IReportModelFactory reportModelFactory;

        private readonly IReportGeneratorModelFactory reportGeneratorModelFactory;

        private readonly IReportService reportService;

        private readonly IReportsBuilder reportsBuilder;

        private readonly IPrintBuilder printBuilder;

        private readonly IExcelBuilder excelBuilder;

        private readonly ISettingService _customerSettingService;

        private readonly IReportServiceService _ReportServiceService;

        private const string _reportFolderName = "Reports";

        private readonly Dictionary<string, string> _reportTypeNames;

        public ReportController(
            IMasterDataService masterDataService,
            ISettingService customerSettingService,
            IReportModelFactory reportModelFactory,
            IReportService reportService,
            IReportsBuilder reportsBuilder,
            IPrintBuilder printBuilder,
            IExcelBuilder excelBuilder,
            IReportGeneratorModelFactory reportGeneratorModelFactory,
            IReportServiceService reportServiceService)
            : base(masterDataService)
        {
            this.reportModelFactory = reportModelFactory;
            this.reportService = reportService;
            this.reportsBuilder = reportsBuilder;
            this.printBuilder = printBuilder;
            this.excelBuilder = excelBuilder;
            this.reportGeneratorModelFactory = reportGeneratorModelFactory;
            this._customerSettingService = customerSettingService;
            this._ReportServiceService = reportServiceService;
            this._reportTypeNames = new Dictionary<string, string>
            {
                {"-1", "CasesPerCasetype"},
                {"-2", "CasesPerDate"},
                {"-3", "CasesPerSource"},
                {"-4", "CasesPerWorkingGroup"},
                {"-5", "CasesPerAdministrator"},
                {"-6", "CasesPerDepartment"},
                {"-7", "NumberOfCases"}
        };
        }

        [HttpGet]
        public ActionResult Index()
        {
			var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
			var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;

			var model = new ReportsOptions();
            var lastState = SessionFacade.ReportService ?? SessionFacade.ReportService;
            model.ReportServiceOverview = GetReportServiceModel(customerId, userId, lastState);

            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult ShowReport(string reportName, ReportFilterJSModel filter)
        {
            reportName = this._reportTypeNames[reportName];
            if (filter.RegisterTo.HasValue)
                filter.RegisterTo = filter.RegisterTo.Value.AddDays(1);

            var selectedReport = filter.MapToSelectedFilter();
            var model = GetReportViewerData(reportName, selectedReport);

            /* As we add virtually Dep/WG when they are not selected so we shouldn't save vistual values in the session */
            if (filter.Deps_OUs == null)
                selectedReport.SeletcedDepartments.ClearItems();

            if (filter.WorkingGroups == null)
                selectedReport.SelectedWorkingGroups.ClearItems();

            // Save state in session
            if (model != null)
                SessionFacade.ReportService = new ReportServiceSessionModel()
                                                {
                                                    ReportName = reportName,
                                                    SelectedFilter = selectedReport
                                                };

            return PartialView("ReportViewer/_PresentReport", model);
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
                    reportGeneratorOptions = TranslateReportFields(reportGeneratorOptions);

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

                case ReportType.FinishingCauseCategoryCustomer:
                    var finishingCauseCategoryCustomer = this.reportService.GetFinishingCauseCategoryCustomerOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/FinishingCauseCategoryCustomer",
                                this.reportModelFactory.GetFinishingCauseCategoryCustomerOptionsModel(finishingCauseCategoryCustomer));

                case ReportType.ClosedCasesDay:
                    var closedCasesDay = this.reportService.GetClosedCasesDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/ClosedCasesDay",
                                this.reportModelFactory.GetClosedCasesDayOptionsModel(closedCasesDay));

                case ReportType.CasesInProgressDay:
                    var casesInProgressDay = this.reportService.GetCasesInProgressDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/CasesInProgressDay",
                                this.reportModelFactory.GetCasesInProgressDayOptionsModel(casesInProgressDay));
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

        [HttpGet]
        [BadRequestOnNotValid]
        [NoCache]
        public ActionResult GetReportGeneratorReport(ReportGeneratorOptionsModel options)
        {
            try
            {
                var filters = options != null
                                        ? options.GetFilter()
                                        : SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator);

                SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, filters);


                if (options != null && options.IsPreview)
                {
                    var previewData = this.reportService.GetReportGeneratorAggregation(this.OperationContext.CustomerId,
                    this.OperationContext.UserId,
                    this.OperationContext.LanguageId,
                    filters.FieldIds,
                    filters.DepartmentIds.Where( d=> d > 0).ToList(),
                    filters.DepartmentIds.Where(d => d < 0).Select(d=> d * -1).ToList(),
                    filters.WorkingGroupIds,
                    filters.ProductAreaIds,
                    filters.AdministratorIds,
                    filters.CaseStatusIds,
                    filters.CaseTypeIds,
                    filters.PeriodFrom,
                    filters.PeriodUntil,
                    string.Empty,
                    filters.SortField,
                    filters.RecordsOnPage,
                    filters.CloseFrom,
                    filters.CloseTo);

                    return this.PartialView("Reports/ReportGeneratorPreview", new ReportGeneratorAggregateModel(previewData));
                }

                var data = this.reportService.GetReportGeneratorData(
                                    this.OperationContext.CustomerId,
                                    this.OperationContext.UserId,
                                    this.OperationContext.LanguageId,
                                    filters.FieldIds,
                                    filters.DepartmentIds.Where(d => d > 0).ToList(),
                                    filters.DepartmentIds.Where(d => d < 0).Select(d=> d * -1).ToList(),                                    
                                    filters.WorkingGroupIds,
                                    filters.ProductAreaIds,
                                    filters.AdministratorIds,
                                    filters.CaseStatusIds,
                                    filters.CaseTypeIds,
                                    filters.PeriodFrom,
                                    filters.PeriodUntil,
                                    string.Empty,
                                    filters.SortField,
                                    filters.RecordsOnPage,
                                    filters.CloseFrom,
                                    filters.CloseTo);

                var model = GetReportGeneratorModel(data, filters.SortField);

                if (options != null && options.IsExcel)
                {
                    var excel = this.excelBuilder.GetReportGeneratorExcel(model);
                    return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", this.excelBuilder.GetExcelFileName(ReportType.ReportGenerator));
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
                                            (CaseRegistrationSource)options.RegistrationSourceId,
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
                                            options.WorkingGroupId,
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

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetFinishingCauseCategoryCustomerReport(FinishingCauseCategoryCustomerOptionsModel options)
        {
            var data = this.reportService.GetFinishingCauseCategoryCustomerData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.WorkingGroupIds,
                                            options.CaseTypeId,
                                            options.PeriodFrom,
                                            options.PeriodUntil);

            var model = this.reportModelFactory.GetFinishingCauseCategoryCustomerModel(data);

            return this.PartialView("Reports/FinishingCauseCategoryCustomer", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public int SaveReportFilters(SaveReportFavoriteModel options)
        {
			var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
			var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;

			var favorite = new ReportFavorite();
            favorite.Customer_Id = customerId;
	        favorite.User_Id = userId;
            favorite.Filters = options.Filters;
            favorite.Name = options.Name;
            favorite.Type = options.OriginalReportId;
            favorite.Id = options.Id.HasValue ? options.Id.Value : 0;

            var id = this.reportService.SaveCustomerReportFavorite(favorite);

            return id;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void DeleteReportFavorite(int id)
        {
            var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
			var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;
			this.reportService.DeleteCustomerReportFavorite(id, customerId, userId);
        }

        [HttpGet]
        public JsonResult GetReportFilterOptions(int id)
        {
            var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
			var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;

			var favorite = this.reportService.GetCustomerReportFavorite(id, customerId, userId);
            var model = new ReportFavoriteModel();
            model.Id = favorite.Id;
            model.Filters = favorite.Filters;
            model.Name = favorite.Name;
            model.OriginalReportId = favorite.Type;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public UnicodeFileContentResult GetClosedCasesDayReport(
                                    string departments,
                                    int? caseType,
                                    int? workingGroup,
                                    int? administrator,
                                    DateTime? period)
        {
            var data = this.reportService.GetClosedCasesDayData(
                                    this.OperationContext.CustomerId,
                                    departments.GetIntValues().ToList(),
                                    caseType,
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this.reportsBuilder.GetClosedCasesDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        [HttpGet]
        public UnicodeFileContentResult GetCasesInProgressDayReport(
                                    int? department,
                                    int? workingGroup,
                                    int? administrator,
                                    DateTime? period)
        {
            var data = this.reportService.GetCasesInProgressDayData(
                                    this.OperationContext.CustomerId,
                                    department,
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this.reportsBuilder.GetCasesInProgressDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        public ReportGeneratorOptions TranslateReportFields(ReportGeneratorOptions reportOptions)
        {
            var translatedFields = new List<ItemOverview>();
            foreach (ItemOverview f in reportOptions.Fields)
                translatedFields.Add(new ItemOverview
                                            (
                                                Translation.Get(f.Name, Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id),
                                                f.Value
                                            ));


            var ret = new ReportGeneratorOptions
                            (
                                translatedFields.OrderBy(f => f.Name).ToList(),
                                reportOptions.Departments,
                                reportOptions.WorkingGroups,
                                reportOptions.CaseTypes.OrderBy(ct => ct.Name).ToList()
                            );
            return ret;
        }

        public ReportGeneratorModel GetReportGeneratorModel(ReportGeneratorData data, SortField sortfield)
        {
            var modelData = this.reportGeneratorModelFactory.GetReportGeneratorModel(data, sortfield);

            var translatedFields = new List<GridColumnHeaderModel>();
            foreach (var h in modelData.Headers)
                translatedFields.Add(new GridColumnHeaderModel
                                            (
                                                h.FieldName,
                                                Translation.Get(h.FieldName, Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id)
                                            ));


            var ret = new ReportGeneratorModel
                            (
                                translatedFields.ToList(),
                                modelData.Cases,
                                modelData.CasesFound,
                                modelData.SortField
                            );
            return ret;
        }


        public CustomSelectList GetCaseStateFilter()
        {
            var ret = new CustomSelectList();
            ret.Items.AddItem(CaseProgressFilter.None, string.Empty);
            ret.Items.AddItem(CaseProgressFilter.CasesInProgress, Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation));
            ret.Items.AddItem(CaseProgressFilter.ClosedCases, Translation.Get("Avslutade ärenden", Enums.TranslationSource.TextTranslation));
            return ret;
        }

        private ReportServiceOverviewModel GetReportServiceModel(int customerId, int userId, ReportServiceSessionModel lastState = null)
        {
			var reports = this.reportService.GetAvailableCustomerReports(customerId);
            var options = this.reportModelFactory.GetReportsOptions(reports);

            var model = new ReportServiceOverviewModel();
            model.CustomerId = SessionFacade.CurrentCustomer.Id;
            model.ReportFilter = GetReportFilterModel(model.CustomerId, lastState);
            model.ReportList = GetReportList(lastState != null ? lastState.ReportName : string.Empty, options.Reports);
            model.ReportFavorites = GetSavedReportFilters(customerId, userId);
            model.ReportViewerData = new ReportPresentationModel();
            model.ReportGeneratorOptions = GetReportGeneratorOptions(customerId);
            model.ReportGeneratorOptions.FieldIds = new List<int>();
            return model;
        }

        private ReportGeneratorOptionsModel GetReportGeneratorOptions(int customerId)
        {
            //TODO: this method gets too many options. Only Fields required. Remove other.
            var reportGeneratorFilters = SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator)
                             ?? ReportGeneratorFilterModel.CreateDefault();

            //SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, reportGeneratorFilters);
            var reportGeneratorOptions = this.reportService.GetReportGeneratorOptions(customerId,
                this.OperationContext != null ? this.OperationContext.LanguageId : SessionFacade.CurrentLanguageId);
            reportGeneratorOptions = TranslateReportFields(reportGeneratorOptions);

            return this.reportGeneratorModelFactory.GetReportGeneratorOptionsModel(reportGeneratorOptions, reportGeneratorFilters);
        }

        private ReportFilterModel GetReportFilterModel(int customerId, ReportServiceSessionModel lastState = null)
        {
            int curUserId = SessionFacade.CurrentUser.Id;
            var customerSettings = this._customerSettingService.GetCustomerSetting(customerId);

            var addOUToDep = (customerSettings != null && customerSettings.ShowOUsOnDepartmentFilter != 0) ? true : false;
            var reportFilter = _ReportServiceService.GetReportFilter(customerId, curUserId, addOUToDep);

            var model = new ReportFilterModel()
            {
                CaseCreationDate = reportFilter.CaseCreationDate,
                Administrators = reportFilter.Administrators,
                Departments = reportFilter.Departments,
                WorkingGroups = reportFilter.WorkingGroups,
                Selected = GetNewFilterSelections(),
                CaseTypes = reportFilter.CaseTypes,
                ProductAreas = reportFilter.ProductAreas,
                Status = GetCaseStateFilter(),
                UserNameOrientation = customerSettings != null ? customerSettings.IsUserFirstLastNameRepresentation : 1
            };

            if (lastState != null)
            {
                model.Selected = lastState.SelectedFilter;
            }

            return model;
        }

        private ReportSelectedFilter GetNewFilterSelections()
        {
            var selections = new ReportSelectedFilter();
            selections.CaseCreationDate = new DateToDate();
            return selections;
        }

        private CustomSelectList GetReportList(string defaultReportName, SelectList reports)
        {
            /* TODO: It must change some how find the files from "Reports" path */
            var newWord = Translation.GetCoreTextTranslation("Ny");
            var ret = new CustomSelectList();
            ret.Items.AddItem("-1", "CasesPerCasetype");
            ret.Items.AddItem("-2", "CasesPerDate");
            ret.Items.AddItem("-3", "CasesPerSource");
            ret.Items.AddItem("-4", "CasesPerWorkingGroup");
            ret.Items.AddItem("-5", "CasesPerAdministrator");
            ret.Items.AddItem("-6", "CasesPerDepartment");
            ret.Items.AddItem("-7", "NumberOfCases");            

            foreach (var customReport in reports)
            {
                ret.Items.AddItem(customReport.Value, customReport.Text);
            }

            var defaultSelected = ret.Items.FirstOrDefault(i => i.Value.ToLower() == defaultReportName.ToLower());
            if (defaultSelected != null)
                ret.SelectedItems.AddItem(int.Parse(defaultSelected.Id));

            return ret;
        }

        private List<SavedReportFavoriteItemModel> GetSavedReportFilters(int custometId, int userId)
        {
			var favorites = this.reportService.GetCustomerReportFavoriteList(custometId, userId);
            var list = favorites.Select(f => new SavedReportFavoriteItemModel
            {
                Id = f.Id,
                Name = f.Name,
                OriginalReportId = f.Type.ToString()
            }).ToList();

            return list;
        }

        private ReportPresentationModel GetReportViewerData(string reportName, ReportSelectedFilter reportSelectedFilter)
        {
            var reportData = _ReportServiceService.GetReportData(reportName, reportSelectedFilter, this.OperationContext.UserId,
                    OperationContext.CustomerId);

            ReportPresentationModel model = new ReportPresentationModel();

            if (reportData == null || (reportData != null && !reportData.DataSets.Any()))
            {
                model.ReportPage = null;
            }
            else
            {
                ReportViewer reportViewer = new ReportViewer();
                var basePath = Request.MapPath(Request.ApplicationPath);
                var fileLocation = Path.Combine(_reportFolderName, string.Format("{0}.rdl", reportData.ReportName));
                var reportFile = Path.Combine(basePath, fileLocation);
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowZoomControl = false;
                reportViewer.LocalReport.ReportPath = reportFile;
                
                ReportParameter rp1 = new ReportParameter("Category", "3", false);                
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(rp1);
                reportViewer.LocalReport.SetParameters(paramList);
                
                reportViewer.ShowParameterPrompts = true;
                foreach (var dataSet in reportData.DataSets)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSet.DataSetName, dataSet.DataSet));

                model.ReportPage = reportViewer;
            }

            return model;
        }


    }
}
