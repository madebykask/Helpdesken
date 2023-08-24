
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services.Cases;
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
	using BusinessData.Models.Case.CaseSettingsOverview;
    using BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public sealed class ReportController : UserInteractionController
    {
        private readonly IReportModelFactory _reportModelFactory;
        private readonly IReportGeneratorModelFactory _reportGeneratorModelFactory;
        private readonly IReportService _reportService;
        private readonly IReportsBuilder _reportsBuilder;
        private readonly IPrintBuilder _printBuilder;
        private readonly IExcelBuilder _excelBuilder;
        private readonly ISettingService _customerSettingService;
        private readonly IReportServiceService _reportServiceService;
        private readonly ICaseSectionService _caseSectionService;
        private readonly IFeatureToggleService _featureToggleService;
        private const string ReportFolderName = "Reports";

        private readonly Dictionary<string, string> _reportTypeNames;
		private IExtendedCaseService _extendedCaseService;

		public ReportController(
            IMasterDataService masterDataService,
            ISettingService customerSettingService,
            IReportModelFactory reportModelFactory,
            IReportService reportService,
            IReportsBuilder reportsBuilder,
            IPrintBuilder printBuilder,
            IExcelBuilder excelBuilder,
            IReportGeneratorModelFactory reportGeneratorModelFactory,
            IReportServiceService reportServiceService,
            ICaseSectionService caseSectionService,
            IFeatureToggleService featureToggleService,
			IExtendedCaseService extendedCaseService)
            : base(masterDataService)
        {
            _reportModelFactory = reportModelFactory;
            _reportService = reportService;
            _reportsBuilder = reportsBuilder;
            _printBuilder = printBuilder;
            _excelBuilder = excelBuilder;
            _reportGeneratorModelFactory = reportGeneratorModelFactory;
            _customerSettingService = customerSettingService;
            _reportServiceService = reportServiceService;
            _caseSectionService = caseSectionService;
            _featureToggleService = featureToggleService;
			_extendedCaseService = extendedCaseService;

			_reportTypeNames = new Dictionary<string, string>
            {
                //{"-1", "CasesPerCasetype"},
                //{"-2", "CasesPerDate"},
                //{"-3", "CasesPerSource"},
                //{"-4", "CasesPerWorkingGroup"},
                {"-5", "CasesPerAdministrator"},
                //{"-6", "CasesPerDepartment"},
                {"-7", "NumberOfCases"},
                {"-8", "AvgResolutionTime"},
                {"-9", "ReportedTime"}
            };

        }

        [HttpGet]
        [UserPermissions(UserPermission.ReportPermission)]
        public ActionResult Index()
        {
            var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
            var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;

            var model = new ReportsOptions();
            var lastState = SessionFacade.ReportService;


            model.ReportServiceOverview = GetReportServiceModel(customerId, userId, lastState);

            return View(model);
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
                    var registratedCasesDayOptions = this._reportService.GetRegistratedCasesDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/RegistratedCasesDay",
                                this._reportModelFactory.GetRegistratedCasesDayOptionsModel(registratedCasesDayOptions));

                case ReportType.CaseTypeArticleNo:
                    var caseTypeArticleNoOptions = this._reportService.GetCaseTypeArticleNoOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/CaseTypeArticleNo",
                                this._reportModelFactory.GetCaseTypeArticleNoOptionsModel(caseTypeArticleNoOptions));

                case ReportType.CaseSatisfaction:
                    return this.PartialView(
                                "Options/SurveySatisfactionOptions",
                                this._reportModelFactory.CreateCaseSatisfactionOptions(this.OperationContext));

                case ReportType.ReportGenerator:
                    var reportGeneratorFilters = SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator)
                                                 ?? ReportGeneratorFilterModel.CreateDefault();

                    SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, reportGeneratorFilters);
                    var reportGeneratorOptions = this._reportService.GetReportGeneratorOptions(this.OperationContext.CustomerId, this.OperationContext.LanguageId);
                    reportGeneratorOptions = TranslateReportFields(reportGeneratorOptions);

                    return this.PartialView(
                                "Options/ReportGenerator",
                                this._reportGeneratorModelFactory.GetReportGeneratorOptionsModel(reportGeneratorOptions, reportGeneratorFilters));

                case ReportType.LeadtimeFinishedCases:
                    var leadtimeFinishedCases = this._reportService.GetLeadtimeFinishedCasesOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/LeadtimeFinishedCases",
                                this._reportModelFactory.GetLeadtimeFinishedCasesOptionsModel(leadtimeFinishedCases));

                case ReportType.LeadtimeActiveCases:
                    var leadtimeActiveCases = this._reportService.GetLeadtimeActiveCasesOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/LeadtimeActiveCases",
                                this._reportModelFactory.GetLeadtimeActiveCasesOptionsModel(leadtimeActiveCases));

                case ReportType.FinishingCauseCustomer:
                    var finishingCauseCustomer = this._reportService.GetFinishingCauseCustomerOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/FinishingCauseCustomer",
                                this._reportModelFactory.GetFinishingCauseCustomerOptionsModel(finishingCauseCustomer));

                case ReportType.FinishingCauseCategoryCustomer:
                    var finishingCauseCategoryCustomer = this._reportService.GetFinishingCauseCategoryCustomerOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/FinishingCauseCategoryCustomer",
                                this._reportModelFactory.GetFinishingCauseCategoryCustomerOptionsModel(finishingCauseCategoryCustomer));

                case ReportType.ClosedCasesDay:
                    var closedCasesDay = this._reportService.GetClosedCasesDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/ClosedCasesDay",
                                this._reportModelFactory.GetClosedCasesDayOptionsModel(closedCasesDay));

                case ReportType.CasesInProgressDay:
                    var casesInProgressDay = this._reportService.GetCasesInProgressDayOptions(this.OperationContext.CustomerId);
                    return this.PartialView(
                                "Options/CasesInProgressDay",
                                this._reportModelFactory.GetCasesInProgressDayOptionsModel(casesInProgressDay));
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
            var data = this._reportService.GetRegistratedCasesDayData(
                                    this.OperationContext.CustomerId,
                                    department,
                                    caseTypes.GetIntValues(),
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this._reportsBuilder.GetRegistratedCasesDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult GetCaseTypeArticleNoReport(CaseTypeArticleNoOptionsModel options)
        {
            if (options.IsPrint)
            {
                var printData = this._reportService.GetCaseTypeArticleNoPrintData(
                                    this.OperationContext.CustomerId,
                                    options.DepartmentIds,
                                    options.WorkingGroupIds,
                                    options.CaseTypeIds,
                                    options.ProductAreaId,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    options.ShowCasesId);

                var print = this._printBuilder.GetCaseTypeArticleNoPrint(
                                    printData,
                                    options.PeriodFrom,
                                    options.PeriodUntil,
                                    options.ShowCasesId,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);

                return new UnicodeFileContentResult(print, this._printBuilder.GetPrintFileName(ReportType.CaseTypeArticleNo));
            }

            var data = this._reportService.GetCaseTypeArticleNoData(
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
                var excel = this._excelBuilder.GetCaseTypeArticleNoExcel(
                                    data,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);
                return new UnicodeFileContentResult(excel, this._excelBuilder.GetExcelFileName(ReportType.CaseTypeArticleNo));
            }

            var model = this._reportModelFactory.GetCaseTypeArticleNoModel(
                                    data,
                                    options.IsShowCaseTypeDetails,
                                    options.IsShowPercents);

            return this.PartialView("Reports/CaseTypeArticleNo", model);
        }

        [HttpGet]
        public FileContentResult GetReportImage(string objectId, string fileName)
        {
            return new FileContentResult(
                                this._reportsBuilder.GetReportImageFromCache(objectId, fileName),
                                MimeHelper.GetMimeType(fileName));
        }

        [HttpPost]
        public ActionResult CaseSatisfactionReport(CaseSatisfactionOptions options)
        {
            var model = this._reportModelFactory.CreateCaseSatisfactionReport(options, this.OperationContext);
            return this.View("Reports/CaseSatisfactionReport", model);
        }

        [HttpPost]
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

                
                var selectedReport = filters.MapToSelectedFilter(filters);

                SessionFacade.ReportService = new ReportServiceSessionModel()
                {
                    ReportName = options.ReportName,
                    SelectedFilter = selectedReport
                };


                if (options != null && options.IsPreview)
                {
                    var previewData = this._reportService.GetReportGeneratorAggregation(this.OperationContext.CustomerId,
                    this.OperationContext.UserId,
                    this.OperationContext.LanguageId,
                    filters.FieldIds,
                    filters.DepartmentIds.Where(d => d > 0).ToList(),
                    filters.DepartmentIds.Where(d => d < 0).Select(d => d * -1).ToList(),
                    filters.WorkingGroupIds,
                    filters.ProductAreaIds,
                    filters.AdministratorIds,
                    filters.CaseStatusIds,
                    filters.CaseTypeIds,
					filters.ExtendedCaseFormFieldIds,
					filters.ExtendedCaseFormId,
                    filters.PeriodFrom,
                    filters.PeriodUntil,
                    string.Empty,
                    filters.SortField,
                    filters.RecordsOnPage,
                    filters.CloseFrom,
                    filters.CloseTo);

                    return this.PartialView("Reports/ReportGeneratorPreview", new ReportGeneratorAggregateModel(previewData));
                }

				var languageId = SessionFacade.CurrentLanguageId;

                var data = this._reportService.GetReportGeneratorData(
                                    this.OperationContext.CustomerId,
                                    this.OperationContext.UserId,
                                    this.OperationContext.LanguageId,
                                    filters.FieldIds,
                                    filters.DepartmentIds.Where(d => d > 0).ToList(),
                                    filters.DepartmentIds.Where(d => d < 0).Select(d => d * -1).ToList(),
                                    filters.WorkingGroupIds,
                                    filters.ProductAreaIds,
                                    filters.AdministratorIds,
                                    filters.CaseStatusIds,
                                    filters.CaseTypeIds,
									filters.ExtendedCaseFormFieldIds,
									filters.ExtendedCaseFormId,
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
                    var excel = this._excelBuilder.GetReportGeneratorExcel(model);
                    return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", this._excelBuilder.GetExcelFileName(ReportType.ReportGenerator));
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
            var data = this._reportService.GetLeadtimeFinishedCasesData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.CaseTypeId,
                                            options.WorkingGroupIds,
                                            (CaseRegistrationSource)options.RegistrationSourceId,
                                            options.PeriodFrom,
                                            options.PeriodUntil,
                                            options.LeadTimeId,
                                            options.IsShowDetails);

            var model = this._reportModelFactory.GetLeadtimeFinishedCasesModel(data, options.IsShowDetails);

            return this.PartialView("Reports/LeadtimeFinishedCases", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetLeadtimeActiveCasesReport(LeadtimeActiveCasesOptionsModel options)
        {
            const int HighHours = 2;
            const int MediumDays = 2;
            const int LowDays = 5;

            var data = this._reportService.GetLeadtimeActiveCasesData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.CaseTypeId,
                                            HighHours,
                                            MediumDays,
                                            LowDays);

            var model = this._reportModelFactory.GetLeadtimeActiveCasesModel(
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
            var data = this._reportService.GetFinishingCauseCustomerData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.WorkingGroupId,
                                            options.CaseTypeId,
                                            options.AdministratorId,
                                            options.PeriodFrom,
                                            options.PeriodUntil);

            var model = this._reportModelFactory.GetFinishingCauseCustomerModel(data, this.OperationContext.CustomerId);

            if (options.IsExcel)
            {
                var excel = this._excelBuilder.GetFinishingCauseCustomerExcel(model);
                return new UnicodeFileContentResult(excel, this._excelBuilder.GetExcelFileName(ReportType.FinishingCauseCustomer));
            }

            return this.PartialView("Reports/FinishingCauseCustomer", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult GetFinishingCauseCategoryCustomerReport(FinishingCauseCategoryCustomerOptionsModel options)
        {
            var data = this._reportService.GetFinishingCauseCategoryCustomerData(
                                            this.OperationContext.CustomerId,
                                            options.DepartmentIds,
                                            options.WorkingGroupIds,
                                            options.CaseTypeId,
                                            options.PeriodFrom,
                                            options.PeriodUntil);

            var model = this._reportModelFactory.GetFinishingCauseCategoryCustomerModel(data);

            return this.PartialView("Reports/FinishingCauseCategoryCustomer", model);
        }

		[HttpPost]
		[BadRequestOnNotValid]
		public JsonResult GetFieldsForExtendedCaseForm(int extendedCaseFormId)
		{
			var languageID = SessionFacade.CurrentLanguageId;
			var fields = _extendedCaseService.GetExtendedCaseFormFields(extendedCaseFormId, languageID);
			var sections = _extendedCaseService.GetExtendedCaseFormSections(extendedCaseFormId, languageID);

            if (fields != null && sections != null)
            {
                foreach (var field in fields)
                {
                    foreach (var section in sections)
                    {
                        if (field.FieldId == section.SectionId)
                        {
                           field.Text = section.Text + " - " + field.Text;                           
                        }                            
                    }
                }
            }

            fields = fields.OrderBy(f => f.Text).ToList();
            return Json(fields);
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

            var id = this._reportService.SaveCustomerReportFavorite(favorite);

            return id;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void DeleteReportFavorite(int id)
        {
            var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
            var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;
            this._reportService.DeleteCustomerReportFavorite(id, customerId, userId);
        }

        [HttpGet]
        public JsonResult GetReportFilterOptions(int id)
        {
            var customerId = OperationContext?.CustomerId ?? SessionFacade.CurrentCustomer.Id;
            var userId = OperationContext?.UserId ?? SessionFacade.CurrentUser.Id;

            var favorite = this._reportService.GetCustomerReportFavorite(id, customerId, userId);
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
            var data = this._reportService.GetClosedCasesDayData(
                                    this.OperationContext.CustomerId,
                                    departments.GetIntValues().ToList(),
                                    caseType,
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this._reportsBuilder.GetClosedCasesDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        [HttpGet]
        public UnicodeFileContentResult GetCasesInProgressDayReport(
                                    int? department,
                                    int? workingGroup,
                                    int? administrator,
                                    DateTime? period)
        {
            var data = this._reportService.GetCasesInProgressDayData(
                                    this.OperationContext.CustomerId,
                                    department,
                                    workingGroup,
                                    administrator,
                                    period.RoundToMonthOrGetCurrent());

            var report = this._reportsBuilder.GetCasesInProgressDayReport(data, period.RoundToMonthOrGetCurrent());

            return new UnicodeFileContentResult(report, string.Empty);
        }

        public ReportGeneratorOptions TranslateReportFields(ReportGeneratorOptions reportOptions)
        {
            var translatedFields = new List<ItemOverview>();
            foreach (var f in reportOptions.Fields)
            {
                var sectionInfo = _caseSectionService.GetSectionInfoByField(f.Name);
                var caseSectionName = sectionInfo != null ? Translation.GetCoreTextTranslation(sectionInfo.DefaultName) : "";
                var fieldName = string.IsNullOrEmpty(caseSectionName) ? Translation.GetCoreTextTranslation(f.Name) : Translation.GetForCase(f.Name, SessionFacade.CurrentCustomer.Id);
                translatedFields.Add(new ItemOverview
                (
                    fieldName,
                    f.Value
                ));
            }

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
            var modelData = this._reportGeneratorModelFactory.GetReportGeneratorModel(data, sortfield);

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
            ret.Items.AddItem(CaseProgressFilter.CasesInProgress, Translation.GetCoreTextTranslation("Pågående ärenden"));
            ret.Items.AddItem(CaseProgressFilter.ClosedCases, Translation.GetCoreTextTranslation("Avslutade ärenden"));
            return ret;
        }

        private ReportServiceOverviewModel GetReportServiceModel(int customerId, int userId, ReportServiceSessionModel lastState = null)
        {
            var reports = this._reportService.GetAvailableCustomerReports(customerId);
            var options = this._reportModelFactory.GetReportsOptions(reports);

            var model = new ReportServiceOverviewModel();
            model.CustomerId = SessionFacade.CurrentCustomer.Id;
            model.ReportFilter = GetReportFilterModel(model.CustomerId, lastState);
            model.ReportList = GetReportList(lastState != null ? lastState.ReportName : string.Empty, options.Reports);
            model.ReportFavorites = GetSavedReportFilters(customerId, userId);
            model.ReportViewerData = new ReportPresentationModel();
            model.ReportGeneratorOptions = GetReportGeneratorOptions(customerId);
			model.ReportGeneratorExtendedCaseForms = GetReportGeneratorExtendedCaseForms(customerId);
			model.ReportGeneratorExtendedCaseFormFields = new List<DH.Helpdesk.Web.Areas.Reports.Models.ReportService.ExtendedCaseFormFieldTranslationModel>();
			model.ReportGeneratorOptions.FieldIds = new List<int>();
			model.ReportGeneratorOptions.ExtendedCaseTranslationFieldIds = new List<string>();
			return model;
        }

		private List<ExtendedCaseCaseSolutionModel> GetReportGeneratorExtendedCaseForms(int customerId)
		{
			var reports = _extendedCaseService.GetExtendedCaseFormsWithCaseSolutionForCustomer(customerId);
			var reportModels =  reports.Select(o => 
			new ExtendedCaseCaseSolutionModel
			{
				Id = o.Id,
				Name = o.CaseSolutions
					.Select(cs => cs.CaseSolutionCategoryName != null ? cs.CaseSolutionCategoryName + " - " + cs.Name : cs.Name)
					.Aggregate((a,b) => a + ", " + b),
				HasInactiveCaseSolutions = o.CaseSolutions.Any(cs => cs.Status != 1)
			})
			.OrderBy(o => o.Name)
			.ToList();

			return reportModels;
		}

		private ReportGeneratorOptionsModel GetReportGeneratorOptions(int customerId)
        {
            //TODO: this method gets too many options. Only Fields required. Remove other.
            var reportGeneratorFilters = SessionFacade.FindPageFilters<ReportGeneratorFilterModel>(PageName.ReportsReportGenerator)
                             ?? ReportGeneratorFilterModel.CreateDefault();

            //SessionFacade.SavePageFilters(PageName.ReportsReportGenerator, reportGeneratorFilters);
            var reportGeneratorOptions = this._reportService.GetReportGeneratorOptions(customerId,
                this.OperationContext != null ? this.OperationContext.LanguageId : SessionFacade.CurrentLanguageId);
            reportGeneratorOptions = TranslateReportFields(reportGeneratorOptions);

            return this._reportGeneratorModelFactory.GetReportGeneratorOptionsModel(reportGeneratorOptions, reportGeneratorFilters);
        }

        private ReportFilterModel GetReportFilterModel(int customerId, ReportServiceSessionModel lastState = null)
        {
            int curUserId = SessionFacade.CurrentUser.Id;
            var customerSettings = this._customerSettingService.GetCustomerSetting(customerId);

            var addOUToDep = (customerSettings != null && customerSettings.ShowOUsOnDepartmentFilter != 0) ? true : false;
            var reportFilter = _reportServiceService.GetReportFilter(customerId, curUserId, addOUToDep);
            var stackByList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Translation.GetForCase(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                        SessionFacade.CurrentCustomer.Id),
                    Value = ((int)GlobalEnums.TranslationCaseFields.CaseType_Id).ToString()
                }
            };
            var groupByList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = Translation.GetForCase(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                        SessionFacade.CurrentCustomer.Id),
                    Value = ((int)GlobalEnums.TranslationCaseFields.WorkingGroup_Id).ToString()
                }
            };
            var firstUserNameOrientation = customerSettings?.IsUserFirstLastNameRepresentation.ToBool() ?? false;
            var model = new ReportFilterModel()
            {
                CaseCreationDate = reportFilter.CaseCreationDate,
                Administrators = firstUserNameOrientation ? reportFilter.Administrators : reportFilter.Administrators.OrderBy(a => a.SurName).ToList(),
                Departments = reportFilter.Departments,
                WorkingGroups = reportFilter.WorkingGroups,
                Selected = GetNewFilterSelections(),
                CaseTypes = reportFilter.CaseTypes,
                ProductAreas = reportFilter.ProductAreas,
                Status = GetCaseStateFilter(),
                FirstUserNameOrientation = firstUserNameOrientation,
                ReportCategory = GetReportGroups(),
                ReportCategoryRt = GetReportGroupsRt(),
                ReportCategorySolvedInTime = GetReportGroupsSolvedInTime(),
                StackByList = stackByList,
                GroupByList = groupByList
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
            var ret = new CustomSelectList();
            var reportedTimeKey =
                !_featureToggleService.IsActive(Helpdesk.Common.Constants.FeatureToggleTypes.USE_DEPRICATED_REPORTED_TIME_REPORT)
                    ? "25"
                    : "-9";
            var numberOfCasesKey = 
                !_featureToggleService.IsActive(Helpdesk.Common.Constants.FeatureToggleTypes.USE_DEPRICATED_NUMBER_OF_CASES_REPORT)
                    ? "26"
                    : "-7";

            var oldReports = new List<KeyValuePair<string, string>>()
            {
                //new KeyValuePair<string, string>("-1", "CasesPerCasetype"),
                //new KeyValuePair<string, string>("-2", "CasesPerDate"),
                //new KeyValuePair<string, string>("-3", "CasesPerSource"),
                //new KeyValuePair<string, string>("-4", "CasesPerWorkingGroup"),
                new KeyValuePair<string, string>("-5", "CasesPerAdministrator")
                //,new KeyValuePair<string, string>("-6", "CasesPerDepartment")
            };

            var newReports = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(numberOfCasesKey, "NumberOfCases"),
                new KeyValuePair<string, string>("-8", "AvgResolutionTime"),
                new KeyValuePair<string, string>(reportedTimeKey, "ReportedTime"),
                new KeyValuePair<string, string>("24", "HistoricalReport"),
                new KeyValuePair<string, string>("27", "SolvedInTimeReport"),
            };

            // List new report first (order by name) then old reports (order by name)
            var listItems = newReports
                .OrderBy(o => o.Value)
                .Concat(oldReports.OrderBy(o => o.Value))
                .Select(o => new ListItem(o.Key, o.Value, false))
                .ToList();

            ret.Items.AddItems(listItems);

            foreach (var customReport in reports.OrderBy(o => o.Text))
            {
                ret.Items.AddItem(customReport.Value, customReport.Text);
            }

            if (!String.IsNullOrEmpty(defaultReportName))
            {
                var defaultSelected = ret.Items.FirstOrDefault(i => i.Value.ToLower() == defaultReportName.ToLower());
                if (defaultSelected != null)
                    ret.SelectedItems.AddItem(int.Parse(defaultSelected.Id));

            }


            return ret;
        }

        private List<SavedReportFavoriteItemModel> GetSavedReportFilters(int custometId, int userId)
        {
            var favorites = this._reportService.GetCustomerReportFavoriteList(custometId, userId);
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
            var reportData = _reportServiceService.GetReportData(reportName, reportSelectedFilter, this.OperationContext.UserId,
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
                var fileLocation = Path.Combine(ReportFolderName, string.Format("{0}.rdl", reportData.ReportName));
                var reportFile = Path.Combine(basePath, fileLocation);
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowZoomControl = false;
                reportViewer.LocalReport.ReportPath = reportFile;

                /*Temp solution*/
                if (reportName == "NumberOfCases" || reportName == "ReportedTime")
                {
                    var selectedReportCategory = reportName == "ReportedTime"
                        ? reportSelectedFilter.SelectedReportCategoryRt.GetFirstOrDefaultSelected()
                        : reportSelectedFilter.SelectedReportCategory.GetFirstOrDefaultSelected();
                    var itemId = selectedReportCategory != null ? selectedReportCategory.Value.ToString() : "1";
                    var categoryParam = new ReportParameter("Category", itemId, false);
                    var paramList = new List<ReportParameter> { categoryParam };
                    reportViewer.LocalReport.SetParameters(paramList);
                }

                foreach (var dataSet in reportData.DataSets)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSet.DataSetName, dataSet.DataSet));

                model.ReportPage = reportViewer;
            }

            return model;
        }

        private List<ListItem> GetReportGroups()
        {
            //_reportCategoriesRt = new CustomSelectList();

            return new[]
            {
                new ListItem("1", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()), false),
                new ListItem("8", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()), false),
                new ListItem("9", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()), false),
                new ListItem("10", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Department_Id.ToString()), false),
                new ListItem("11", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()), false),
                new ListItem("13", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()), false),
                new ListItem("12", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()), false),
                new ListItem("7", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()), false),
                new ListItem("5", GetReportFormTranslation(ReportItemNames.RegistrationDate), false),
                new ListItem("2", GetReportFormTranslation(ReportItemNames.RegistrationYear), false),
                new ListItem("4", GetReportFormTranslation(ReportItemNames.RegistrationMonth), false),
                new ListItem("3", GetReportFormTranslation(ReportItemNames.RegistrationWeekday), false),
                new ListItem("6", GetReportFormTranslation(ReportItemNames.RegistrationHour), false)
            }.OrderBy(l => l.Value)
                .ToList();
        }

        private List<ListItem> GetReportGroupsRt()
        {
            return new[]
            {
                new ListItem("1", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()), false),
                new ListItem("2", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.CaseNumber.ToString()), false),
                new ListItem("3", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Department_Id.ToString()), false),
                new ListItem("4", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()), false),
                new ListItem("5", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()), false),
                new ListItem("6", GetReportFormTranslation(ReportItemNames.LogNoteDate), false),
                new ListItem("7", GetReportFormTranslation(ReportItemNames.RegisteredBy.ToString()), false),
                //new ListItem("8", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()), false)
            }.OrderBy(l => l.Value)
                .ToList();
        }

        private List<ListItem> GetReportGroupsSolvedInTime()
        {
            return new[]
            {
                new ListItem("1", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()), false),
                new ListItem("3", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Department_Id.ToString()), false),
                new ListItem("4", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()), false),
                new ListItem("5", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()), false),
                new ListItem("7", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()), false),
                new ListItem("8", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()), false),
                new ListItem("9", GetReportFormTranslation(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()), false),
                new ListItem("14", GetReportFormTranslation(ReportItemNames.FinishingMonth), false),
                new ListItem("15", GetReportFormTranslation(ReportItemNames.FinishingYear), false),

            }.OrderBy(l => l.Value)
                .ToList();
        }

        private static string GetReportFormTranslation(string value)
        {
            if (value.Equals(ReportItemNames.RegistrationDate))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegistrationDate);
            if (value.Equals(ReportItemNames.RegistrationYear))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegistrationYear);
            if (value.Equals(ReportItemNames.RegistrationMonth))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegistrationMonth);
            if (value.Equals(ReportItemNames.RegistrationWeekday))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegistrationWeekday);
            if (value.Equals(ReportItemNames.RegistrationHour))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegistrationHour);
            if (value.Equals(ReportItemNames.LogNoteDate))
                return Translation.GetMasterDataTranslation(ReportItemNames.LogNoteDate);
            if (value.Equals(ReportItemNames.FinishingMonth))
                return Translation.GetMasterDataTranslation(ReportItemNames.FinishingMonth);
            if (value.Equals(ReportItemNames.FinishingYear))
                return Translation.GetMasterDataTranslation(ReportItemNames.FinishingYear);            
            if (value.Equals(ReportItemNames.RegisteredBy))
                return Translation.GetMasterDataTranslation(ReportItemNames.RegisteredBy);


            var defaultValue = string.Empty;
            if (value.Equals(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()))
                defaultValue = "Case Type";
            if (value.Equals(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()))
                defaultValue = "Working Group";
            if (value.Equals(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
                defaultValue = "SubStatus";
            if (value.Equals(GlobalEnums.TranslationCaseFields.Department_Id.ToString()))
                defaultValue = "Department";
            if (value.Equals(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()))
                defaultValue = "Priority";
            if (value.Equals(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()))
                defaultValue = "Product Area";
            if (value.Equals(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()))
                defaultValue = "Closing Date";
            if (value.Equals(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()))
                defaultValue = "Source";
            if (value.Equals(GlobalEnums.TranslationCaseFields.CaseNumber.ToString()))
                defaultValue = "Case Number";
            if (value.Equals(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()))
                defaultValue = "Administrator";
            var translation = Translation.GetForCase(value, SessionFacade.CurrentCustomer.Id);
            return !string.IsNullOrEmpty(translation) ? translation : defaultValue;
        }

    }
}
