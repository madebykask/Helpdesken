﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Services.Utils;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Case;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions;
    using DH.Helpdesk.Web.Infrastructure.Grid;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Case.ChildCase;
    using DH.Helpdesk.Web.Models.Case.Input;
    using DH.Helpdesk.Web.Models.Case.Output;
    using DH.Helpdesk.Web.Models.CaseLock;
    using DH.Helpdesk.Web.Models.Shared;
    using DH.Helpdesk.Common.Extensions.DateTime;

    using Org.BouncyCastle.Bcpg;

    using DHDomain = DH.Helpdesk.Domain;
    using ParentCaseInfo = DH.Helpdesk.BusinessData.Models.Case.ChidCase.ParentCaseInfo;
    using DH.Helpdesk.Web.Enums;
    using System.Web.Script.Serialization;
    using Microsoft.Reporting.WebForms;
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.Enums.CaseSolution;

    public class CasesController : BaseController
    {        
        #region ***Constant/Variables***

        private const string ParentPathDefaultValue = "--";
        private const string ChildCasesHashTab = "childcases-tab";
        private const string _reportFolderName = "StaticReports";
        private const int MaxTextCharCount = 200;

        private readonly ICaseService _caseService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseFileService _caseFileService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICategoryService _categoryService;
        private readonly IComputerService _computerService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IDepartmentService _departmentService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly IImpactService _impactService;
        private readonly IOUService _ouService;
        private readonly IProblemService _problemService;
        private readonly IPriorityService _priorityService;
        private readonly IProductAreaService _productAreaService;
        private readonly IRegionService _regionService;
        private readonly ISettingService _settingService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IStatusService _statusService;
        private readonly ISupplierService _supplierService;
        private readonly IStandardTextService _standardTextService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IProjectService _projectService;
        private readonly IChangeService _changeService;
        private readonly ILogService _logService;
        private readonly ILogFileService _logFileService;
        private readonly ITemporaryFilesCache userTemporaryFilesStorage;
        private readonly IEmailGroupService _emailGroupService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IEmailService _emailService;
        private readonly ILanguageService _languageService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly ICausingPartService _causingPartService;

        private readonly IInvoiceArticlesModelFactory invoiceArticlesModelFactory;

        private readonly ICaseNotifierModelFactory caseNotifierModelFactory;

        private readonly INotifierService notifierService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceArticleService invoiceArticleService;        

        private readonly IConfiguration configuration;

        private readonly ICaseSolutionSettingService caseSolutionSettingService;

        private readonly IInvoiceHelper invoiceHelper;

        private readonly ICaseModelFactory caseModelFactory;

        private readonly CaseOverviewGridSettingsService caseOverviewSettingsService;

        private readonly GridSettingsService gridSettingsService;

        private readonly IOrganizationService _organizationService;

        private readonly IMasterDataService _masterDataService;

        private readonly OrganizationJsonService _orgJsonService;

        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;

        private readonly ICaseLockService _caseLockService;

        private readonly int _defaultMaxRows;

        private readonly int _defaultCaseLockBufferTime;

        private readonly int _defaultExtendCaseLockTime;

        private readonly IWatchDateCalendarService watchDateCalendarServcie;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        private readonly IReportServiceService _ReportServiceService;
         
        #endregion

        #region ***Constructor***

        public CasesController(
            ICaseService caseService,
            ICaseSearchService caseSearchService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseFileService caseFileService,
            ICaseSettingsService caseSettingService,
            ICaseTypeService caseTypeService,
            ICategoryService categoryService,
            IComputerService computerService,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IDepartmentService departmentService,
            IFinishingCauseService finishingCauseService,
            IImpactService impactService,
            IOUService ouService,
            IProblemService problemService,
            IPriorityService priorityService,
            IProductAreaService productAreaService,
            IRegionService regionService,
            ISettingService settingService,
            IStateSecondaryService stateSecondaryService,
            IStatusService statusService,
            IStandardTextService standardTextService,
            ISupplierService supplierService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IProjectService projectService,
            IChangeService changeService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            ICaseSolutionService caseSolutionService,
            ILogService logService,
            IEmailGroupService emailGroupService,
            IEmailService emailService,
            ILanguageService languageService,
            ILogFileService logFileService,
            IGlobalSettingService globalSettingService,
            IWorkContext workContext,
            ICaseNotifierModelFactory caseNotifierModelFactory,
            INotifierService notifierService,
            IInvoiceArticleService invoiceArticleService,            
            IConfiguration configuration,
            ICaseSolutionSettingService caseSolutionSettingService,
            IInvoiceHelper invoiceHelper,
            ICaseModelFactory caseModelFactory,
            CaseOverviewGridSettingsService caseOverviewSettingsService,
            GridSettingsService gridSettingsService,
            IOrganizationService organizationService,
            OrganizationJsonService orgJsonService,
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            ICaseLockService caseLockService,
            IMailTemplateService mailTemplateService,
            IWatchDateCalendarService watchDateCalendarServcie,
            ICaseInvoiceSettingsService CaseInvoiceSettingsService,
            ICausingPartService causingPartService,
            IInvoiceArticlesModelFactory invoiceArticlesModelFactory,
            IReportServiceService reportServiceService)
            : base(masterDataService)
        {
            this._masterDataService = masterDataService;
            this._caseService = caseService;
            this._caseSearchService = caseSearchService;
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseFileService = caseFileService;
            this._caseSettingService = caseSettingService;
            this._caseTypeService = caseTypeService;
            this._categoryService = categoryService;
            this._computerService = computerService;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._departmentService = departmentService;
            this._finishingCauseService = finishingCauseService;
            this._impactService = impactService;
            this._ouService = ouService;
            this._problemService = problemService;
            this._priorityService = priorityService;
            this._productAreaService = productAreaService;
            this._regionService = regionService;
            this._settingService = settingService;
            this._stateSecondaryService = stateSecondaryService;
            this._statusService = statusService;
            this._standardTextService = standardTextService;
            this._supplierService = supplierService;
            this._systemService = systemService;
            this._urgencyService = urgencyService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._projectService = projectService;
            this._changeService = changeService;
            this._logService = logService;
            this._logFileService = logFileService;
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
            this._caseSolutionService = caseSolutionService;
            this._emailGroupService = emailGroupService;
            this._emailService = emailService;
            this._languageService = languageService;
            this._globalSettingService = globalSettingService;
            this.workContext = workContext;
            this.caseNotifierModelFactory = caseNotifierModelFactory;
            this.notifierService = notifierService;
            this.invoiceArticleService = invoiceArticleService;            
            this.configuration = configuration;
            this.caseSolutionSettingService = caseSolutionSettingService;
            this.invoiceHelper = invoiceHelper;
            this.caseModelFactory = caseModelFactory;
            this.caseInvoiceSettingsService = CaseInvoiceSettingsService;
            this.caseOverviewSettingsService = caseOverviewSettingsService;
            this.gridSettingsService = gridSettingsService;
            this._organizationService = organizationService;
            this._orgJsonService = orgJsonService;
            this._registrationSourceCustomerService = registrationSourceCustomerService;
            this._caseLockService = caseLockService;
            this.watchDateCalendarServcie = watchDateCalendarServcie;
            this._mailTemplateService = mailTemplateService;
            this._defaultMaxRows = 10;
            this._defaultCaseLockBufferTime = 30; // Second
            this._defaultExtendCaseLockTime = 60; // Second
            this._causingPartService = causingPartService;
            this._ReportServiceService = reportServiceService;
            this.invoiceArticlesModelFactory = invoiceArticlesModelFactory;
        }

        #endregion

        #region ***Public Methods***

        #region --Advanced Search--
        public ActionResult AdvancedSearch(bool? clearFilters = false, bool doSearchAtBegining = false)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentUserId = SessionFacade.CurrentUser.Id;

            var customers = this._userService.GetUserProfileCustomersSettings(SessionFacade.CurrentUser.Id);
            var m = new AdvancedSearchIndexViewModel();
            var availableCustomers = customers.Select(c => new ItemOverview(c.CustomerName, c.CustomerId.ToString())).OrderBy(c => c.Name).ToList();

            m.SelectedCustomers = availableCustomers;

            CaseSearchModel advancedSearchModel;
            if ((clearFilters != null && clearFilters.Value) 
                || SessionFacade.CurrentAdvancedSearch == null)
            {
                SessionFacade.CurrentAdvancedSearch = null;
                advancedSearchModel = this.InitAdvancedSearchModel(currentCustomerId, currentUserId);
                SessionFacade.CurrentAdvancedSearch = advancedSearchModel;
            }
            else
                advancedSearchModel = SessionFacade.CurrentAdvancedSearch;

            m.CaseSearchFilterData = this.CreateAdvancedSearchFilterData(
                                                        currentCustomerId,
                                                        currentUserId,
                                                        advancedSearchModel,
                                                        availableCustomers);

            m.SpecificSearchFilterData = CreateAdvancedSearchSpecificFilterData(currentUserId);

            m.CaseSetting = this.GetCaseSettingModel(currentCustomerId, currentUserId);
            m.GridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(currentCustomerId);

            if (advancedSearchModel.Search != null)
                m.GridSettings.sortOptions = new GridSortOptions()
                    {
                        sortBy = advancedSearchModel.Search.SortBy,
                        sortDir = (advancedSearchModel.Search.Ascending) ? SortingDirection.Asc : SortingDirection.Desc
                    };

            m.DoSearchAtBegining = doSearchAtBegining;
            return this.View("AdvancedSearch/Index", m);
        }

        [HttpGet]
        public PartialViewResult GetCustomerSpecificFilter(int selectedCustomerId, bool resetFilter = false)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return PartialView("AdvancedSearch/_SpecificSearchTab", null);
            }            
            CaseSearchModel csm = null;

            if (!resetFilter)
                selectedCustomerId = 0;

            var model = CreateAdvancedSearchSpecificFilterData(SessionFacade.CurrentUser.Id, selectedCustomerId);
            return PartialView("AdvancedSearch/_SpecificSearchTab", model);
        }

        [ValidateInput(false)]
        public ActionResult DoAdvancedSearch(FormCollection frm)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            #region Code from old method. TODO: code review wanted
            var f = new CaseSearchFilter();

            var m = new CaseSearchResultModel();

            f.CustomerId = int.Parse(frm.ReturnFormValue("currentCustomerId"));
            f.Customer = frm.ReturnFormValue("lstfilterCustomers");
            f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");            
            f.UserPerformer = frm.ReturnFormValue("lstFilterPerformer");            
            f.Initiator = frm.ReturnFormValue("CaseInitiatorFilter");
            f.CaseRegistrationDateStartFilter = frm.GetDate("CaseRegistrationDateStartFilter");
            f.CaseRegistrationDateEndFilter = frm.GetDate("CaseRegistrationDateEndFilter");
            f.CaseClosingDateStartFilter = frm.GetDate("CaseClosingDateStartFilter");
            f.CaseClosingDateEndFilter = frm.GetDate("CaseClosingDateEndFilter");

            if (f.CaseRegistrationDateEndFilter != null)
                f.CaseRegistrationDateEndFilter = f.CaseRegistrationDateEndFilter.Value.AddDays(1);

            if (f.CaseClosingDateEndFilter != null)
                f.CaseClosingDateEndFilter = f.CaseClosingDateEndFilter.Value.AddDays(1);

            //Apply & save specific filters only when user has selected one customer 
            if (!string.IsNullOrEmpty(f.Customer) && !f.Customer.Contains(","))
            {
                f.WorkingGroup = frm.ReturnFormValue("lstFilterWorkingGroup");
                f.Department = frm.ReturnFormValue("lstfilterDepartment");
                f.Priority = frm.ReturnFormValue("lstfilterPriority");
                f.StateSecondary = frm.ReturnFormValue("lstfilterStateSecondary");                
                f.CaseType = frm.ReturnFormValue("hid_CaseTypeDropDown").convertStringToInt();
                f.ProductArea = f.ProductArea = frm.ReturnFormValue("hid_ProductAreaDropDown").ReturnCustomerUserValue();
                f.CaseClosingReasonFilter = frm.ReturnFormValue("hid_ClosingReasonDropDown").ReturnCustomerUserValue();


                var departments_OrganizationUnits = frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

                f.Department = GetDepartmentsFrom(departments_OrganizationUnits);
                f.OrganizationUnit = GetOrganizationUnitsFrom(departments_OrganizationUnits);
            }
            else
            {
                f.Department = string.Empty;
                f.WorkingGroup = string.Empty;
                f.Priority = string.Empty;
                f.StateSecondary = string.Empty;
                f.CaseType = 0;
                f.ProductArea = string.Empty;
                f.CaseClosingReasonFilter = string.Empty;
            }

            f.UserId = SessionFacade.CurrentUser.Id;

            if (!string.IsNullOrEmpty(frm.ReturnFormValue("txtCaseNumberSearch")))
            {
                f.FreeTextSearch = string.Format("#{0}", frm.ReturnFormValue("txtCaseNumberSearch"));
                f.CaseNumber = frm.ReturnFormValue("txtCaseNumberSearch");
            }
            else
            {
                if (!string.IsNullOrEmpty(frm.ReturnFormValue("txtCaptionSearch")))
                    f.CaptionSearch = frm.ReturnFormValue("txtCaptionSearch");
                else
                {
                    f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");
                    f.SearchThruFiles = frm.IsFormValueTrue("searchThruFiles");
                }
            }

            var maxRecords = this._defaultMaxRows;
            int.TryParse(frm.ReturnFormValue("lstfilterMaxRows"), out maxRecords);
            f.MaxRows = maxRecords.ToString();
            

            if (string.IsNullOrEmpty(f.CaseProgress))
                f.CaseProgress = CaseProgressFilter.None;

            CaseSearchModel sm;
            sm = this.InitAdvancedSearchModel(f.CustomerId, f.UserId);
            sm.caseSearchFilter = f;

            var jsonGridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(SessionFacade.CurrentCustomer.Id);

            // Convert Json Model to Business Model
            var colDefs = jsonGridSettings.columnDefs.Select(c => new GridColumnDef
                                                                        {
                                                                            id = GridColumnsDefinition.GetFieldId(c.field),
                                                                            cls = c.cls,
                                                                            name = c.field,
                                                                            isExpandable = c.isExpandable,
                                                                            width = c.width
                                                                        }).ToList();

            var gridSettings =
                new GridSettingsModel()
                    {
                        CustomerId = f.CustomerId,
                        cls = jsonGridSettings.cls,
                        pageOptions = jsonGridSettings.pageOptions,
                        sortOptions = jsonGridSettings.sortOptions,
                        columnDefs = colDefs
                    };

            gridSettings.sortOptions.sortBy = frm.ReturnFormValue("sortBy");
            var sortDir = 0;
            gridSettings.sortOptions.sortDir = (!string.IsNullOrEmpty(frm.ReturnFormValue("sortDir"))
                               && int.TryParse(frm.ReturnFormValue("sortDir"), out sortDir)
                               && sortDir == (int)SortingDirection.Asc) ? SortingDirection.Asc : SortingDirection.Desc;

            SessionFacade.AdvancedSearchOverviewGridSettings = gridSettings;

            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;
            m.caseSettings = new List<DHDomain.CaseSettings>();
            foreach (var col in colDefs)
            {
                var curSetting = new DHDomain.CaseSettings()
                        {
                            Id = col.id,
                            Name = col.name
                        };
                m.caseSettings.Add(curSetting);
            }

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            f.MaxTextCharacters = 0;
            m.cases = this._caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                0,
                0,
                userTimeZone,
                ApplicationTypes.Helpdesk
                ).Take(maxRecords).ToList();

            m.cases = this.TreeTranslate(m.cases, currentCustomerId);
            sm.Search.IdsForLastSearch = this.GetIdsFromSearchResult(m.cases);

            if (!string.IsNullOrWhiteSpace(sm.caseSearchFilter.FreeTextSearch))
            {
                if (sm.caseSearchFilter.FreeTextSearch[0] == '#')
                    sm.caseSearchFilter.FreeTextSearch = string.Empty;
            }

            if (!string.IsNullOrEmpty(f.OrganizationUnit))
            {
                var ouIds = f.OrganizationUnit.Split(',');
                if (ouIds.Any())
                {
                    foreach (var id in ouIds)
                        if (!string.IsNullOrEmpty(id))
                        {
                            if (string.IsNullOrEmpty(sm.caseSearchFilter.Department))
                                sm.caseSearchFilter.Department += string.Format("-{0}", id);
                            else
                                sm.caseSearchFilter.Department += string.Format(",-{0}", id);
                        }
                }
            }

            SessionFacade.CurrentAdvancedSearch = sm;
            #endregion

            var data = new List<Dictionary<string, object>>();
            var customerSettings = this._settingService.GetCustomerSetting(f.CustomerId);
            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            foreach (var searchRow in m.cases)
            {
                var jsRow = new Dictionary<string, object>
                                {
                                    { "case_id", searchRow.Id },
                                    { "caseIconTitle", searchRow.CaseIcon.CaseIconTitle() },
                                    {
                                        "caseIconUrl",
                                        string.Format(
                                            "/Content/icons/{0}",
                                            searchRow.CaseIcon.CaseIconSrc())
                                    },
                                    { "isUnread", searchRow.IsUnread },
                                    { "isUrgent", searchRow.IsUrgent },
                                };
                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    if (searchCol != null)
                    {
                        jsRow.Add(col.name, outputFormatter.FormatField(searchCol));
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }
                }

                data.Add(jsRow);
            }

            return this.Json(new { result = "success", data = data, gridSettings = gridSettings });
        }
        #endregion

        #region --Case Overview--

        public ActionResult InitFilter(
            int? customerId,
            bool? clearFilters = false,
            CasesCustomFilter customFilter = CasesCustomFilter.None,
            bool? useMyCases = false)
        {
            if (customerId.HasValue && (SessionFacade.CurrentCustomer == null || customerId.Value != SessionFacade.CurrentCustomer.Id))
            {
                SessionFacade.CurrentCustomer = this._customerService.GetCustomer(customerId.Value);
                SessionFacade.CaseOverviewGridSettings = null;
                SessionFacade.CurrentAdvancedSearch = null;
            }
            else
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    SessionFacade.CurrentCustomer = this._customerService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
                    SessionFacade.CaseOverviewGridSettings = null;
                    SessionFacade.CurrentAdvancedSearch = null;
                }
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var caseSearchModel = (clearFilters.HasValue && clearFilters.Value)
                                      ? this.InitEmptySearchModel(
                        SessionFacade.CurrentCustomer.Id,
                                          SessionFacade.CurrentUser.Id)
                                      : this.InitCaseSearchModel(
                                          SessionFacade.CurrentCustomer.Id,
                                          SessionFacade.CurrentUser.Id);
            if (useMyCases.HasValue && useMyCases == true)
            {
                caseSearchModel.caseSearchFilter.SearchInMyCasesOnly = true;
                caseSearchModel.caseSearchFilter.UserPerformer = string.Empty;
                caseSearchModel.caseSearchFilter.CaseProgress = CaseSearchFilter.InProgressCases;
            }

            switch (customFilter)
            {
                case CasesCustomFilter.UnreadCases:
                    caseSearchModel.caseSearchFilter.CaseProgress = CaseSearchFilter.UnreadCases;
                    caseSearchModel.caseSearchFilter.UserPerformer = string.Empty;
                    break;
                case CasesCustomFilter.HoldCases:
                    caseSearchModel.caseSearchFilter.CaseProgress = CaseSearchFilter.HoldCases;
                    caseSearchModel.caseSearchFilter.UserPerformer = string.Empty;
                    break;
                case CasesCustomFilter.InProcessCases:
                    caseSearchModel.caseSearchFilter.CaseProgress = CaseSearchFilter.InProgressCases;
                    caseSearchModel.caseSearchFilter.UserPerformer = string.Empty;
                    break;
            }

            caseSearchModel.caseSearchFilter.CustomFilter = customFilter;
            SessionFacade.CurrentCaseSearch = caseSearchModel;

            return new RedirectResult("~/cases");
        }


        public ActionResult Index()
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;

            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                    this.gridSettingsService.GetForCustomerUserGrid(
                        customerId,
                        SessionFacade.CurrentUser.UserGroupId,
                        userId,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }

            if (SessionFacade.CurrentCaseSearch == null)            
                SessionFacade.CurrentCaseSearch = this.InitCaseSearchModel(customerId, userId);
            
            var m = new JsonCaseIndexViewModel();
            
            var customerUser = this._customerUserService.GetCustomerSettings(customerId, userId);
            m.CaseSearchFilterData = this.CreateCaseSearchFilterData(customerId, SessionFacade.CurrentUser, customerUser, SessionFacade.CurrentCaseSearch);
            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.OnCaseOverview);
            this._caseSettingService.GetCaseSettingsWithUser(customerId, userId, SessionFacade.CurrentUser.UserGroupId);
            m.CaseSetting = this.GetCaseSettingModel(customerId, userId);
            var user = this._userService.GetUser(userId);

            m.PageSettings = new PageSettingsModel()
                                 {
                                     searchFilter = JsonCaseSearchFilterData.MapFrom(m.CaseSetting),
                                     userFilterFavorites = GetMyFavorites(customerId, userId),
                                     gridSettings = 
                                         JsonGridSettingsMapper.ToJsonGridSettingsModel(
                                             SessionFacade.CaseOverviewGridSettings,
                                             SessionFacade.CurrentCustomer.Id,
                                             m.CaseSetting.ColumnSettingModel.AvailableColumns.Count()),
                                     refreshContent = user.RefreshContent,
                                     messages = new Dictionary<string, string>()
                                                    {
                                                        { "information", Translation.GetCoreTextTranslation("Information") },
                                                        { "records_limited_msg", Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.") },
                                                    }
                                 };

            return this.View("Index", m);
        }

        [ValidateInput(false)]
        public ActionResult SearchAjax(FormCollection frm)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            #region Code from old method. TODO: code review wanted
            var f = new CaseSearchFilter();
            var m = new CaseSearchResultModel
            {
                GridSettings =
                    this.caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id)
            };
            f.CustomerId = SessionFacade.CurrentCustomer.Id;
            f.UserId = SessionFacade.CurrentUser.Id;
            f.Initiator = frm.ReturnFormValue(CaseFilterFields.InitiatorNameAttribute);
            f.CaseType = frm.ReturnFormValue(CaseFilterFields.CaseTypeIdNameAttribute).convertStringToInt();
            f.ProductArea = frm.ReturnFormValue(CaseFilterFields.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            f.Region = frm.ReturnFormValue(CaseFilterFields.RegionNameAttribute);
            f.User = frm.ReturnFormValue(CaseFilterFields.RegisteredByNameAttribute);
            f.Category = frm.ReturnFormValue(CaseFilterFields.CategoryNameAttribute);
            f.WorkingGroup = frm.ReturnFormValue(CaseFilterFields.WorkingGroupNameAttribute);
            f.UserResponsible = frm.ReturnFormValue(CaseFilterFields.ResponsibleNameAttribute);
            f.UserPerformer = frm.ReturnFormValue(CaseFilterFields.PerformerNameAttribute);

            f.Priority = frm.ReturnFormValue(CaseFilterFields.PriorityNameAttribute);
            f.Status = frm.ReturnFormValue(CaseFilterFields.StatusNameAttribute);
            f.StateSecondary = frm.ReturnFormValue(CaseFilterFields.StateSecondaryNameAttribute);

            f.CaseRegistrationDateStartFilter = frm.GetDate(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute);
            f.CaseRegistrationDateEndFilter = frm.GetDate(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute);

            f.CaseWatchDateStartFilter = frm.GetDate(CaseFilterFields.CaseWatchDateStartFilterNameAttribute);
            f.CaseWatchDateEndFilter = frm.GetDate(CaseFilterFields.CaseWatchDateEndFilterNameAttribute);
            f.CaseClosingDateStartFilter = frm.GetDate(CaseFilterFields.CaseClosingDateStartFilterNameAttribute);
            f.CaseClosingDateEndFilter = frm.GetDate(CaseFilterFields.CaseClosingDateEndFilterNameAttribute);
            f.CaseClosingReasonFilter = frm.ReturnFormValue(CaseFilterFields.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            f.SearchInMyCasesOnly = frm.IsFormValueTrue("SearchInMyCasesOnly");

            f.CaseProgress = frm.ReturnFormValue(CaseFilterFields.FilterCaseProgressNameAttribute);
            f.CaseFilterFavorite = frm.ReturnFormValue(CaseFilterFields.CaseFilterFavoriteNameAttribute);
            f.FreeTextSearch = frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);
            var departments_OrganizationUnits = frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

            f.Department = GetDepartmentsFrom(departments_OrganizationUnits);
            f.OrganizationUnit = GetOrganizationUnitsFrom(departments_OrganizationUnits);

            f.CaseRemainingTime = frm.ReturnFormValue(CaseFilterFields.CaseRemainingTimeAttribute);
            
            if (!string.IsNullOrEmpty(f.CaseRemainingTime))
            {
                int remainingTimeId;
                if (int.TryParse(f.CaseRemainingTime, out remainingTimeId))
                {
                    var timeTable = GetRemainigTimeById((RemainingTimes) remainingTimeId);
                    if (timeTable != null)
                    {
                        f.CaseRemainingTimeFilter = timeTable.RemaningTime;
                        f.CaseRemainingTimeUntilFilter = timeTable.RemaningTimeUntil;
                        f.CaseRemainingTimeMaxFilter = timeTable.MaxRemaningTime;
                        f.CaseRemainingTimeHoursFilter = timeTable.IsHour;
                    }
                }
            }

            int caseRemainingTimeFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTime"), out caseRemainingTimeFilter))
            {
                f.CaseRemainingTimeFilter = caseRemainingTimeFilter;
            }

            int caseRemainingTimeUntilFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeUntil"), out caseRemainingTimeUntilFilter))
            {
                f.CaseRemainingTimeUntilFilter = caseRemainingTimeUntilFilter;
            }

            int caseRemainingTimeMaxFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeMax"), out caseRemainingTimeMaxFilter))
            {
                f.CaseRemainingTimeMaxFilter = caseRemainingTimeMaxFilter;
            }

            bool caseRemainingTimeHoursFilter;
            if (bool.TryParse(frm.ReturnFormValue("CaseRemainingTimeHours"), out caseRemainingTimeHoursFilter))
            {
                f.CaseRemainingTimeHoursFilter = caseRemainingTimeHoursFilter;
            }

            CaseSearchModel sm;
            if (SessionFacade.CurrentCaseSearch == null
                || SessionFacade.CurrentCaseSearch.caseSearchFilter.CustomerId != f.CustomerId)
            {
                sm = this.InitCaseSearchModel(f.CustomerId, f.UserId);
            }
            else
            {
                sm = SessionFacade.CurrentCaseSearch;
                f.CustomFilter = sm.caseSearchFilter.CustomFilter;
            }

            this.ResolveParentPathesForFilter(f);
            sm.caseSearchFilter = f;
            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                    this.gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }
            else
            {
                //////////////////////////////////////////////////////////////////////////
                //// @TODO (alexander.semenischev): put validation for sortOpt.sortBy
                var sortBy = frm.ReturnFormValue("sortBy");
                var sortDirInt = 0;
                var sortDir = (!string.IsNullOrEmpty(frm.ReturnFormValue("sortDir"))
                                   && int.TryParse(frm.ReturnFormValue("sortDir"), out sortDirInt)
                                   && sortDirInt == (int)SortingDirection.Asc) ? SortingDirection.Asc : SortingDirection.Desc;
                if (sortBy != SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy
                    || sortDir != SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir)
                {
                    SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy = sortBy;
                    SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir = sortDir;
                    this.gridSettingsService.SaveCaseoviewSettings(
                        SessionFacade.CaseOverviewGridSettings,
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserGroupId);
                }
            }

            var gridSettings = SessionFacade.CaseOverviewGridSettings;
            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;
            m.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            f.MaxTextCharacters = MaxTextCharCount;
            m.cases = this._caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                this.workContext.Customer.WorkingDayStart,
                this.workContext.Customer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                showRemainingTime,
                out remainingTimeData,
                out aggregateData);

            m.cases = this.TreeTranslate(m.cases, f.CustomerId);
            sm.Search.IdsForLastSearch = this.GetIdsFromSearchResult(m.cases);                            
            
            var ouIds = f.OrganizationUnit.Split(',');
            if (ouIds.Any())
            {
                foreach (var id in ouIds)
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (string.IsNullOrEmpty(sm.caseSearchFilter.Department))
                            sm.caseSearchFilter.Department += string.Format("-{0}", id);
                        else
                            sm.caseSearchFilter.Department += string.Format(",-{0}", id);
                    }
            }

            SessionFacade.CurrentCaseSearch = sm;
            #endregion

            var customerSettings = this._settingService.GetCustomerSetting(f.CustomerId);            

            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            var data = new List<Dictionary<string, object>>();
            foreach (var searchRow in m.cases)
            {                
                var jsRow = new Dictionary<string, object>
                                {
                                    { "case_id", searchRow.Id },
                                    { "caseIconTitle", searchRow.CaseIcon.CaseIconTitle() },
                                    {
                                        "caseIconUrl",
                                        string.Format(
                                            "/Content/icons/{0}",
                                            searchRow.CaseIcon.CaseIconSrc())
                                    },
                                    { "isUnread", searchRow.IsUnread },
                                    { "isUrgent", searchRow.IsUrgent },
                                };
                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    if (searchCol != null)
                    {
                        jsRow.Add(col.name, outputFormatter.FormatField(searchCol));
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }
                }

                data.Add(jsRow);
            }

            var remainingView = string.Empty;
            if (SessionFacade.CurrentUser.ShowSolutionTime)
            {
                remainingView = this.RenderPartialViewToString(
                    "CaseRemainingTime",
                    this.caseModelFactory.GetCaseRemainingTimeModel(remainingTimeData, this.workContext));
            }

            string statisticsView = null;
            
            var statisticsFields = m.GridSettings.CaseFieldSettings.Where(cf=>  cf.ShowOnStartPage != 0 &&
                                                                               (cf.Name == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                                                                                cf.Name == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
                                                                   .Select(cf => cf.Name)
                                                                   .ToList();            
            var showCaseStatistics = SessionFacade.CurrentUser.ShowCaseStatistics && statisticsFields.Any();            
            if (showCaseStatistics)
            {                
                var expandedGroup = frm.ReturnFormValue("expandedGroup");
                if (string.IsNullOrEmpty(expandedGroup))
                    expandedGroup = SessionFacade.CaseOverviewGridSettings.ExpandedStatistics;

                statisticsView = this.RenderPartialViewToString(
                    "_Statistics",
                    GetCaseStatisticsModel(aggregateData, m.cases.Count(), expandedGroup, statisticsFields));    
            }

            return this.Json(new { result = "success", data = data, remainingView = remainingView, statisticsView = statisticsView });
        }

        #endregion

        #region --Favorite--

        [HttpPost]
        public JsonResult SaveFavorite(int favoriteId, string favoriteName, MyFavoriteFilterJSFields favoriteFilter)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json("Please logout and login again!");

            var jsFilter = new MyFavoriteFilterJSModel(favoriteId, favoriteName, favoriteFilter);

            var favoriteBM = new CaseFilterFavorite(jsFilter.Id,
                                                       SessionFacade.CurrentCustomer.Id,
                                                       SessionFacade.CurrentUser.Id,
                                                       jsFilter.Name,
                                                       jsFilter.GetFavoriteFields());

            return Json(this._caseService.SaveFavorite(favoriteBM));
        }

        [HttpPost]
        public JsonResult DeleteFavorite(int favoriteId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json("Please logout and login again!");            

            return Json(this._caseService.DeleteFavorite(favoriteId));
        }

        [HttpPost]
        public JsonResult LoadFavorites()
        {
            var favorites = new List<MyFavoriteFilterJSModel>();
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json(new List<MyFavoriteFilterJSModel>());
            else
                return Json(GetMyFavorites(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id));
        }

        #endregion

        #region --Case Lock--

        public JsonResult UnLockCase(string lockGUID)
        {
            if (!string.IsNullOrEmpty(lockGUID))
                this._caseLockService.UnlockCaseByGUID(new Guid(lockGUID));
            return Json("Success");
        }

        public JsonResult IsCaseAvailable(int caseId, DateTime caseChangedTime, string lockGuid)
        {
            var caseLock = this._caseLockService.GetCaseLockByCaseId(caseId);

            
            if (caseLock != null && caseLock.LockGUID == new Guid(lockGuid) && caseLock.ExtendedTime >= DateTime.Now)
                // Case still is locked by me
                return Json(true);
            else                            
                if (caseLock == null || (caseLock != null && !(caseLock.ExtendedTime >= DateTime.Now)))
                {
                    //case is not locked by me or is not locked at all 
                    var curCase = this._caseService.GetCaseById(caseId);
                    if (curCase != null && curCase.ChangeTime.RoundTick() == caseChangedTime.RoundTick())
                        //case is not updated yet by any other
                        return Json(true);
                    else
                        return Json(false);
                }
            else
                return Json(false);            
        }

        public JsonResult ReExtendCaseLock(string lockGuid, int extendValue)
        {
            return Json(this._caseLockService.ReExtendLockCase(new Guid(lockGuid), extendValue));
        }

        #endregion
                       
        #region --Case Save/Delete--

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult New(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation });
        }
         
        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult NewAndClose(CaseEditInput m, int? templateId, string BackUrl)
        {
            var newChild = false;

            if (m.case_.Id == 0 && m.ParentId != null)
                newChild = true;

            int caseId = this.Save(m);
            this.CheckTemplateParameters(templateId, caseId);
            string url;

            if (BackUrl == null)
                BackUrl = "";


            if (BackUrl != "")
            {
                url = BackUrl;
            }
            else if (m.ParentId.HasValue && newChild)
            {
                url = this.GetLinkWithHash(ChildCasesHashTab, new { id = m.ParentId }, "Edit");
            }
            else
            {
                url = this.GetLinkWithHash(string.Empty, new { customerId = m.case_.Customer_Id }, "Index");
            }

            return this.Redirect(url);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndAddCase(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult Edit(CaseEditInput m)
        {
            int caseId = this.Save(m);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation, updateState = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult EditAndClose(CaseEditInput m, string BackUrl)
        {
            this.Save(m);
            return string.IsNullOrEmpty(BackUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = m.case_.Customer_Id })) : this.Redirect(BackUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditAndAddCase(CaseEditInput m)
        {
            this.Save(m);
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
        }

        [HttpPost]
        public RedirectResult DeleteCase(
            int caseId,
            int customerId,            
            int? parentCaseId,
            string backUrl
            )
        {
            var basePath = this._masterDataService.GetFilePath(customerId);
            var caseGuid = this._caseService.Delete(caseId, basePath, parentCaseId);
            this.userTemporaryFilesStorage.ResetCacheForObject(caseGuid.ToString());
            if (parentCaseId.HasValue)
            {
                var parentCase = this._caseService.GetCaseById(parentCaseId.Value);
                if (parentCase.Performer_User_Id == SessionFacade.CurrentUser.Id)
                {
                    var url = this.GetLinkWithHash(ChildCasesHashTab, new { id = parentCaseId.Value }, "Edit");
                    return this.Redirect(url);
                }
            }
            return string.IsNullOrEmpty(backUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = customerId })) : this.Redirect(backUrl);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            var currentuser = this._userService.GetUser(SessionFacade.CurrentUser.Id);
            string err = "";

            var tmpLog = this._logService.GetLogById(id);
            var logFiles = this._logFileService.FindFileNamesByLogId(id);

            var logFileStr = string.Empty;
            if (logFiles.Any())
            {
                if (currentuser.DeleteAttachedFilePermission != 0)
                {
                    logFileStr = string.Format("{0}{1}", StringTags.LogFile, string.Join(StringTags.Seperator, logFiles.ToArray()));
                }
                else
                {
                    err = Translation.Get("Du kan inte ta bort noteringen, eftersom du saknar behörighet att ta bort bifogade filer") + ".";
                    TempData["PreventError"] = err;
                    return this.RedirectToAction("editlog", "cases", new { id = id, customerId = SessionFacade.CurrentCustomer.Id });
                }
            }

            var c = this._caseService.GetCaseById(caseId);
            var basePath = _masterDataService.GetFilePath(c.Customer_Id);
            var logGuid = this._logService.Delete(id, basePath);
            this.userTemporaryFilesStorage.ResetCacheForObject(logGuid.ToString());

            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var logStr = string.Format("{0}{1}{2}{3}{4}{5}",
                                        StringTags.Delete,
                                        StringTags.ExternalLog,
                                        tmpLog.TextExternal,
                                        StringTags.InternalLog,
                                        tmpLog.TextInternal,
                                        logFileStr);

            var extraField = new ExtraFieldCaseHistory { CaseLog = logStr };            
            this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);

            return this.RedirectToAction("edit", "cases", new { id = caseId });

        }
        
        #endregion
       
        #region --New/Edit--

        public ActionResult New(
           int? customerId,
           int? templateId,
           int? copyFromCaseId,
           int? caseLanguageId,
           int? templateistrue)
        {
            CaseInputViewModel m = null;
            if (!customerId.HasValue)
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    return new RedirectResult("~/Error/Unathorized");
                }

                customerId = SessionFacade.CurrentCustomer.Id;
            }

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            if (SessionFacade.CurrentUser != null)
            {
                if (SessionFacade.CurrentUser.CreateCasePermission == 1 || templateistrue == 1)
                {
                    var userId = SessionFacade.CurrentUser.Id;
                    var caseLockModel = new CaseLockModel();
                    m = this.GetCaseInputViewModel(
                        userId,
                        customerId.Value,
                        0,
                        caseLockModel,
                        string.Empty,
                        null,
                        templateId,
                        copyFromCaseId,
                        false,
                        templateistrue);

                    var caseParam = new NewCaseParams
                    {
                        customerId = customerId.Value,
                        templateId = templateId,
                        copyFromCaseId = copyFromCaseId,
                        caseLanguageId = caseLanguageId
                    };

                    m.NewModeParams = caseParam;
                    AddViewDataValues();

                    // Positive: Send Mail to...
                    if (m.CaseMailSetting.DontSendMailToNotifier == false) m.CaseMailSetting.DontSendMailToNotifier = true;
                    else m.CaseMailSetting.DontSendMailToNotifier = false;

                    var moduleCaseInvoice = this._settingService.GetCustomerSetting(customerId.Value).ModuleCaseInvoice;
                    if (moduleCaseInvoice == 1)
                    {
                        var caseInvoices = this.invoiceArticleService.GetCaseInvoicesWithTimeZone(m.case_.Id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        var invoiceArticles = this.invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                        m.InvoiceModel = new CaseInvoiceModel(customerId.Value, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
                    }
                    
                    return this.View(m);
                }
            }

            return this.RedirectToAction("index", "cases", new { id = customerId });
        }


        [UserCasePermissions]
        public ActionResult Edit(int id, 
            string redirectFrom = "", 
            int? moveToCustomerId = null, 
            bool? uni = null, 
            bool updateState = true, 
            string backUrl = null)
        {
            CaseInputViewModel m = null;
            
            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;

                var caseLockViewModel = GetCaseLockModel(id, userId);
                int customerId = moveToCustomerId.HasValue ? moveToCustomerId.Value : _caseService.GetCaseById(id).Customer_Id;
                m = this.GetCaseInputViewModel(userId, customerId, id, caseLockViewModel, redirectFrom, backUrl, null, null, updateState);
                if (uni.HasValue)
                {
                    m.UpdateNotifierInformation = uni.Value;                    
                }
                
                ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

                // If user logged in from link in email
                var currentCustomerId = SessionFacade.CurrentCustomer.Id;
                var currentCase = this._caseService.GetCaseById(id);
                if (currentCustomerId != currentCase.Customer_Id)
                    this.InitFilter(currentCase.Customer_Id, false, CasesCustomFilter.None, false);
                    
                // User has not access to case
                if (m.EditMode == Enums.AccessMode.NoAccess)
                    return this.RedirectToAction("index", "home");

                // move case to another customer
                if (moveToCustomerId.HasValue)
                {
                    m.case_.Customer_Id = moveToCustomerId.Value;
                    m.case_.CaseType_Id = 0;
                    m.case_.ProductArea_Id = null;
                    m.case_.Category_Id = null;
                    m.case_.Change_Id = null;
                    m.case_.ChangeByUser_Id = null;
                    m.case_.Department_Id = null;
                    m.case_.Impact_Id = null;
                    m.case_.LockCaseToWorkingGroup_Id = null;
                    m.case_.OU_Id = null;
                    m.case_.Performer_User_Id = 0;
                    m.case_.Priority_Id = null;
                    m.case_.Problem_Id = null;
                    m.case_.ProductAreaQuestionVersion_Id = null;
                    m.case_.ProductAreaSetDate = null;
                    m.case_.Project_Id = null;
                    m.case_.Region_Id = null;
                    m.case_.StateSecondary_Id = null;
                    m.case_.Status_Id = null;
                    m.case_.Supplier_Id = null;
                    m.case_.System_Id = null;
                    m.case_.Urgency_Id = null;
                    m.case_.User_Id = null;
                    m.case_.WorkingGroup_Id = null;
                    m.ParantPath_CaseType = ParentPathDefaultValue;
                    m.ParantPath_ProductArea = ParentPathDefaultValue;
                    m.ParantPath_OU = ParentPathDefaultValue;
                }

                var moduleCaseInvoice = this._settingService.GetCustomerSetting(m.case_.Customer_Id).ModuleCaseInvoice;
                if (moduleCaseInvoice == 1)
                {
                    var caseInvoices = this.invoiceArticleService.GetCaseInvoicesWithTimeZone(id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    var invoiceArticles = this.invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                    m.InvoiceModel = new CaseInvoiceModel(m.case_.Customer_Id, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
                }              

                m.CustomerSettings = this.workContext.Customer.Settings;

                m.CustomerSettings.ModuleCaseInvoice = Convert.ToBoolean(this._settingService.GetCustomerSetting(m.case_.Customer_Id).ModuleCaseInvoice); // TODO FIX
            }        

            AddViewDataValues();

            // Positive: Send Mail to...
            if (m.CaseMailSetting.DontSendMailToNotifier == false)
                m.CaseMailSetting.DontSendMailToNotifier = true;
            else
                m.CaseMailSetting.DontSendMailToNotifier = false;
            
            return this.View(m);
        }

        public ActionResult EditLog(int id, int customerId)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
                var cs = this._settingService.GetCustomerSetting(customerId);
                var customer = this._customerService.GetCustomer(customerId);
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

                if (cu != null)
                {
                    m = new CaseInputViewModel();
                    m.CaseLog = this._logService.GetLogById(id);
                    m.LogKey = m.CaseLog.Id.ToString();
                    m.customerUserSetting = cu;
                    m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                    m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
                    m.finishingCauses = this._finishingCauseService.GetFinishingCauses(customerId);
                    m.case_ = this._caseService.GetCaseById(m.CaseLog.CaseId);
                    bool UseVD = false;
                    if (!string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
                    {
                        UseVD = true;
                    }
                    m.LogFilesModel = new FilesModel(id.ToString(), this._logFileService.FindFileNamesByLogId(id), UseVD);
                    const bool isAddEmpty = true;
                    var responsibleUsersAvailable = this._userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
                    var customerSettings = this._settingService.GetCustomerSetting(customerId);
                    m.OutFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);
                    m.ResponsibleUsersAvailable = responsibleUsersAvailable.MapToSelectList(customerSettings, isAddEmpty);
                    m.SendToDialogModel = this.CreateNewSendToDialogModel(customerId, responsibleUsersAvailable.ToList(), cs);
                    m.CaseLog.SendMailAboutCaseToNotifier = false;
                    m.MinWorkingTime = cs.MinRegWorkingTime != 0 ? cs.MinRegWorkingTime : 30;                    
                    // check department info
                    m.ShowInvoiceFields = 0;
                    if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
                    {
                        var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                        if (d != null)
                            m.ShowInvoiceFields = d.Charge;
                    }
                    // check state secondary info
                    m.Disable_SendMailAboutCaseToNotifier = false;
                    if (m.case_.StateSecondary_Id > 0)
                        if (m.case_.StateSecondary != null)
                        {
                            m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1 ? true : false;
                        }

                    var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
                    var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);

                    // TODO: Should mix CustomerSettings & Setting 
                    m.CustomerSettings = this.workContext.Customer.Settings;
                    m.Setting = cs;                    
                    m.EditMode = EditMode(m, ModuleName.Log, deps, acccessToGroups, true);
                    m.LogFileNames = string.Join("|", m.LogFilesModel.Files.ToArray());
                    AddViewDataValues();
                    SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
                    // User has not access to case/log
                    if (m.EditMode == Enums.AccessMode.NoAccess)
                        return this.RedirectToAction("index", "home");

                }
            }

            return this.View(m);
        }

        [HttpPost]
        public RedirectToRouteResult EditLog(Case case_, CaseLog caseLog)
        {
            this.UpdateCaseLogForCase(case_, caseLog);
            return this.RedirectToAction("edit", "cases", new { id = caseLog.CaseId });
        }

        #endregion

        #region --Auto Complete Fields--
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Search_User(string query, int customerId, string searchKey)
        {
            var result = this._computerService.SearchComputerUsers(customerId, query);

            var ComputerUserSearchRestriction = _settingService.GetCustomerSetting(customerId).ComputerUserSearchRestriction;
            if (ComputerUserSearchRestriction == 1)
            {
                var departmentIds = this._departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId).Select(x => x.Id).ToList();
                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                {
                    departmentIds = this._departmentService.GetDepartments(customerId).Select(x => x.Id).ToList();
                }

                result = this._computerService.SearchComputerUsersByDepartments(customerId, query, departmentIds);
            }

            return Json(new { searchKey = searchKey, result = result });            
        }

        [HttpPost]
        public JsonResult Get_User(int id)
        {
            var cu = this._computerService.GetComputerUser(id);
            var u = new
            {
                num = cu.UserId,
                name = cu.FirstName + ' ' + cu.SurName,
                email = cu.Email,
                place = cu.Location,
                phone = cu.Phone,
                usercode = cu.UserCode,
                cellphone = cu.Cellphone,
                regionid = (cu.Department != null) ? cu.Department.Region_Id == null ? string.Empty : cu.Department.Region_Id.Value.ToString() : string.Empty,
                regionname = (cu.Department != null) ? cu.Department.Region != null ? cu.Department.Region.Name : string.Empty : string.Empty,
                departmentid = cu.Department_Id,
                departmentname = (cu.Department != null? cu.Department.DepartmentName : string.Empty),
                ouid = cu.OU_Id,
                ouname = cu.OU != null ? (cu.OU.Parent != null ? cu.OU.Parent.Name + " - " : string.Empty) + cu.OU.Name : string.Empty,
                costcentre = string.IsNullOrEmpty(cu.CostCentre)? string.Empty : cu.CostCentre
            };
            return this.Json(u);
        }

        [HttpPost]
        public ActionResult Search_Computer(string query, int customerId)
        {
            var result = this._computerService.SearchComputer(customerId, query);
            return this.Json(result);
        }

        #endregion

        #region --Change Depended Field--

        public JsonResult ChangeRegion(int? id, int customerId, int departmentFilterFormat)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return this.Json(new { success = false, message = "Access denied" });
            }

            var list = this._orgJsonService.GetActiveDepartmentForUserByRegion(id, SessionFacade.CurrentUser.Id, customerId, departmentFilterFormat);
            return this.Json(new { list });
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<User> performersList;
            var customerSettings = this._settingService.GetCustomerSetting(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = this._userService.GetAvailablePerformersOrUserId(customerId);
            }

            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                return
                    this.Json(
                        new
                            {
                                list =
                                    performersList.OrderBy(it => it.FirstName)
                                        .ThenBy(it => it.SurName)
                                        .Select(
                                            it =>
                                            new IdName
                                                {
                                                    id = it.Id,
                                                    name = string.Format("{0} {1}", it.FirstName, it.SurName)
                                                })
                            });
            }

            return
                this.Json(
                    new
                        {
                            list =
                                performersList.OrderBy(it => it.SurName)
                                    .ThenBy(it => it.FirstName)
                                    .Select(
                                        it =>
                                        new IdName
                                            {
                                                id = it.Id,
                                                name = string.Format("{0} {1}", it.SurName, it.FirstName)
                                            })
                        });
        }

        public JsonResult GetWatchDateByDepartment(int departmentId)
        {
            var dept = this._departmentService.GetDepartment(departmentId);
            DateTime? res = null;
            if (dept != null && dept.WatchDateCalendar_Id.HasValue)
            {
                res = this.watchDateCalendarServcie.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
            }

            return this.Json(new { result = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        public int ChangeWorkingGroupSetStateSecondary(int? id)
        {
            int ret = 0;
            if (id.HasValue)
            {
                DHDomain.WorkingGroupEntity w = this._workingGroupService.GetWorkingGroup(id.Value);
                ret = w != null ? w.StateSecondary_Id.HasValue ? w.StateSecondary_Id.Value : 0 : 0;
            }
            return ret;
        }

        public int ShowInvoiceFields(int? departmentId)
        {
            if (departmentId.HasValue)
            {
                var d = this._departmentService.GetDepartment(departmentId.Value);
                return d.Charge;
            }
            return 0;
        }

        public JsonResult ChangeDepartment(int? id, int customerId)
        {
            var list = this._orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._supplierService.GetSuppliers(customerId).Where(x => x.IsActive == 1).Select(x => new { id = x.Id, name = x.Name });
            return this.Json(new { list });
        }

        public JsonResult GetCaseFields()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var CaseFields = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            return this.Json(new { Result = CaseFields.Select(x => new {
                Name = x.Name,
                Show = x.ShowOnStartPage
            }) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangePriority(int? id)
        {
            string ret = string.Empty;
            if (id.HasValue)
            {
                var p = _priorityService.GetPriority(id.Value);
                ret = p != null ? p.LogText : string.Empty;
            }
            return Json(new { ExternalLogText = ret });
        }

        public int GetPriorityIdForImpactAndUrgency(int? impactId, int? urgencyId)
        {
            int ret = 0;
            if (impactId.HasValue && urgencyId.HasValue)
            {
                ret = this._priorityService.GetPriorityIdByImpactAndUrgency(impactId.Value, urgencyId.Value);
            }
            return ret;
        }

        public string ChangeCaseType(int? id)
        {
            string ret = null;
            if (id.HasValue)
            {
                var e = _caseTypeService.GetCaseType(id.Value);
                if (e != null)
                    ret = e.User_Id.HasValue ? e.User_Id.Value != 0 ? e.User_Id.Value.ToString() : null : null;
            }
            return ret;
        }

        public string ChangeSystem(int? id)
        {
            string ret = null;
            if (id.HasValue)
            {
                var e = _systemService.GetSystem(id.Value);
                if (e != null)
                    ret = e.Urgency_Id.HasValue ? e.Urgency_Id != 0 ? e.Urgency_Id.Value.ToString() : null : null;
            }
            return ret;
        }

        public JsonResult ChangeProductArea(int? id)
        {
            int workinggroupId = 0;
            int priorityId = 0;
            int hasChild = 0;
            if (id.HasValue)
            {
                var e = _productAreaService.GetProductArea(id.Value);
                if (e != null)
                {
                    workinggroupId = e.WorkingGroup_Id.HasValue ? e.WorkingGroup_Id.Value : 0;
                    priorityId = e.Priority_Id.HasValue ? e.Priority_Id.Value : 0;
                    if (e.SubProductAreas != null && e.SubProductAreas.Where(s => s.IsActive != 0).ToList().Count > 0)
                        hasChild = 1;
                }
            }
            return Json(new { WorkingGroup_Id = workinggroupId, Priority_Id = priorityId, HasChild = hasChild });
        }

        public JsonResult ChangeStatus(int? id)
        {
            int workinggroupId = 0;
            int stateSecondaryId = 0;

            if (id.HasValue)
            {
                var e = _statusService.GetStatus(id.Value);
                if (e != null)
                {
                    workinggroupId = e.WorkingGroup_Id.HasValue ? e.WorkingGroup_Id.Value : 0;
                    stateSecondaryId = e.StateSecondary_Id.HasValue ? e.StateSecondary_Id.Value : 0;
                }
            }
            return Json(new { WorkingGroup_Id = workinggroupId, StateSecondary_Id = stateSecondaryId });
        }

        public JsonResult ChangeStateSecondary(int? id)
        {
            int workinggroupId = 0;
            int noMailToNotifier = 0;
            int reCalculateWatchDate = 0;

            if (id.HasValue)
            {
                var s = _stateSecondaryService.GetStateSecondary(id.Value);
                reCalculateWatchDate = s != null ? s.RecalculateWatchDate : 0;
                noMailToNotifier = s != null ? s.NoMailToNotifier : 0;
                workinggroupId = s != null ? s.WorkingGroup_Id.HasValue ? s.WorkingGroup_Id.Value : 0 : 0;
            }
            return Json(new {
                NoMailToNotifier = noMailToNotifier,
                WorkingGroup_Id = workinggroupId,
                ReCalculateWatchDate = reCalculateWatchDate
            });
        }

        #endregion

        #region --Validation--

        [HttpPost]
        public JsonResult CheckCaseForActiveData(CaseMasterDataFieldsModel caseMasterDataFields)
        {       
            var inactiveFields = GetInactiveFieldsValue(caseMasterDataFields);
            if (inactiveFields.Any())
            {
                var publicMessage = Translation.Get("Ärendet kunde inte sparas pga inaktiva värden(n). Var vänlig kontrollera ärendet.");
                var fieldNames = String.Join("<br/> ", inactiveFields.ToArray());
                return Json(string.Format("{0}<br/> {1}", publicMessage, fieldNames));
            }
            else
                return Json("valid");
        }

        [HttpGet]
        public JsonResult ProductAreaHasChild(int pId)
        {
            var res = "false";
            var productArea = this._productAreaService.GetProductArea(pId);
            if (productArea != null && productArea.SubProductAreas != null &&
                productArea.SubProductAreas.Where(p => p.IsActive != 0).ToList().Count > 0)
                res = "true";

            return Json(res);
        }

        #endregion

        #region --Files--

        [HttpGet]
        public ActionResult Files(string id, string savedFiles)
        {
            //var files = this._caseFileService.GetCaseFiles(int.Parse(id));
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Cases)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            var cfs = MakeCaseFileModel(files, savedFiles);
            var customerId = 0;
            if (!GuidHelper.IsGuid(id)) {
                customerId = this._caseService.GetCaseById(int.Parse(id)).Customer_Id;
            }
            
            //
            bool UseVD = false;
            if (customerId != 0 && !string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
            {
                UseVD = true;
            }
            
            var model = new CaseFilesModel(id, cfs.ToArray(), savedFiles, UseVD);
            return this.PartialView("_CaseFiles", model);
        }
        
        [HttpGet]
        public ActionResult LogFiles(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Log)
                                : this._logFileService.FindFileNamesByLogId(int.Parse(id));
            
            var customerId = 0;
            if (!GuidHelper.IsGuid(id))
            {
                var caseId = this._logService.GetLogById(int.Parse(id)).CaseId;
                customerId = this._caseService.GetCaseById(caseId).Customer_Id;
            }
            bool UseVD = false;
            if (customerId != 0 &&!string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
            {
                UseVD = true;
            }

            var model = new FilesModel(id, files,UseVD);
            return this.PartialView("_CaseLogFiles", model);
        }

        [HttpPost]
        public void UploadCaseFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this.userTemporaryFilesStorage.FileExists(name, id, ModuleName.Cases))
                {
                    name = DateTime.Now.ToString() + '-' + name;
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Cases);
            }
            else
            {
                if (this._caseFileService.FileExists(int.Parse(id), name))
                {
                    name = DateTime.Now.ToString() + '_' + name;
                }

                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                var caseFileDto = new CaseFileDto(
                                uploadedData,
                                basePath,
                                name,
                                DateTime.Now,
                                int.Parse(id),
                                this.workContext.User.UserId);
                this._caseFileService.AddFile(caseFileDto);

            }
        }

        [HttpPost]
        public void UploadLogFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this.userTemporaryFilesStorage.FileExists(name, id, ModuleName.Log))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Log);
            }
        }
        
        [HttpPost]
        public void DeleteCaseFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this.userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Cases);
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = _masterDataService.GetFilePath(c.Customer_Id);

                this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), basePath, fileName.Trim());

                IDictionary<string, string> errors;
                string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var extraField = new ExtraFieldCaseHistory { CaseFile = StringTags.Delete + fileName.Trim() };
                this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
            }
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this.userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);
            else
            {
                var log = this._logService.GetLogById(int.Parse(id));
                DHDomain.Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = this._caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), basePath, fileName.Trim());

                IDictionary<string, string> errors;
                string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (c != null)
                {
                    var extraField = new ExtraFieldCaseHistory { LogFile = StringTags.Delete + fileName.Trim() };
                    this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
                }
            }
        }

        [HttpGet]
        public UnicodeFileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            var defaultFileName = GetDefaultFileName(fileName);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", defaultFileName));

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        [HttpGet]
        public UnicodeFileContentResult DownloadLogFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log);
            else
            {
                var l = this._logService.GetLogById(int.Parse(id));
                var basePath = string.Empty;
                if (l != null)
                {
                    var c = this._caseService.GetCaseById(l.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                fileContent = this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }
            var defaultFileName = GetDefaultFileName(fileName);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", defaultFileName));

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        private string GetCaseFileNames(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Cases)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return String.Join("|", files);

        }

        private string GetLogFileNames(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Log)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return String.Join("|", files);

        }                                

        #endregion

        #region --Case Actions--

        public JsonResult MarkAsUnread(int id, int customerId)
        {
            this._caseService.MarkAsUnread(id);
            //return this.RedirectToAction("index", "cases", new { customerId = customerId });
            return Json("Success");
        }

        public PartialViewResult ShowCasePrintPreview(int caseId, int caseNumber)
        {
            var model = GetCaseReportViewerData("CaseDetailsList", caseId, caseNumber);
            model.CanShow = true;
            return PartialView("_CasePrint", model);
        }

        public RedirectToRouteResult Copy(int id, int customerId)
        {
            return this.RedirectToAction("new", "cases", new { customerId = customerId, copyFromCaseId = id });
        }

        public RedirectToRouteResult FollowUpRemove(int id)
        {
            this._caseService.UpdateFollowUpDate(id, null);
            return this.RedirectToAction("edit", "cases", new { id = id, redirectFrom = "save" });
        }

        public RedirectToRouteResult FollowUp(int id)
        {
            this._caseService.UpdateFollowUpDate(id, DateTime.UtcNow);
            return this.RedirectToAction("edit", "cases", new { id = id, redirectFrom = "save" });
        }

        public RedirectToRouteResult Activate(int id, string backUrl = null)
        {
            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (SessionFacade.CurrentUser != null)
                if (SessionFacade.CurrentUser.ActivateCasePermission == 1)
                    this._caseService.Activate(id, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors);

            return this.RedirectToAction("edit", "cases", new { id, redirectFrom = "save", backUrl });
        }

        #endregion
        
        #region --Overview Setting--

        public void SaveSetting(FormCollection frm)
        {
            int customerId = int.Parse(frm["CustomerId"]);
            int userId = int.Parse(frm["UserId"]);

            bool regionCheck = frm.IsFormValueTrue("RegionCheck");
            var regions = (regionCheck)
                ? ((frm.ReturnFormValue("lstRegions") == string.Empty) ? "0" : frm.ReturnFormValue("lstRegions"))
                : string.Empty;

            bool departmentsCheck = frm.IsFormValueTrue("IsDepartmentChecked");
            var departments = departmentsCheck
                 ? ((frm.ReturnFormValue(CaseSettingModel.DepartmentsControlName) == string.Empty) ? "0" : frm.ReturnFormValue(CaseSettingModel.DepartmentsControlName))
                : string.Empty;

            bool registerByCheck = frm.IsFormValueTrue("RegisteredByCheck");
            var registerBy = (registerByCheck)
                ? ((frm.ReturnFormValue("lstRegisterBy") == string.Empty) ? "0" : frm.ReturnFormValue("lstRegisterBy"))
                : string.Empty;

            bool caseTypeCheck = frm.IsFormValueTrue("CaseTypeCheck");
            var caseType = caseTypeCheck
                ? ((frm.ReturnFormValue("CaseTypeId") == string.Empty) ? "0" : frm.ReturnFormValue("CaseTypeId"))
                : string.Empty;

            bool productAreaCheck = frm.IsFormValueTrue("ProductAreaCheck");
            var productArea = (productAreaCheck)
                ? ((frm.ReturnFormValue("ProductAreaId") == string.Empty) ? "0" : frm.ReturnFormValue("ProductAreaId"))
                : string.Empty;

            bool workingGroupCheck = frm.IsFormValueTrue("WorkingGroupCheck");
            var workingGroup = (workingGroupCheck)
                ? ((frm.ReturnFormValue("lstWorkingGroup") == string.Empty)
                    ? "0"
                    : frm.ReturnFormValue("lstWorkingGroup"))
                : string.Empty;

            bool responsibleCheck = frm.IsFormValueTrue("ResponsibleCheck");

            // we always show administrator filter field
            var administrator = (frm.ReturnFormValue("lstAdministrator") == string.Empty)
                ? " "
                : frm.ReturnFormValue("lstAdministrator");

            bool priorityCheck = frm.IsFormValueTrue("PriorityCheck");
            var priority = (priorityCheck)
                ? ((frm.ReturnFormValue("lstPriority") == string.Empty) ? "0" : frm.ReturnFormValue("lstPriority"))
                : string.Empty;

            var stateCheck = frm.IsFormValueTrue("StateCheck");

            bool subStateCheck = frm.IsFormValueTrue("SubStateCheck");
            var subState = (subStateCheck)
                ? ((frm.ReturnFormValue("lstSubState") == string.Empty) ? "0" : frm.ReturnFormValue("lstSubState"))
                : string.Empty;

            var closingReasonCheck = frm.IsFormValueTrue("ClosingReasonCheck");
            var closingReason = closingReasonCheck
                ? ((frm.ReturnFormValue("ClosingReasonId") == string.Empty) ? "0" : frm.ReturnFormValue("ClosingReasonId"))
                : string.Empty;

            var caseRegistrationDateCheck = frm.IsFormValueTrue("CaseRegistrationDateFilterShow");
            var caseClosingDateCheck = frm.IsFormValueTrue("CaseClosingDateFilterShow");
            var caseWatchDateCheck = frm.IsFormValueTrue("CaseWatchDateFilterShow");

            bool caseRemainingTimeCheck = frm.IsFormValueTrue("CaseRemainingTimeChecked");
            var caseRemainingTime = caseRemainingTimeCheck
                 ? ((frm.ReturnFormValue("lstfilterCaseRemainingTime") == string.Empty) ? "0" : frm.ReturnFormValue("lstfilterCaseRemainingTime"))
                : string.Empty;

            var newCaseSetting = new UserCaseSetting(
                            customerId,
                            userId,
                            regions,
                            departments,
                            registerBy,
                            caseType,
                            productArea,
                            workingGroup,
                            responsibleCheck,
                            administrator,
                            priority,
                            stateCheck,
                            subState,
                            (caseRegistrationDateCheck ? frm.GetDate("CaseRegistrationDateStartFilter") : null),
                            (caseRegistrationDateCheck ? frm.GetDate("CaseRegistrationDateEndFilter") : null),
                            (caseWatchDateCheck ? frm.GetDate("CaseWatchDateStartFilter") : null),
                            (caseWatchDateCheck ? frm.GetDate("CaseWatchDateEndFilter") : null),
                            (caseClosingDateCheck ? frm.GetDate("CaseClosingDateStartFilter") : null),
                            (caseClosingDateCheck ? frm.GetDate("CaseClosingDateEndFilter") : null),
                            frm.IsFormValueTrue("CaseRegistrationDateFilterShow"),
                            frm.IsFormValueTrue("CaseWatchDateFilterShow"),
                            frm.IsFormValueTrue("CaseClosingDateFilterShow"),
                            closingReason,
                            frm.IsFormValueTrue("CaseInitiatorFilterShow"),
                            caseRemainingTime);

            this._customerUserService.UpdateUserCaseSetting(newCaseSetting);
            SessionFacade.CurrentCaseSearch = null;
        }

        /// <summary>
        /// Saving settings for case overview page
        /// </summary>
        /// <param name="inputSettings"></param>
        public void SaveColSetting(CaseOverviewSettingsInput inputSettings)
        {
            if (!inputSettings.FieldStyle.Any())
            {
                throw new ArgumentException("columns count can not be 0");
            }

            this.gridSettingsService.SaveCaseoviewSettings(
                inputSettings.MapToGridSettingsModel(),
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserGroupId);
            SessionFacade.CaseOverviewGridSettings = null;
        }

        #endregion

        #region --Parent Child Case--

        public ActionResult NewChildCase(int parentCaseId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            CaseInputViewModel m = this.GetCaseInputViewModel(userId, customerId, 0, caseLockModel, string.Empty, null, null, null, false, null, parentCaseId);

            var caseParam = new NewCaseParams
            {
                customerId = SessionFacade.CurrentCustomer.Id,
                templateId = null,
                copyFromCaseId = null,
                caseLanguageId = SessionFacade.CurrentCaseLanguageId
            };

            m.NewModeParams = caseParam;
            AddViewDataValues();

            // Positive: Send Mail to...
            if (m.CaseMailSetting.DontSendMailToNotifier == false)
                m.CaseMailSetting.DontSendMailToNotifier = true;
            else
                m.CaseMailSetting.DontSendMailToNotifier = false;

            return this.View("New", m);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult NewAndGotoParentCase(CaseEditInput m)
        {
            if (m.ParentId == null)
            {
                throw new ArgumentException("bad input");
            }

            int id = this.Save(m);
            var url = this.GetLinkWithHash(ChildCasesHashTab, new { id = m.ParentId }, "Edit");
            return this.Redirect(url);
        }

        #endregion

        #region --Related Case--

        [HttpGet]
        public ViewResult RelatedCases(int caseId, string userId)
        {
            var relatedCases = this._caseService.GetCaseRelatedCases(
                                                caseId,
                                                this.workContext.Customer.CustomerId,
                                                userId,
                                                SessionFacade.CurrentUser);

            var model = this.caseModelFactory.GetRelatedCasesModel(relatedCases, this.workContext.Customer.CustomerId, userId);
            return this.View(model);
        }

        [HttpGet]
        public ViewResult RelatedCasesFull(int caseId, string userId)
        {
            var sortBy = CaseSortField.CaseNumber;
            var sortByAsc = true;
            if (SessionFacade.CurrentCaseSearch != null &&
                SessionFacade.CurrentCaseSearch.Search != null)
            {
                sortBy = SessionFacade.CurrentCaseSearch.Search.SortBy;
                sortByAsc = SessionFacade.CurrentCaseSearch.Search.Ascending;
            }

            var model = this.caseModelFactory.GetRelatedCasesFullModel(
                                                null,
                                                userId,
                                                caseId,
                                                sortBy,
                                                sortByAsc);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult RelatedCasesFullContent(int caseId, string userId, string sortBy, string sortByAsc)
        {
            var model = this.GetRelatedCasesViewModel(sortBy, sortByAsc, caseId, userId);
            return this.PartialView(model);
        }

        [ValidateInput(false)]
        [HttpGet]
        public JsonResult RelatedCasesCount(int caseId, string userId)
        {
            var count = this._caseService.GetCaseRelatedCasesCount(
                                                caseId,
                                                this.workContext.Customer.CustomerId,
                                                userId,
                                                SessionFacade.CurrentUser);
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region --Othres--

        [HttpGet]
        public ViewResult CaseByIds(string caseIds, string sortBy, string sortByAsc)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, null, null, caseIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            var model = new CaseByIdsViewModel(cases, sortBy, sortByAsc.convertStringToBool(), caseIds);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult CaseByIdsContent(string caseIds, string sortBy, string sortByAsc)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, null, null, caseIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            var model = new CaseByIdsViewModel(cases, sortBy, sortByAsc.convertStringToBool(), caseIds);
            return this.PartialView(model);
        }

        #endregion

        #region --General Methods--

        [HttpGet]
        public RedirectToRouteResult ChangeCurrentLanguage(int languageId)
        {
            SessionFacade.CurrentLanguageId = languageId;

            var language = _languageService.GetLanguage(languageId);

            if (language != null)
            {
                SessionFacade.CurrentLanguageCode = language.LanguageID;
            }

            var prevInfo = this.ExtractPreviousRouteInfo();
            var refreshRouteInfo = new RouteValueDictionary();

            if (prevInfo != null && prevInfo.Any())
            {
                foreach (var p in prevInfo)
                {
                    if (p.Key.ToLower() == "language")
                        refreshRouteInfo.Add(p.Key, language.LanguageID);
                    else
                        refreshRouteInfo.Add(p.Key, p.Value);
                }
            }

            var res = new RedirectToRouteResult(refreshRouteInfo);
            return res;
        }

        #endregion

        #region --Developer Test--
        /****  Case SLA Calculation simulator ***/
        // ** This function is using for developing test **//
        public JsonResult CalculateSLA(int caseId, int userId, DateTime simulateTime)
        {
            var utcNow = simulateTime;
            var case_ = this._caseService.GetCaseById(caseId);
            if (case_ == null || case_.Id < 1)
                return Json("There is no case info for case id:" + caseId.ToString());

            var user = this._userService.GetUser(userId);
            if (user == null || user.Id < 1)
                return Json("User number is not exist:" + userId.ToString());

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

            var curCustomer = _customerService.GetCustomer(case_.Customer_Id);
            var caseRegTime = DateTime.SpecifyKind(case_.RegTime, DateTimeKind.Utc);

            bool edit = true;
            // get case as it was before edit           
            var curCase = this._caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < simulateTime)
                                                                              .OrderByDescending(x => x.CreatedDate)
                                                                              .FirstOrDefault();
            var oldCase = this._caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < curCase.CreatedDate)
                                                                            .OrderByDescending(x => x.CreatedDate)
                                                                            .FirstOrDefault();
            var externalTime = 0;
            if (edit)
            {
                #region Editing existing case
                //oldCase = this._caseService.GetDetachedCaseById(case_.Id);


                if (curCase == null)
                    return Json("There is no history about case");

                var oldCase_StateSecondary_Id = oldCase.StateSecondary_Id;
                if (oldCase_StateSecondary_Id.HasValue)
                {
                    var caseSubState = this._stateSecondaryService.GetStateSecondary(oldCase_StateSecondary_Id.Value);

                    // calculating time spent in "inactive" state since last changing every save
                    if (caseSubState.IncludeInCaseStatistics == 0)
                    {
                        var workTimeCalcFactory =
                            new WorkTimeCalculatorFactory(
                                ManualDependencyResolver.Get<IHolidayService>(),
                                curCustomer.WorkingDayStart,
                                curCustomer.WorkingDayEnd,
                                TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId));
                        int[] deptIds = null;

                        var caseDepartmentId = curCase.Department_Id;
                        if (caseDepartmentId.HasValue)
                        {
                            deptIds = new int[] { caseDepartmentId.Value };
                        }

                        var oldCase_ChangeTime = oldCase.CreatedDate;
                        var oldCase_DepartmentId = oldCase.Department_Id;
                        var workTimeCalc = workTimeCalcFactory.Build(oldCase_ChangeTime, utcNow, deptIds);
                        externalTime = workTimeCalc.CalculateWorkTime(
                            oldCase_ChangeTime,
                            utcNow,
                            oldCase_DepartmentId) + oldCase.ExternalTime;
                    }
                }


                #endregion
            }

            var case_FinishingDate = simulateTime;

            var workTimeCalcFactory2 = new WorkTimeCalculatorFactory(
                ManualDependencyResolver.Get<IHolidayService>(),
                curCustomer.WorkingDayStart,
                curCustomer.WorkingDayEnd,
                TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId));
            int[] deptIds2 = null;

            if (curCase.Department_Id.HasValue)
            {
                deptIds2 = new int[] { curCase.Department_Id.Value };
            }

            var workTimeCalc2 = workTimeCalcFactory2.Build(caseRegTime, case_FinishingDate, deptIds2);
            var leadTime = workTimeCalc2.CalculateWorkTime(
                caseRegTime,
                case_FinishingDate,
                curCase.Department_Id) - externalTime;

            var msg = string.Format("System >> LeadTime:{0} ExternalTime:{1} <br/> Simulator >> LeadTime:{2} ExternalTime:{3}",
                                    case_.LeadTime, case_.ExternalTime, leadTime, externalTime);

            return Json(msg);
        }

        #endregion

        #region --Invoice--

       

        #endregion

        #region --Ajax--

        public JsonResult SaveStatisticsStateInSession(string value)
        {
            SessionFacade.CaseOverviewGridSettings.ExpandedStatistics = value;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region ***Private Methods***

        #region --Case Template--

        private void CheckTemplateParameters(int? templateId, int caseId)
        {
            if (templateId.HasValue)
            {
                // check template parameters
                var template = _caseSolutionService.GetCaseSolution(templateId.Value);

                // if formGuid this indicates that we want to initate a form by this guid.
                if (template.FormGUID.HasValue)
                {
                    _caseSolutionService.SaveEmptyForm(template.FormGUID.Value, caseId);
                }
            }
        }

        private CaseTemplateTreeModel GetCaseTemplateTreeModel(int customerId, int userId, CaseSolutionLocationShow location)
        {
            var model = new CaseTemplateTreeModel();
            model.CustomerId = customerId;
            model.CaseTemplateCategoryTree = _caseSolutionService.GetCaseSolutionCategoryTree(customerId, userId, location);
            return model;
        }

        #endregion

        #region --Save/Update--
        
        /// <summary>
        /// @TODO: this method should defenitely be moved to service in future
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private int Save(CaseEditInput m)
        {
            var utcNow = DateTime.UtcNow;
            var case_ = m.case_;
            var caseLog = m.caseLog;
            var caseMailSetting = m.caseMailSetting;
            var updateNotifierInformation = m.updateNotifierInformation;            
            case_.Performer_User_Id = m.Performer_Id;
            case_.CaseResponsibleUser_Id = m.ResponsibleUser_Id;            
            case_.RegistrationSourceCustomer_Id = m.customerRegistrationSourceId;
            case_.Ou = null;
            case_.Department = null;
            case_.Region = null;
            bool edit = case_.Id != 0;
            var isItChildCase = m.ParentId.HasValue;
            Case parentCase = null;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);          

            if (isItChildCase)
            {
                parentCase = this._caseService.GetCaseById(m.ParentId.Value);
                if (parentCase == null)
                {
                    throw new ArgumentException("parentCaseId has an invalid value");
                }
            }

            IDictionary<string, string> errors;
            var mailSenders = new MailSenders();
            if (case_.RegLanguage_Id == 0)
            {
                case_.RegLanguage_Id = SessionFacade.CurrentLanguageId;
            }


            if (case_.IsAbout != null)
                case_.IsAbout.Id = case_.Id;


            // Positive: Send Mail to...
            if (caseMailSetting.DontSendMailToNotifier == false)
                caseMailSetting.DontSendMailToNotifier = true;
            else
                caseMailSetting.DontSendMailToNotifier = false;

            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

            if (case_.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(case_.WorkingGroup_Id.Value);
                if (curWG != null)
                    if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                        mailSenders.WGEmail = curWG.EMail;
            }

            if (case_.DefaultOwnerWG_Id.HasValue && case_.DefaultOwnerWG_Id.Value > 0)
            {
                var defaultWGEmail = _workingGroupService.GetWorkingGroup(case_.DefaultOwnerWG_Id.Value).EMail;
                mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
            }

            // get case as it was before edit
            var curCustomer = _customerService.GetCustomer(case_.Customer_Id);
            if (curCustomer == null)
            {
                throw new ArgumentException("Case customer has an invalid value");
            }

            DHDomain.Case oldCase = new DHDomain.Case();
            if (edit)
            {                
                #region Editing existing case
                oldCase = this._caseService.GetDetachedCaseById(case_.Id);
                var cu = this._customerUserService.GetCustomerSettings(case_.Customer_Id, SessionFacade.CurrentUser.Id);
                if (cu != null)
                {
                    if (cu.UserInfoPermission == 0)
                    {
                        case_.ReportedBy = oldCase.ReportedBy;
                        case_.Place = oldCase.Place;
                        case_.PersonsName = oldCase.PersonsName;
                        case_.PersonsEmail = oldCase.PersonsEmail;
                        case_.PersonsPhone = oldCase.PersonsPhone;
                        case_.PersonsCellphone = oldCase.PersonsCellphone;
                        case_.Region_Id = oldCase.Region_Id;
                        case_.Department_Id = oldCase.Department_Id;
                        case_.OU_Id = oldCase.OU_Id;
                        case_.UserCode = oldCase.UserCode;
                    }                    
                }

                if (oldCase.StateSecondary_Id.HasValue)
                {
                    var caseSubState = this._stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                    // calculating time spent in "inactive" state since last changing every save
                    if (caseSubState.IncludeInCaseStatistics == 0)
                    {
                        var workTimeCalcFactory =
                            new WorkTimeCalculatorFactory(
                                ManualDependencyResolver.Get<IHolidayService>(),
                                curCustomer.WorkingDayStart,
                                curCustomer.WorkingDayEnd,
                                TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        int[] deptIds = null;
                        if (case_.Department_Id.HasValue)
                        {
                            deptIds = new int[] { case_.Department_Id.Value };
                        }

                        var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, utcNow, deptIds);
                        case_.ExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            utcNow,
                            oldCase.Department_Id) + oldCase.ExternalTime;
                    }
                }

                case_.RegTime = DateTime.SpecifyKind(oldCase.RegTime, DateTimeKind.Utc);
                #endregion
            }
            else
            {
                #region NewCase

                case_.RegTime = utcNow;

                #endregion
            }

            case_.LatestSLACountDate = CalculateLatestSLACountDate(oldCase.StateSecondary_Id, case_.StateSecondary_Id, oldCase.LatestSLACountDate);
            
            var leadTime = 0; 
            if (caseLog != null && caseLog.FinishingType > 0)
            {
                if (caseLog.FinishingDate == null)
                {
                    caseLog.FinishingDate = utcNow;
                }
                else
                {
                    // för att få med klockslag
                    if (caseLog.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        caseLog.FinishingDate = utcNow;
                    }
                    else
                    {
                        caseLog.FinishingDate = DateTime.SpecifyKind(caseLog.FinishingDate.Value, DateTimeKind.Local).ToUniversalTime();
                    }
                }

                case_.FinishingDate = DatesHelper.Max(case_.RegTime, caseLog.FinishingDate.Value);

                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    curCustomer.WorkingDayStart,
                    curCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                int[] deptIds = null;
                if (case_.Department_Id.HasValue)
                {
                    deptIds = new int[] { case_.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, case_.FinishingDate.Value, deptIds);
                leadTime = workTimeCalc.CalculateWorkTime(
                    case_.RegTime,
                    case_.FinishingDate.Value.ToUniversalTime(),
                    case_.Department_Id) - case_.ExternalTime;

                case_.LeadTime = leadTime;
            }
            else
            {                
                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    curCustomer.WorkingDayStart,
                    curCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                int[] deptIds = null;
                if (case_.Department_Id.HasValue)
                {
                    deptIds = new int[] { case_.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, utcNow, deptIds);
                leadTime = workTimeCalc.CalculateWorkTime(
                    case_.RegTime,
                    utcNow.ToUniversalTime(),
                    case_.Department_Id) - case_.ExternalTime;
            }

            var childCasesIds = this._caseService.GetChildCasesFor(case_.Id).Where(it => !it.ClosingDate.HasValue).Select(it => it.Id).ToArray();

            var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = leadTime };
            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(
                        case_,
                        caseLog,
                        caseMailSetting,
                        SessionFacade.CurrentUser.Id,
                        this.User.Identity.Name,
                        ei,
                        out errors,
                        parentCase);                       
            
            if (updateNotifierInformation.HasValue && updateNotifierInformation.Value)
            {
                var names = case_.PersonsName.Split(' ');

                var fName = string.Empty;
                var lName = string.Empty;

                if (names.Length > 0)
                {
                    for (int i = 0; i < names.Length - 1; i++)
                        if (i == 0)
                            fName = names[i];
                        else
                            fName += " " + names[i];
                }

                if (names.Length > 1)
                    lName = names[names.Length - 1];

                var caseNotifier = this.caseNotifierModelFactory.Create(
                                                            case_.ReportedBy,
                                                            fName,
                                                            lName,
                                                            case_.PersonsEmail,
                                                            case_.PersonsPhone,
                                                            case_.PersonsCellphone,
                                                            case_.Department_Id,
                                                            case_.OU_Id,
                                                            case_.Place,
                                                            case_.UserCode,
                                                            case_.Customer_Id);

                this.notifierService.UpdateCaseNotifier(caseNotifier);
            }

            // save log
            var temporaryLogFiles = this.userTemporaryFilesStorage.FindFiles(caseLog.LogGuid.ToString(), ModuleName.Log);
            caseLog.CaseId = case_.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            var orginalInternalLog = caseLog.TextInternal;
            
            if (caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value
                && !string.IsNullOrEmpty(caseLog.TextInternal) && childCasesIds.Length > 0)
            {
                caseLog.TextInternal = string.Format(
                    "[{0}]: {1}",
                    Translation.Get(CaseLog.ParentChildCasesMarker),
                    caseLog.TextInternal);
            }

            if (caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value && parentCase != null)
            {
                caseLog.TextInternal = string.Format(
                   "[{0}]: {1}",
                   Translation.Get(CaseLog.ChildParentCasesMarker),
                   caseLog.TextInternal);
            }

            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
            caseLog.TextInternal = orginalInternalLog;

            if (caseLog != null && caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value 
                && !string.IsNullOrEmpty(caseLog.TextInternal))
            {
                if (parentCase != null)
                {
                    var parentCaseLog = new CaseLog
                                            {
                                                CaseId = parentCase.Id,
                                                UserId = caseLog.UserId,
                                                TextInternal = string.Format("[{0} #{1}]: {2}", Translation.Get(CaseLog.ChildCaseMarker), case_.CaseNumber, caseLog.TextInternal)
                                            };
                    this.UpdateCaseLogForCase(parentCase, parentCaseLog);
                }
                
                if (childCasesIds != null && childCasesIds.Length > 0)
                {
                    caseLog.TextInternal = string.Format("[{0}]: {1}", Translation.Get(CaseLog.ParentCaseMarker), caseLog.TextInternal);
                    this._logService.AddParentCaseLogToChildCases(childCasesIds, caseLog);
                }
            }

            var basePath = _masterDataService.GetFilePath(case_.Customer_Id);
            // save case files
            if (!edit) 
            {
                var temporaryFiles = this.userTemporaryFilesStorage.FindFiles(case_.CaseGUID.ToString(), ModuleName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, case_.Id, this.workContext.User.UserId)).ToList();
                this._caseFileService.AddFiles(newCaseFiles);
            }

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, this.workContext.User.UserId)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            caseMailSetting.CustomeMailFromAddress = mailSenders;
            // send emails
            this._caseService.SendCaseEmail(case_.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, newLogFiles);

            //Unlock Case            
            if (m.caseLock != null && !string.IsNullOrEmpty(m.caseLock.LockGUID))
                this._caseLockService.UnlockCaseByGUID(new Guid(m.caseLock.LockGUID));

            // delete temp folders                
            this.userTemporaryFilesStorage.ResetCacheForObject(case_.CaseGUID.ToString());
            this.userTemporaryFilesStorage.ResetCacheForObject(caseLog.LogGuid.ToString());

            return case_.Id;
        }

        private DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;
            
            if (oldSubStateId.HasValue)
            {
                var oldSubStatus = _stateSecondaryService.GetStateSecondary(oldSubStateId.Value);
                if (oldSubStatus != null)
                    oldSubStateMode = oldSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (oldSubStateMode == -1 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode ==  1)
                ret = null;
            else if (oldSubStateMode ==  0 && newSubStateMode ==  1)
                ret = null;
            else if (oldSubStateMode ==  0 && newSubStateMode == -1)
                ret = null;            
            else if (oldSubStateMode == -1 && newSubStateMode ==  0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode ==  1 && newSubStateMode ==  0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode ==  1 && newSubStateMode == -1)
                ret = oldSLADate;
            else if (oldSubStateMode ==  1 && newSubStateMode ==  1)
                ret = oldSLADate;
            else if (oldSubStateMode ==  0 && newSubStateMode ==  0)
                ret = oldSLADate;
           
            return ret;
        }

        private void UpdateCaseLogForCase(Case @case, CaseLog caseLog)
        {
            if (@case == null || caseLog == null)
            {
                throw new ArgumentException("@case or/and caseLog is null");
            }

            IDictionary<string, string> errors;

            if (caseLog.FinishingType != null && caseLog.FinishingType != 0)
            {
                var c = this._caseService.GetCaseById(caseLog.CaseId);
                // save case and case history
                c.FinishingDescription = @case.FinishingDescription;
                var ei = new CaseExtraInfo() {CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = 0};
                int caseHistoryId = this._caseService.SaveCase(c, caseLog, null, SessionFacade.CurrentUser.Id, this.User.Identity.Name, ei, out errors);
                caseLog.CaseHistoryId = caseHistoryId;
            }

            this._logService.SaveLog(caseLog, 0, out errors);
        }
        
        #endregion

        #region --Get Data--

        /// <summary>
        /// The edit mode.
        /// </summary>
        /// <param name="m">
        /// The m.
        /// </param>
        /// <param name="topic">
        /// The topic.
        /// </param>
        /// <param name="departmensForUser">
        /// The departments for user.
        /// </param>
        /// <param name="accessToWorkinggroups">
        /// The access to working groups.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        /// 
        private Enums.AccessMode EditMode(CaseInputViewModel m, string topic, IList<DHDomain.Department> departmensForUser, 
                                          List<CustomerWorkingGroupForUser> accessToWorkinggroups, bool temporaryHasAccessToWG = false)
        {
            var gs = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (m == null)
            {
                return Enums.AccessMode.NoAccess;
            }

            if (SessionFacade.CurrentUser == null)
            {
                return Enums.AccessMode.NoAccess;
            }

            if (m.case_ == null)
            {
                return Enums.AccessMode.NoAccess;
            }

            if (departmensForUser != null)
            {
                var accessToDepartments = departmensForUser.Select(d => d.Id).ToList();
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
                    if (accessToDepartments.Count > 0 && m.case_.Department_Id.HasValue)
                    {
                        if (!accessToDepartments.Contains(m.case_.Department_Id.Value))
                        {
                            return Enums.AccessMode.NoAccess;
                        }
                    }
                }
            }

            // In new case shouldn't check
            if (accessToWorkinggroups != null && m.case_.Id != 0)
            {
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
                    if (accessToWorkinggroups.Count > 0 && m.case_.WorkingGroup_Id.HasValue)
                    {
                        var wg = accessToWorkinggroups.FirstOrDefault(w => w.WorkingGroup_Id == m.case_.WorkingGroup_Id.Value);
                        if (wg == null && (gs != null && gs.LockCaseToWorkingGroup == 1))
                        {
                            if (!temporaryHasAccessToWG)
                              return Enums.AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {
                            if (!temporaryHasAccessToWG)
                                return Enums.AccessMode.ReadOnly;
                        }
                    }
                }
            }

            if (m.case_.FinishingDate.HasValue)
            {
                return Enums.AccessMode.ReadOnly;
            }

            if (m.CaseLock != null && m.CaseLock.IsLocked)
            {
                return Enums.AccessMode.ReadOnly;
            }

            if (topic == ModuleName.Log
                && SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator
                && SessionFacade.CurrentUser.Id != m.CaseLog.UserId)
            {
                return Enums.AccessMode.ReadOnly;
            }

            return Enums.AccessMode.FullAccess;
        }
        
        private string GetIdsFromSearchResult(IList<CaseSearchResult> cases)
        {
            if (cases == null)
                return string.Empty;

            return string.Join(",", cases.Select(c => c.Id));
        }        

      
        
        private List<string> GetInactiveFieldsValue(CaseMasterDataFieldsModel fields)
        {
            var ret = new List<string>();           

            if (fields == null || fields.CustomerId <= 0)
            {
                throw new ArgumentException("@Customer is not valid!");
            }

            if (fields.RegionId.HasValue)
            {
                var region = this._regionService.GetRegion(fields.RegionId.Value);
                if (region != null && region.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.Region_Id.ToString(), 
                                            Enums.TranslationSource.CaseTranslation, 
                                            fields.CustomerId)));
            }
            if (fields.CaseTypeId.HasValue)
            {
                var caseType = this._caseTypeService.GetCaseType(fields.CaseTypeId.Value);
                if (caseType != null && caseType.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.ProductAreaId.HasValue)
            {
                var productArea = this._productAreaService.GetProductArea(fields.ProductAreaId.Value);
                if (productArea != null && productArea.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.CategoryId.HasValue)
            {
                var category = this._categoryService.GetCategory(fields.CategoryId.Value, fields.CustomerId);
                if (category != null && category.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SupplierId.HasValue)
            {
                var supplier = this._supplierService.GetSupplier(fields.SupplierId.Value);
                if (supplier != null && supplier.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.PriorityId.HasValue)
            {
                var priority = this._priorityService.GetPriority(fields.PriorityId.Value);
                if (priority != null && priority.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.StatusId.HasValue)
            {
                var status = this._statusService.GetStatus(fields.StatusId.Value);
                if (status != null && status.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SubStatusId.HasValue)
            {
                var subStatus = this._stateSecondaryService.GetStateSecondary(fields.SubStatusId.Value);
                if (subStatus != null && subStatus.IsActive == 0)
                    ret.Add(string.Format("[{0}]",Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.WorkingGroupId.HasValue)
            {
                var workingGroup = this._workingGroupService.GetWorkingGroup(fields.WorkingGroupId.Value);
                if (workingGroup != null && workingGroup.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            return ret;
        }

        private string GetDepartmentsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var depIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) > 0)
                        depIds.Add(int.Parse(ids[i]));
            }
            if (depIds.Any())
                ret = string.Join(",", depIds);
            return ret;
        }

        private string GetOrganizationUnitsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var ouIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) < 0)
                        ouIds.Add(-int.Parse(ids[i]));
            }
            if (ouIds.Any())
                ret = string.Join(",", ouIds);
            return ret;
        }

        private CaseRemainingTimeTable GetRemainigTimeById(RemainingTimes remainigTimeId)
        {
            var ret = new CaseRemainingTimeTable();

            switch (remainigTimeId)
            {
                case RemainingTimes.Delayed:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, true);
                    break;

                case RemainingTimes.OneHour:
                    ret = new CaseRemainingTimeTable(1, null, 5, true);
                    break;

                case RemainingTimes.TwoHours:
                    ret = new CaseRemainingTimeTable(2, null, 5, true);
                    break;

                case RemainingTimes.FourHours:
                    ret = new CaseRemainingTimeTable(2, 4, 5, true);
                    break;

                case RemainingTimes.EightHours:
                    ret = new CaseRemainingTimeTable(4, 8, 5, true);
                    break;

                case RemainingTimes.OneDay:
                    ret = new CaseRemainingTimeTable(1, null, 5, false);
                    break;

                case RemainingTimes.TwoDays:
                    ret = new CaseRemainingTimeTable(2, null, 5, false);
                    break;

                case RemainingTimes.ThreeDays:
                    ret = new CaseRemainingTimeTable(3, null, 5, false);
                    break;

                case RemainingTimes.FourDays:
                    ret = new CaseRemainingTimeTable(4, null, 5, false);
                    break;

                case RemainingTimes.FiveDays:
                    ret = new CaseRemainingTimeTable(5, null, 5, false);
                    break;

                case RemainingTimes.MaxDays:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, false);
                    break;
                default:
                    ret = null;
                    break;
            }

            return ret;

        }

        #endregion

        #region --Initiating--

        private CaseSearchFilterData CreateCaseSearchFilterData(int cusId, UserOverview userOverview, DHDomain.CustomerUser cu, CaseSearchModel sm)
        {
            var userId = userOverview.Id;
            var fd = new CaseSearchFilterData
            {
                customerUserSetting = cu,
                customerSetting = this._settingService.GetCustomerSetting(cusId),
                filterCustomerId = cusId
            };

            //region
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter))
                fd.filterRegion = this._regionService.GetRegions(cusId);

            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseDepartmentFilter))
            {
                const bool IsTakeOnlyActive = false;
                fd.filterDepartment = this._departmentService.GetDepartmentsByUserPermissions(
                    userId,
                    cusId,
                    IsTakeOnlyActive);
                if (!fd.filterDepartment.Any())
                {
                    fd.filterDepartment =
                        this._departmentService.GetDepartments(cusId)
                            .Where(
                                d =>
                                d.Region_Id == null
                                || IsTakeOnlyActive == false
                                || (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0))
                            .ToList();
                }

                if(fd.customerSetting != null && fd.customerSetting.ShowOUsOnDepartmentFilter != 0)
                    fd.filterDepartment = AddOrganizationUnitsToDepartments(fd.filterDepartment);                
            }

            //ärendetyp
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter))
            {
                const bool IsTakeOnlyActive = true;
                fd.filterCaseType = this._caseTypeService.GetCaseTypes(cusId, IsTakeOnlyActive);
            }

            //working group
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseWorkingGroupFilter))
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
                const bool isTakeOnlyActive = true;
                if (gs.LockCaseToWorkingGroup == 0)
                    fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId);
                else
                {
                    if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                        fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive);
                    else
                        fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);

                    //var isAdmin = SessionFacade.CurrentUser.IsAdministrator();
                    //if (isAdmin)                    
                    //    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);                           
                    //else
                    //    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive);                                     
                }
            }

            //produktonmråde
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseProductAreaFilter))
            {
                const bool isTakeOnlyActive = true;
                fd.filterProductArea = this._productAreaService.GetTopProductAreasForUser(
                    cusId,
                    SessionFacade.CurrentUser,
                    isTakeOnlyActive);
            }

            //kategori                        
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCategoryFilter))
                fd.filterCategory = this._categoryService.GetActiveCategories(cusId);
            //prio
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePriorityFilter))
                fd.filterPriority = this._priorityService.GetPriorities(cusId);
            //status
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter))
                fd.filterStatus = this._statusService.GetStatuses(cusId);
            //understatus
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter))
                fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);

            fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser.FollowUpPermission, cusId);            
            fd.CaseRegistrationDateStartFilter = fd.customerUserSetting.CaseRegistrationDateStartFilter;
            fd.CaseRegistrationDateEndFilter = fd.customerUserSetting.CaseRegistrationDateEndFilter;
            fd.CaseWatchDateStartFilter = fd.customerUserSetting.CaseWatchDateStartFilter;
            fd.CaseWatchDateEndFilter = fd.customerUserSetting.CaseWatchDateEndFilter;
            fd.CaseClosingDateStartFilter = fd.customerUserSetting.CaseClosingDateStartFilter;
            fd.CaseClosingDateEndFilter = fd.customerUserSetting.CaseClosingDateEndFilter;
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseClosingReasonFilter))
            {
                fd.ClosingReasons = this._finishingCauseService.GetFinishingCauses(cusId);
            }

            fd.caseSearchFilter = sm.caseSearchFilter;
            fd.CaseClosingDateEndFilter = sm.caseSearchFilter.CaseClosingDateEndFilter;
            fd.CaseClosingDateStartFilter = sm.caseSearchFilter.CaseClosingDateStartFilter;
            fd.CaseRegistrationDateEndFilter = sm.caseSearchFilter.CaseRegistrationDateEndFilter;
            fd.CaseRegistrationDateStartFilter = sm.caseSearchFilter.CaseRegistrationDateStartFilter;
            fd.CaseWatchDateEndFilter = sm.caseSearchFilter.CaseWatchDateEndFilter;
            fd.CaseWatchDateStartFilter = sm.caseSearchFilter.CaseWatchDateStartFilter;
            fd.CaseInitiatorFilter = sm.caseSearchFilter.Initiator;
            fd.SearchInMyCasesOnly = sm.caseSearchFilter.SearchInMyCasesOnly;

            //användare
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseUserFilter))
            {
                const bool IsTakeOnlyActive = true;
                fd.RegisteredByUserList = this._userService.GetUserOnCases(cusId, IsTakeOnlyActive).MapToSelectList(fd.customerSetting);
                if (!string.IsNullOrEmpty(fd.caseSearchFilter.User))
                {
                    fd.lstfilterUser = fd.caseSearchFilter.User.Split(',').Select(int.Parse).ToArray();
                }
            }

            //ansvarig
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseResponsibleFilter))
            {
                fd.ResponsibleUserList = this._userService.GetAvailablePerformersOrUserId(cusId).MapToSelectList(fd.customerSetting);
                if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserResponsible))
                {
                    fd.lstfilterResponsible = fd.caseSearchFilter.UserResponsible.Split(',').Select(int.Parse).ToArray();
                }
            }

            var performers = this._userService.GetAvailablePerformersOrUserId(cusId);
            //performers
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            fd.AvailablePerformersList = performers.MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);
            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            //Tid kvar 
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRemainingTimeFilter))
                fd.filterCaseRemainingTime = GetRemainigTimeList(cusId);           

            
            return fd;
        }

        private CaseSearchFilterData CreateAdvancedSearchFilterData(int cusId, int userId, CaseSearchModel sm, List<ItemOverview> customers)
        {
            var fd = new CaseSearchFilterData();

            fd.caseSearchFilter = sm.caseSearchFilter;
            fd.CaseInitiatorFilter = sm.caseSearchFilter.Initiator;
            fd.customerSetting = this._settingService.GetCustomerSetting(cusId);
            fd.filterCustomers = customers;
            fd.filterCustomerId = cusId;
            // Case #53981
            var userSearch = new UserSearch() { CustomerId = cusId, StatusId = 3 };
            fd.AvailablePerformersList = this._userService.SearchSortAndGenerateUsers(userSearch).MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);
            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            fd.filterCaseProgress = ObjectExtensions.GetFilterForAdvancedSearch();

            //Working group            
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            const bool IsTakeOnlyActive = false;
            if (gs.LockCaseToWorkingGroup == 0)
            {
                fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId, IsTakeOnlyActive);
            }
            else
            {
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                {
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, userId, IsTakeOnlyActive);
                }
                else
                {
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, IsTakeOnlyActive);
                }
            }

            fd.filterWorkingGroup.Insert(0, ObjectExtensions.notAssignedWorkingGroup());

            //Sub status            
            fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);
            fd.filterMaxRows = GetMaxRowsFilter();

            return fd;
        }

        private AdvancedSearchSpecificFilterData CreateAdvancedSearchSpecificFilterData(int userId, int customerId = 0)
        {
            var csm = new CaseSearchModel();

            // While customer is not changed (cusId == 0), should use session values for default filter
            if (customerId == 0)
            {
                csm = SessionFacade.CurrentAdvancedSearch;
                if (csm != null && csm.caseSearchFilter != null)
                    customerId = csm.caseSearchFilter.CustomerId;
            }

            var customerSetting = this._settingService.GetCustomerSetting(customerId);

            var specificFilter = new AdvancedSearchSpecificFilterData();

            specificFilter.CustomerId = customerId;
            specificFilter.CustomerSetting = this._settingService.GetCustomerSetting(customerId);

            specificFilter.FilteredCaseTypeText = ParentPathDefaultValue;
            specificFilter.FilteredProductAreaText = ParentPathDefaultValue;
            specificFilter.FilteredClosingReasonText = ParentPathDefaultValue;

            specificFilter.NewProductAreaList = GetProductAreasModel(customerId, null);

            var customerfieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.Department_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                const bool IsTakeOnlyActive = false;
                specificFilter.DepartmentList = this._departmentService.GetDepartmentsByUserPermissions(
                    userId,
                    customerId,
                    IsTakeOnlyActive);
                if (!specificFilter.DepartmentList.Any())
                {
                    specificFilter.DepartmentList =
                        this._departmentService.GetDepartments(customerId, ActivationStatus.All)
                        .ToList();
                }

                if (customerSetting != null && customerSetting.ShowOUsOnDepartmentFilter != 0)
                    specificFilter.DepartmentList = AddOrganizationUnitsToDepartments(specificFilter.DepartmentList);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.StateSecondaryList = this._stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.PriorityList = this._priorityService.GetPriorities(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.ClosingReason.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.ClosingReasonList = this._finishingCauseService.GetFinishingCauses(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {

                specificFilter.CaseTypeList = this._caseTypeService.GetCaseTypes(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                const bool isTakeOnlyActive = false;
                specificFilter.ProductAreaList = this._productAreaService.GetTopProductAreasForUser(
                    customerId,
                    SessionFacade.CurrentUser,
                    isTakeOnlyActive);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
                const bool IsTakeOnlyActive = false;
                if (gs.LockCaseToWorkingGroup == 0)
                {
                    specificFilter.WorkingGroupList = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
                }
                else
                {
                    if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                    {
                        specificFilter.WorkingGroupList = this._workingGroupService.GetWorkingGroups(customerId, userId, IsTakeOnlyActive);
                    }
                    else
                    {
                        specificFilter.WorkingGroupList = this._workingGroupService.GetWorkingGroups(customerId, IsTakeOnlyActive);
                    }
                    
                }

                specificFilter.WorkingGroupList.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
            }

            if (csm != null && csm.caseSearchFilter != null)
            {
                specificFilter.FilteredDepartment = csm.caseSearchFilter.Department;
                specificFilter.FilteredPriority = csm.caseSearchFilter.Priority;
                specificFilter.FilteredStateSecondary = csm.caseSearchFilter.StateSecondary;
                specificFilter.FilteredCaseType = csm.caseSearchFilter.CaseType;
                specificFilter.FilteredWorkingGroup = csm.caseSearchFilter.WorkingGroup;
                if (specificFilter.FilteredCaseType > 0)
                {
                    var c = this._caseTypeService.GetCaseType(specificFilter.FilteredCaseType);
                    if (c != null)
                        specificFilter.FilteredCaseTypeText = c.getCaseTypeParentPath();
                }

                specificFilter.FilteredProductArea = csm.caseSearchFilter.ProductArea;
                if (!string.IsNullOrWhiteSpace(specificFilter.FilteredProductArea))
                {
                    if (specificFilter.FilteredProductArea != "0")
                    {
                        var p = this._productAreaService.GetProductArea(specificFilter.FilteredProductArea.convertStringToInt());
                        if (p != null)
                        {
                            specificFilter.FilteredProductAreaText = string.Join(
                                " - ",
                                this._productAreaService.GetParentPath(p.Id, customerId));
                        }
                    }
                }

                specificFilter.FilteredClosingReason = csm.caseSearchFilter.CaseClosingReasonFilter;
                if (!string.IsNullOrWhiteSpace(specificFilter.FilteredClosingReason))
                {
                    if (specificFilter.FilteredClosingReason != "0")
                    {
                        var fc =
                            this._finishingCauseService.GetFinishingCause(
                                specificFilter.FilteredClosingReason.convertStringToInt());
                        if (fc != null)
                        {
                            specificFilter.FilteredClosingReasonText = fc.GetFinishingCauseParentPath();
                        }
                    }
                }

            }

            return specificFilter;
        }

        #endregion

        #region --Othres--

        private IList<Department> AddOrganizationUnitsToDepartments(IList<Department> departments)
        {            
            if (departments.Any())
            {
                var allOUsNeeded = this._ouService.GetAllOUs()
                                                  .Where(o => o.Department_Id.HasValue)
                                                  .Where(o => departments.Select(d => d.Id).ToList().Contains(o.Department_Id.Value))
                                                  .ToList();
                var dep_Ou = new List<Department>();
                foreach (var dep in departments)
                {
                    var currentOUsForDep = allOUsNeeded.Where(o => o.Department_Id == dep.Id).ToList();
                    foreach (var o in currentOUsForDep)
                    {
                        /*Attention: As we make combination of Department and OU list in a one filter in case overview, 
                        we use -OU.id (Id with minus sign) to detect it is OU.Id (Not Department.Id) and 
                        then for fetching data we convert Negative to positive, Also they are storing Negative in the Session*/
                        var newDep = new Department()
                        {
                            Id = -o.Id,
                            DepartmentName = dep.DepartmentName + " - " + (o.Parent_OU_Id.HasValue? o.Parent.Name + " - " + o.Name : o.Name),
                            IsActive = 1
                        };
                        dep_Ou.Add(newDep);
                    }
                }

                foreach (var newDep_OU in dep_Ou.ToList())
                    departments.Add(newDep_OU);

                departments = departments.OrderBy(d => d.DepartmentName).ToList();
            }

            return departments;
        }
        
        #endregion        

        #region --Resolvers--

        private void ResolveParentPathesForFilter(CaseSearchFilter f)
        {
            // hämta parent path för casetype
            if (f.CaseType > 0)
            {
                var c = this._caseTypeService.GetCaseType(f.CaseType);
                if (c != null)
                {
                    f.ParantPath_CaseType = c.getCaseTypeParentPath();
                }
            }

            if (!string.IsNullOrWhiteSpace(f.ProductArea))
            {
                if (f.ProductArea != "0")
                {
                    var p =
                        this._productAreaService.GetProductArea(
                            f.ProductArea.convertStringToInt());
                    if (p != null)
                    {
                        f.ParantPath_ProductArea = string.Join(
                            " - ",
                            this._productAreaService.GetParentPath(p.Id, SessionFacade.CurrentCustomer.Id));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(f.CaseClosingReasonFilter))
            {
                if (f.CaseClosingReasonFilter != "0")
                {
                    var fc =
                        this._finishingCauseService.GetFinishingCause(
                            f.CaseClosingReasonFilter.convertStringToInt());
                    if (fc != null)
                    {
                        f.ParentPathClosingReason = fc.GetFinishingCauseParentPath();
                    }
                }
            }
        }

        private IList<CaseSearchResult> TreeTranslate(IList<CaseSearchResult> cases, int customerId)
        {
            var ret = cases;
            var productareaCache = this._productAreaService.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => true);
            foreach (CaseSearchResult r in ret)
            {
                foreach (var c in r.Columns)
                {
                    if (c.TreeTranslation)
                    {
                        switch (c.Key.ToLower())
                        {
                            case "productarea_id":
                                if (productareaCache.ContainsKey(c.Id))
                                {
                                    var names = this._productAreaService.GetParentPath(c.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                                    c.StringValue = string.Join(" - ", names);
                                }

                                break;
                        }
                    }
                }
            }

            return ret;
        }

        private string GetDefaultFileName(string fileName)
        {
            var defaultFileName = fileName;
            if (Request.Browser.Browser.ToLower() == "ie" || Request.Browser.Browser.ToLower() == "internetexplorer")
            {
                defaultFileName = fileName.Replace("%", "");
                defaultFileName = defaultFileName.Replace("?", "");
            }
            return defaultFileName;
        }

        private DHDomain.CaseType TranslateCaseType(DHDomain.CaseType caseType)
        {
            if (caseType.ParentCaseType != null)
                caseType.ParentCaseType = TranslateCaseType(caseType.ParentCaseType);

            caseType.Name = Translation.Get(caseType.Name);

            return caseType;
        }

        private string GetFinishingCauseFullPath(
                        FinishingCauseInfo[] finishingCauses,
                        int? finishingCauseId)
        {
            if (!finishingCauseId.HasValue)
            {
                return string.Empty;
            }

            var finishingCause = finishingCauses.FirstOrDefault(f => f.Id == finishingCauseId);
            if (finishingCause == null)
            {
                return string.Empty;
            }

            var list = new List<FinishingCauseInfo>();
            var parent = finishingCause;
            do
            {
                list.Add(parent);
                parent = finishingCauses.FirstOrDefault(c => c.Id == parent.ParentId);
            }
            while (parent != null);

            return string.Join(" - ", list.Select(c => c.Name).Reverse());
        }        
        #endregion

        #region --Get Models--

        /// <summary>
        /// The get case input view model.
        /// </summary>
        private CaseInputViewModel GetCaseInputViewModel(
            int userId,
            int customerId,
            int caseId,
            CaseLockModel caseLocked,
            string redirectFrom = "",
            string backUrl = null,
            int? templateId = null,
            int? copyFromCaseId = null,
            bool updateState = true,
            int? templateistrue = 0,
            int? parentCaseId = null)
        {
            var m = new CaseInputViewModel ();
            m.BackUrl = backUrl;
            m.CanGetRelatedCases = SessionFacade.CurrentUser.IsAdministrator();
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
            var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);
            var isCreateNewCase = caseId == 0;
            m.CaseLock = caseLocked;
            m.MailTemplates = this._mailTemplateService.GetCustomMailTemplates(customerId).ToList();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            if (!isCreateNewCase)
            {
                var markCaseAsRead = string.IsNullOrWhiteSpace(redirectFrom);
                m.case_ = this._caseService.GetCaseById(caseId);

                var editMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);
                if (m.case_.Unread != 0 && updateState && editMode == Enums.AccessMode.FullAccess)
                    this._caseService.MarkAsRead(caseId);

                customerId = customerId == 0 ? m.case_.Customer_Id : customerId;
                //SessionFacade.CurrentCaseLanguageId = m.case_.RegLanguage_Id;
                
                var userLocal_ChangeTime = TimeZoneInfo.ConvertTimeFromUtc(m.case_.ChangeTime, userTimeZone);
                m.ChangeTime = userLocal_ChangeTime;
            }

            var customerUserSetting = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (customerUserSetting == null)
            {
                throw new ArgumentException(string.Format("No customer settings for this customer '{0}' and user '{1}'", customerId, userId));
            }

            

            var case_ = m.case_;
            var customer = this._customerService.GetCustomer(customerId);
            var customerSetting = this._settingService.GetCustomerSetting(customerId);
            var outputFormatter = new OutputFormatter(customerSetting.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            m.OutFormatter = outputFormatter;
            m.customerUserSetting = customerUserSetting;
            m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
            m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
            m.DepartmentFilterFormat = customerSetting.DepartmentFilterFormat;
            m.ParantPath_CaseType = ParentPathDefaultValue;
            m.ParantPath_ProductArea = ParentPathDefaultValue;
            m.ParantPath_OU = ParentPathDefaultValue;
            m.MinWorkingTime = customerSetting.MinRegWorkingTime != 0 ? customerSetting.MinRegWorkingTime : 30;
            m.CaseFilesModel = new CaseFilesModel();
            m.LogFilesModel = new FilesModel();
            m.CaseFileNames = GetCaseFileNames(caseId.ToString());
            m.LogFileNames = GetLogFileNames(caseId.ToString());

            if (isCreateNewCase)
            {
                #region New case model initialization actions
                var identity = global::System.Security.Principal.WindowsIdentity.GetCurrent();
                var windowsUser = identity != null ? identity.Name : null;
                if (copyFromCaseId.HasValue)
                {
                    m.case_ = this._caseService.Copy(
                        copyFromCaseId.Value,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        this.Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        windowsUser);
                }
                else if (parentCaseId.HasValue)
                {
                    ParentCaseInfo parentCaseInfo;
                    m.case_ = this._caseService.InitChildCaseFromCase(
                       parentCaseId.Value,
                       userId,
                       this.Request.GetIpAddress(),
                       CaseRegistrationSource.Administrator,
                       windowsUser,
                       out parentCaseInfo);
                    m.ParentCaseInfo = parentCaseInfo.MapBusinessToWebModel(outputFormatter);
                }
                else
                {
                    m.case_ = this._caseService.InitCase(
                        customerId,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        this.Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        customerSetting,
                        windowsUser);
                }

                var defaultStateSecondary = this._stateSecondaryService.GetDefaultOverview(customerId);
                if (defaultStateSecondary != null)
                {
                    m.case_.StateSecondary_Id = int.Parse(defaultStateSecondary.Value);
                }
                #endregion
            }
            else
            {
                #region Existing case model initialization actions
                m.Logs = this._logService.GetCaseLogOverviews(caseId);

                bool UseVD = false;
                if (!string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
                {
                    UseVD = true;
                }

                var canDelete = (SessionFacade.CurrentUser.DeleteAttachedFilePermission == 1);
                m.SavedFiles = canDelete? string.Empty : m.CaseFileNames;                

                m.CaseFilesModel = new CaseFilesModel(caseId.ToString(global::System.Globalization.CultureInfo.InvariantCulture), this._caseFileService.GetCaseFiles(caseId, canDelete).OrderBy(x => x.CreatedDate).ToArray(), m.SavedFiles, UseVD);
                if (m.case_.User_Id.HasValue)
                {
                    m.RegByUser = this._userService.GetUser(m.case_.User_Id.Value);
                }
                if (m.Logs != null)
                {
                    var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
                    var lastLog = m.Logs.FirstOrDefault();
                    if (lastLog != null)
                    {
                        m.FinishingCause = this.GetFinishingCauseFullPath(finishingCauses.ToArray(), lastLog.FinishingType);
                    }
                }

                var childCases = this._caseService.GetChildCasesFor(caseId);
                m.ChildCaseViewModel = new ChildCaseViewModel
                {
                    Formatter = outputFormatter,
                    ChildCaseList = childCases
                };
                m.ClosedChildCasesCount = childCases.Count(it => it.ClosingDate != null);
                m.ParentCaseInfo = this._caseService.GetParentInfo(caseId).MapBusinessToWebModel(outputFormatter);

                #endregion
            }


            var caseTemplateButtons = _caseSolutionService.GetCaseSolutions(customerId)
                                                          .Where(c => c.Status != 0 && c.ShowInsideCase != 0 && c.ConnectedButton.HasValue)
                                                          .Select(c => new CaseTemplateButton() 
                                                                            { 
                                                                                CaseTemplateId = c.Id, 
                                                                                CaseTemplateName = c.Name,
                                                                                ButtonNumber = c.ConnectedButton.Value
                                                                            })
                                                          .OrderBy(c=> c.ButtonNumber)
                                                          .ToList();
            m.CaseTemplateButtons = caseTemplateButtons;

            m.CaseMailSetting = new CaseMailSetting(
                customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                RequestExtension.GetAbsoluteUrl(),
                customerSetting.DontConnectUserToWorkingGroup);

            m.CaseMailSetting.DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool();

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1)
            {
                const bool TAKE_ONLY_ACTIVE = true;
                m.caseTypes = this._caseTypeService.GetCaseTypes(customerId, TAKE_ONLY_ACTIVE);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1)
            {
                m.categories = this._categoryService.GetCategories(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1)
            {
                m.impacts = this._impactService.GetImpacts(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
            {
                m.priorities = this._priorityService.GetPriorities(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
            {
                m.productAreas = this._productAreaService.GetTopProductAreasForUserOnCase(
                    customerId,
                    m.case_.ProductArea_Id,
                    SessionFacade.CurrentUser);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1)
            {
                m.regions = this._regionService.GetRegions(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1)
            {
                m.statuses = this._statusService.GetStatuses(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1)
            {
                m.stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString()).ShowOnStartPage == 1)
            {
                m.suppliers = this._supplierService.GetSuppliers(customerId);
                m.countries = this._countryService.GetCountries(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.System_Id.ToString()).ShowOnStartPage == 1)
            {
                m.systems = this._systemService.GetSystems(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Urgency_Id.ToString()).ShowOnStartPage == 1)
            {
                m.urgencies = this._urgencyService.GetUrgencies(customerId);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CausingPart.ToString()).ShowOnStartPage == 1)
            {
                var causingParts = GetCausingPartsModel(customerId, m.case_.CausingPartId);              
                m.causingParts = causingParts;
                //#1
                //m.causingParts = this._causingPartService.GetCausingParts(customerId);
            }

            // "Workging group" field
            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
            {
                var IsTakeOnlyActive = isCreateNewCase;
                m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
            }

            if (isCreateNewCase && m.workingGroups != null && m.workingGroups.Count > 0)
            {
                var defWorkingGroup = m.workingGroups.Where(it => it.IsDefault == 1).FirstOrDefault();
                if (defWorkingGroup != null)
                {
                    m.case_.WorkingGroup_Id = defWorkingGroup.Id;
                }
            }

            // "RegistrationSourceCustomer" field
            if (m.caseFieldSettings.getCaseSettingsValue(
                    GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()).ShowOnStartPage == 1)
            {
                var customerSources =
                    this._registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId).ToArray();

                if (m.case_.RegistrationSourceCustomer_Id.HasValue)
                {
                    m.CustomerRegistrationSourceId = m.case_.RegistrationSourceCustomer_Id.Value;
                    m.SelectedCustomerRegistrationSource = m.case_.RegistrationSourceCustomer.SourceName;
                }
                else
                {
                    var defaultSource =
                        customerSources.Where(it => it.SystemCode == (int)CaseRegistrationSource.Administrator).FirstOrDefault();
                    if (isCreateNewCase && defaultSource != null)
                    {
                        m.CustomerRegistrationSourceId = defaultSource.Id;
                        m.SelectedCustomerRegistrationSource = defaultSource.SourceName;
                    }
                }

                m.CustomerRegistrationSources.AddRange(
                    customerSources.Select(
                        it => new SelectListItem()
                        {
                            Text = Translation.GetCoreTextTranslation(it.SourceName),
                            Value = it.Id.ToString(),
                            Selected = it.Id == m.CustomerRegistrationSourceId
                        }));
            }

            if (customerSetting.ModuleProject == 1)
            {
                m.projects = this._projectService.GetCustomerProjects(customerId);
            }

            if (customerSetting.ModuleChangeManagement == 1)
            {
                m.changes = this._changeService.GetChanges(customerId);
            }
            
            m.finishingCauses = this._finishingCauseService.GetFinishingCauses(customerId);
            m.problems = this._problemService.GetCustomerProblems(customerId);
            m.currencies = this._currencyService.GetCurrencies();
            var responsibleUsersList = this._userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
            m.projects = this._projectService.GetCustomerProjects(customerId);
            m.departments = deps.Any() ? deps :
                this._departmentService.GetDepartments(customerId)
                .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                .ToList();

            m.standardTexts = this._standardTextService.GetStandardTexts(customerId);
            m.Languages = this._languageService.GetActiveLanguages();
            m.SendToDialogModel = this.CreateNewSendToDialogModel(customerId, responsibleUsersList.ToList(), customerSetting);
            m.CaseLog = this._logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty);
            m.CaseKey = m.case_.Id == 0 ? m.case_.CaseGUID.ToString() : m.case_.Id.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
            m.LogKey = m.CaseLog.LogGuid.ToString();

            if (m.case_.Supplier_Id > 0 && m.suppliers != null)
            {
                var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                m.CountryId = sup.Country_Id.GetValueOrDefault();
            }

            if (isCreateNewCase)
            {
                #region New case initialize

                // Load template info
                if (templateId != null)
                {
                    var caseTemplate = this._caseSolutionService.GetCaseSolution(templateId.Value);
                    var caseTemplateSettings =
                        this.caseSolutionSettingService.GetCaseSolutionSettingOverviews(templateId.Value);

                    if (caseTemplateSettings.Any())
                    {
                        m.CaseSolutionSettingModels = CaseSolutionSettingModel.CreateModel(caseTemplateSettings);
                    }

                    if (caseTemplate != null)
                    {
                        #region new case from template
                        if (caseTemplate.CaseType_Id != null)
                        {
                            m.case_.CaseType_Id = caseTemplate.CaseType_Id.Value;
                        }

                        if (caseTemplate.PerformerUser_Id != null)
                        {
                            m.case_.Performer_User_Id = caseTemplate.PerformerUser_Id.Value;
                        }
                        else
                        {
                            m.case_.Performer_User_Id = 0;
                        }

                        if (SessionFacade.CurrentUser != null && caseTemplate.SetCurrentUserAsPerformer == 1)
                            m.case_.Performer_User_Id = SessionFacade.CurrentUser.Id;

                        if (caseTemplate.Category_Id != null)
                        {
                            m.case_.Category_Id = caseTemplate.Category_Id.Value;
                        }

                        if (caseTemplate.CausingPartId.HasValue)
                        {
                            m.case_.CausingPartId = caseTemplate.CausingPartId.Value;
                            if (m.causingParts != null)
                            {
                                var templateCausingPart = m.causingParts.Where(c => c.Value == caseTemplate.CausingPartId.Value.ToString()).SingleOrDefault();
                                if (templateCausingPart != null)
                                    templateCausingPart.Selected = true;
                            }
                        }

                        if (caseTemplate.UpdateNotifierInformation.HasValue)
                        {
                            m.UpdateNotifierInformation = caseTemplate.UpdateNotifierInformation.Value.ToBool();
                        }

                        if (caseTemplate.Supplier_Id != null)
                            m.case_.Supplier_Id = caseTemplate.Supplier_Id.Value;

                        m.case_.ReportedBy = caseTemplate.ReportedBy;
                        m.case_.Department_Id = caseTemplate.Department_Id;
                        m.CaseMailSetting.DontSendMailToNotifier = caseTemplate.NoMailToNotifier.ToBool();
                        m.case_.ProductArea_Id = caseTemplate.ProductArea_Id;
                        m.case_.Caption = caseTemplate.Caption;
                        m.case_.Description = caseTemplate.Description;
                        m.case_.Miscellaneous = caseTemplate.Miscellaneous;

                        if (caseTemplate.CaseWorkingGroup_Id != null)
                        {
                            m.case_.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
                        }

                        m.case_.Priority_Id = caseTemplate.Priority_Id;
                        m.case_.Project_Id = caseTemplate.Project_Id;
                        m.CaseLog.TextExternal = caseTemplate.Text_External;
                        m.CaseLog.TextInternal = caseTemplate.Text_Internal;
                        m.CaseLog.FinishingType = caseTemplate.FinishingCause_Id;
                        if (m.CaseLog.FinishingType.HasValue)
                            m.CaseLog.FinishingDate = DateTime.UtcNow;
                        m.case_.PersonsName = caseTemplate.PersonsName;
                        m.case_.PersonsEmail = caseTemplate.PersonsEmail;
                        m.case_.PersonsPhone = caseTemplate.PersonsPhone;                        
                        m.case_.Region_Id = caseTemplate.Region_Id;
                        m.case_.OU_Id = caseTemplate.OU_Id;
                        m.case_.Place = caseTemplate.Place;
                        m.case_.UserCode = caseTemplate.UserCode;
                        m.case_.Urgency_Id = caseTemplate.Urgency_Id;
                        m.case_.Impact_Id = caseTemplate.Impact_Id;
                        m.case_.InvoiceNumber = caseTemplate.InvoiceNumber;
                        m.case_.ReferenceNumber = caseTemplate.ReferenceNumber;
                        m.case_.Status_Id = caseTemplate.Status_Id;
                        m.case_.StateSecondary_Id = caseTemplate.StateSecondary_Id;
                        m.case_.Verified = caseTemplate.Verified;
                        m.case_.VerifiedDescription = caseTemplate.VerifiedDescription;
                        m.case_.SolutionRate = caseTemplate.SolutionRate;
                        m.case_.InventoryNumber = caseTemplate.InventoryNumber;
                        m.case_.InventoryType = caseTemplate.InventoryType;
                        m.case_.InventoryLocation = caseTemplate.InventoryLocation;
                        m.case_.System_Id = caseTemplate.System_Id;
                        m.case_.Currency = caseTemplate.Currency;
                        m.case_.Cost = caseTemplate.Cost;
                        m.case_.OtherCost = caseTemplate.OtherCost;
                        m.case_.Available = caseTemplate.Available;
                        m.case_.ContactBeforeAction = caseTemplate.ContactBeforeAction;
                        m.case_.CostCentre = caseTemplate.CostCentre;
                        m.case_.PersonsCellphone = caseTemplate.PersonsCellPhone;

                        if (m.case_.IsAbout == null)
                            m.case_.IsAbout = new CaseIsAboutEntity();

                        m.case_.IsAbout.Id = 0;
                        m.case_.IsAbout.ReportedBy = caseTemplate.IsAbout_ReportedBy;
                        m.case_.IsAbout.Person_Name = caseTemplate.IsAbout_PersonsName;
                        m.case_.IsAbout.Person_Email = caseTemplate.IsAbout_PersonsEmail;
                        m.case_.IsAbout.Person_Phone = caseTemplate.IsAbout_PersonsPhone;
                        m.case_.IsAbout.Person_Cellphone = caseTemplate.IsAbout_PersonsCellPhone;
                        m.case_.IsAbout.Region_Id = caseTemplate.IsAbout_Region_Id;
                        m.case_.IsAbout.Department_Id = caseTemplate.IsAbout_Department_Id;
                        m.case_.IsAbout.OU_Id = caseTemplate.IsAbout_OU_Id;
                        m.case_.IsAbout.CostCentre = caseTemplate.IsAbout_CostCentre;
                        m.case_.IsAbout.Place = caseTemplate.IsAbout_Place;
                        m.case_.IsAbout.UserCode = caseTemplate.UserCode;

                        if (caseTemplate.RegistrationSource.HasValue)
                        {
                            m.CustomerRegistrationSourceId = caseTemplate.RegistrationSource.Value;
                            var RegistrationSource = this._registrationSourceCustomerService.GetRegistrationSouceCustomer(caseTemplate.RegistrationSource.Value);
                            m.SelectedCustomerRegistrationSource = RegistrationSource.SourceName;
                        }

                        // "watch date" 
                        if (caseTemplate.WatchDate.HasValue)
                        {
                            m.case_.WatchDate = caseTemplate.WatchDate;
                        }
                        else
                        {
                            if (m.case_.Department_Id.HasValue && m.case_.Priority_Id.HasValue)
                            {
                                var dept = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                                var priority =
                                    m.priorities.Where(it => it.Id == m.case_.Priority_Id && it.IsActive == 1).FirstOrDefault();
                                if (dept != null && priority != null && priority.SolutionTime == 0)
                                {
                                    m.case_.WatchDate =
                                    this.watchDateCalendarServcie.GetClosestDateTo(
                                            dept.WatchDateCalendar_Id.Value,
                                            DateTime.UtcNow);
                                }
                            }
                        }

                        m.case_.Project_Id = caseTemplate.Project_Id;
                        m.case_.Problem_Id = caseTemplate.Problem_Id;
                        m.case_.Change_Id = caseTemplate.Change_Id;
                        m.case_.FinishingDate = caseTemplate.FinishingDate;
                        m.case_.FinishingDescription = caseTemplate.FinishingDescription;
                        m.case_.PlanDate = caseTemplate.PlanDate;
                        m.CaseTemplateName = caseTemplate.Name;
                        m.case_.SMS = caseTemplate.SMS;
                        // This is used for hide fields(which are not in casetemplate) in new case input
                        m.templateistrue = templateistrue;
                        var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
                        m.FinishingCause = this.GetFinishingCauseFullPath(
                            finishingCauses.ToArray(),
                            caseTemplate.FinishingCause_Id);
                        #endregion
                    }
                }
                #endregion
            }

            DHDomain.User admUser = null;
            if (m.case_.Performer_User_Id.HasValue)
            {
                admUser = this._userService.GetUser(m.case_.Performer_User_Id.Value);
            }

            var performersList = responsibleUsersList;
            if (customerSetting.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, m.case_.WorkingGroup_Id);
            }

            if (!performersList.Contains(admUser) && admUser != null)
            {
                performersList.Insert(0, admUser);
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1)
            {
                //m.ous = this._ouService.GetOUs(customerId);
                m.ous = this._organizationService.GetOUs(m.case_.Department_Id).ToList();
            }

            if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString()).ShowOnStartPage == 1)
            {
                if (m.case_.IsAbout != null)
                    m.isaboutous = this._organizationService.GetOUs(m.case_.IsAbout.Department_Id).ToList();
                else
                    m.isaboutous = null;
            }

            // hämta parent path för casetype
            if (m.case_.CaseType_Id > 0)
            {
                var c = this._caseTypeService.GetCaseType(m.case_.CaseType_Id);
                if (c != null)
                {
                    c = TranslateCaseType(c);
                    m.ParantPath_CaseType = c.getCaseTypeParentPath();
                    if (isCreateNewCase && c.User_Id.HasValue)
                    {
                        m.case_.Performer_User_Id = c.User_Id.Value;
                    }
                }
            }

            // hämta parent path för productArea 
            m.ProductAreaHasChild = 0;
            if (m.case_.ProductArea_Id.HasValue)
            {
                var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                if (p != null)
                {
                    var names =
                        this._productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                    m.ParantPath_ProductArea = string.Join(" - ", names);
                    if (p.SubProductAreas != null && p.SubProductAreas.Where(s => s.IsActive != 0).ToList().Count > 0)
                    {
                        m.ProductAreaHasChild = 1;
                    }
                }
            }
            

            // check department info
            m.ShowInvoiceFields = 0;
            if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
            {
                var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                if (d != null)
                {
                    m.ShowInvoiceFields = d.Charge;
                }
            }

            // check state secondary info
            m.CaseLog.SendMailAboutCaseToNotifier = customer.CommunicateWithNotifier.ToBool();

            m.Disable_SendMailAboutCaseToNotifier = false;
            if (m.case_.StateSecondary_Id > 0)
            {
                if (m.case_.StateSecondary != null)
                {
                    m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                }
            }

            var temporaryUserHasAccessToWG = redirectFrom.ToLower() == "save" ? true : false;
            m.EditMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups, temporaryUserHasAccessToWG);
            if (isCreateNewCase)
            {
                m.case_.DefaultOwnerWG_Id = null;
                if (m.case_.User_Id.HasValue && m.case_.User_Id != 0)
                {
                    // http://redmine.fastdev.se/issues/10997
                    /*var curUser = _userService.GetUser(m.case_.User_Id);                        

                    if (curUser.Default_WorkingGroup_Id != null)
                        m.case_.DefaultOwnerWG_Id = curUser.Default_WorkingGroup_Id;*/

                    var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(m.case_.User_Id.Value, m.case_.Customer_Id);
                    if (userDefaultWorkingGroupId.HasValue)
                    {
                        m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
                    }
                }
            }
            else
            {
                if (m.case_.DefaultOwnerWG_Id.HasValue && m.case_.DefaultOwnerWG_Id.Value > 0)
                {
                    m.CaseOwnerDefaultWorkingGroup = this._workingGroupService.GetWorkingGroup(m.case_.DefaultOwnerWG_Id.Value);
                }
            }

            // TODO: Should mix CustomerSettings & Setting                 
            m.CustomerSettings = this.workContext.Customer.Settings;
            m.Setting = customerSetting;

            // "Responsible"
            m.ResponsibleUser_Id = m.case_.CaseResponsibleUser_Id ?? 0;
            const bool isAddEmpty = true;
            m.ResponsibleUsersAvailable = responsibleUsersList.MapToSelectList(m.Setting, isAddEmpty);

            // "Administrator" (Performer)
            m.Performer_Id = m.case_.Performer_User_Id ?? 0;
            m.Performers = performersList.Where(it => it.IsActive == 1 && (it.Performer == 1 || it.Id == m.Performer_Id))
                .MapToSelectList(m.Setting, isAddEmpty);

            m.DynamicCase = this._caseService.GetDynamicCase(m.case_.Id);
            if (m.DynamicCase != null)
            {
                var l = m.Languages.Where(x => x.Id == SessionFacade.CurrentLanguageId).FirstOrDefault();
                m.DynamicCase.FormPath = m.DynamicCase.FormPath
                    .Replace("[CaseId]", m.case_.Id.ToString())
                    .Replace("[UserId]", SessionFacade.CurrentUser.UserId.ToString())
                    .Replace("[ApplicationType]", "HD5")
                    .Replace("[Language]", l.LanguageId);
                m.DynamicCase.FormPath = m.DynamicCase.FormPath.Replace(@"\", @"\\"); //this is because users with backslash in name will have issues with container.js
            }

            
            if (case_ != null)
            {
                m.MapCaseToCaseInputViewModel(case_, userTimeZone);
            }

            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.InsideTheCase);

            m.CasePrintView = new ReportModel(false);
           
            return m;
        }

        private CaseSearchResultModel GetUnfilteredCases(
                   string sortBy,
                   string sortByAsc,
                   int? relatedCasesCaseId = null,
                   string relatedCasesUserId = null,
                   int[] caseIds = null)
        {
            var searchResult = new CaseSearchResultModel
            {
                GridSettings =
                    this.caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id),
                caseSettings = this._caseSettingService.GetCaseSettingsWithUser(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId)
            };
            var search = this.InitEmptySearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            search.Search.SortBy = sortBy ?? string.Empty;
            search.Search.Ascending = sortByAsc.convertStringToBool();
            search.caseSearchFilter.CaseProgress = CaseProgressFilter.None;
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id).ToArray();
            searchResult.cases = this._caseSearchService.Search(
                search.caseSearchFilter,
                searchResult.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                search.Search,
                this.workContext.Customer.WorkingDayStart,
                this.workContext.Customer.WorkingDayEnd,
                TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId),
                ApplicationTypes.Helpdesk,
                showRemainingTime,
                out remainingTime,
                out aggregateData,
                relatedCasesCaseId,
                relatedCasesUserId,
                caseIds);

            searchResult.cases = this.TreeTranslate(searchResult.cases, SessionFacade.CurrentCustomer.Id);
            search.Search.IdsForLastSearch = this.GetIdsFromSearchResult(searchResult.cases);
            searchResult.BackUrl = Url.Action("RelatedCasesFull", "Cases", new { area = string.Empty, caseId = relatedCasesCaseId, userId = relatedCasesUserId });

            return searchResult;
        }

        private RelatedCasesFullViewModel GetRelatedCasesViewModel(
                                            string sortBy,
                                            string sortByAsc,
                                            int caseId,
                                            string userId)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, caseId, userId);
            var model = this.caseModelFactory.GetRelatedCasesFullModel(cases, userId, caseId, sortBy, sortByAsc.convertStringToBool());

            return model;
        }

        private CaseSearchModel InitCaseSearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter();
            var m = new CaseSearchModel();
            var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
            {
                throw new Exception(string.Format("Customers settings is empty or not valid for customer id {0}", customerId));
            }

            f.CustomerId = customerId;
            f.UserId = userId;
            f.CaseType = cu.CaseCaseTypeFilter.ReturnCustomerUserValue().convertStringToInt();
            f.Category = cu.CaseCategoryFilter.ReturnCustomerUserValue();
            f.Priority = cu.CasePriorityFilter.ReturnCustomerUserValue();
            f.ProductArea = cu.CaseProductAreaFilter.ReturnCustomerUserValue();
            f.Region = cu.CaseRegionFilter.ReturnCustomerUserValue();
            f.Department = cu.CaseDepartmentFilter.ReturnCustomerUserValue();
            f.StateSecondary = cu.CaseStateSecondaryFilter.ReturnCustomerUserValue();
            f.Status = cu.CaseStatusFilter.ReturnCustomerUserValue();
            f.User = cu.CaseUserFilter.ReturnCustomerUserValue();
            f.UserPerformer = cu.CasePerformerFilter.ReturnCustomerUserValue();
            f.UserResponsible = cu.CaseResponsibleFilter.ReturnCustomerUserValue();
            f.WorkingGroup = cu.CaseWorkingGroupFilter.ReturnCustomerUserValue();
            f.CaseProgress = CaseSearchFilter.InProgressCases;
            f.CaseRegistrationDateStartFilter = cu.CaseRegistrationDateStartFilter;
            f.CaseRegistrationDateEndFilter = cu.CaseRegistrationDateEndFilter;
            f.CaseWatchDateStartFilter = cu.CaseWatchDateStartFilter;
            f.CaseWatchDateEndFilter = cu.CaseWatchDateEndFilter;
            f.CaseClosingDateStartFilter = cu.CaseClosingDateStartFilter;
            f.CaseClosingDateEndFilter = cu.CaseClosingDateEndFilter;
            f.CaseClosingReasonFilter = cu.CaseClosingReasonFilter.ReturnCustomerUserValue();
            f.CaseRemainingTime = cu.CaseRemainingTimeFilter.ReturnCustomerUserValue();
            this.ResolveParentPathesForFilter(f);

            s.SortBy = "CaseNumber";
            s.Ascending = true;

            return new CaseSearchModel() { caseSearchFilter = f, Search = s };
        }

        private CaseSearchModel InitEmptySearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter();
            var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
            {
                throw new Exception("It looks that something has happened with your session. Refresh page to fix it.");
            }

            f.CustomerId = customerId;
            f.UserId = userId;
            f.CaseType = 0;
            f.Category = null;
            f.Priority = null;
            f.ProductArea = null;
            f.Region = null;
            f.StateSecondary = null;
            f.Status = null;
            f.User = null;
            f.UserPerformer = null;
            f.UserResponsible = null;
            f.WorkingGroup = null;
            f.CaseProgress = CaseSearchFilter.InProgressCases;
            f.CaseRegistrationDateStartFilter = null;
            f.CaseRegistrationDateEndFilter = null;
            f.CaseWatchDateStartFilter = null;
            f.CaseWatchDateEndFilter = null;
            f.CaseClosingDateStartFilter = null;
            f.CaseClosingDateEndFilter = null;
            f.CaseClosingReasonFilter = null;
            this.ResolveParentPathesForFilter(f);

            s.SortBy = "CaseNumber";
            s.Ascending = true;

            return new CaseSearchModel() { caseSearchFilter = f, Search = s };
        }

        private CaseSearchModel InitAdvancedSearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter
            {
                CustomerId = customerId,
                UserId = userId,
                UserPerformer = string.Empty,
                CaseProgress = string.Empty,
                WorkingGroup = string.Empty,
                CaseRegistrationDateStartFilter = null,
                CaseRegistrationDateEndFilter = null,
                CaseClosingDateStartFilter = null,
                CaseClosingDateEndFilter = null,
                Customer = customerId.ToString()
            };

            s.SortBy = "CaseNumber";
            s.Ascending = false;

            return new CaseSearchModel() { caseSearchFilter = f, Search = s };
        }

        private List<MyFavoriteFilterJSModel> GetMyFavorites(int customerId, int userId)
        {
            var ret = new List<MyFavoriteFilterJSModel>();
            var favorites = this._caseService.GetMyFavorites(customerId, userId);
            if (favorites.Any())
            {
                foreach (var favorite in favorites)
                    ret.Add(new MyFavoriteFilterJSModel(favorite.Id, favorite.Name, favorite.Fields));
            }
            return ret.OrderBy(f => f.Name).ToList();
        }        

        private List<SelectListItem> GetRemainigTimeList(int customerId)
        {
            var curCustomer = this._customerService.GetCustomer(customerId);
            var dayWorkingTime = curCustomer.WorkingDayEnd - curCustomer.WorkingDayStart;
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Akuta ärenden", Enums.TranslationSource.TextTranslation),
                Value = RemainingTimes.Delayed.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<1 h",
                Value = RemainingTimes.OneHour.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<2 h",
                Value = RemainingTimes.TwoHours.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<4 h",
                Value = RemainingTimes.FourHours.ToStr(),
                Selected = false
            });

            if (dayWorkingTime > 8)
            {
                li.Add(new SelectListItem()
                {
                    Text = Translation.Get("Återstående åtgärdstid") + ", " + "<8 h",
                    Value = RemainingTimes.EightHours.ToStr(),
                    Selected = false
                });
            }

            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<1 d",
                Value = RemainingTimes.OneDay.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<2 d",
                Value = RemainingTimes.TwoDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<3 d",
                Value = RemainingTimes.ThreeDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<4 d",
                Value = RemainingTimes.FourDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<5 d",
                Value = RemainingTimes.FiveDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + ">=5 d",
                Value = RemainingTimes.MaxDays.ToStr(),
                Selected = false
            });

            return li;
        }

        private List<CaseFileModel> MakeCaseFileModel(List<string> files, string savedFiles)
        {
            var res = new List<CaseFileModel>();
            int i = 0;

            var savedFileList = string.IsNullOrEmpty(savedFiles)? null : savedFiles.Split('|').ToList();

            foreach (var f in files)
            {
                i++;
                var canDelete = !(savedFileList != null && savedFileList.Contains(f));
                var cf = new CaseFileModel(i, i, f, DateTime.Now, SessionFacade.CurrentUser.FirstName + " " + SessionFacade.CurrentUser.SurName, canDelete);
                res.Add(cf);
            }

            return res;
        }

        private List<ItemOverview> GetMaxRowsFilter()
        {
            var ret = new List<ItemOverview>();
            ret.Add(new ItemOverview("10", "10"));
            ret.Add(new ItemOverview("20", "20"));
            ret.Add(new ItemOverview("30", "30"));
            ret.Add(new ItemOverview("40", "40"));
            ret.Add(new ItemOverview("50", "50"));
            ret.Add(new ItemOverview("100", "100"));
            ret.Add(new ItemOverview("200", "200"));
            return ret;
        }

        private CaseLockModel GetCaseLockModel(int caseId, int userId)
        {
            var caseLock = this._caseLockService.GetCaseLockByCaseId(caseId);
            var caseIsLocked = true;
            var gs = this._globalSettingService.GetGlobalSettings().FirstOrDefault();
            var extendedSec = (gs != null && gs.CaseLockExtendTime > 0 ? gs.CaseLockExtendTime : this._defaultExtendCaseLockTime);
            var timerInterval = (gs != null ? gs.CaseLockTimer : 0);
            var bufferTime = (gs != null && gs.CaseLockBufferTime > 0 ? gs.CaseLockBufferTime : this._defaultCaseLockBufferTime);
            var caseLockGUID = string.Empty;
            var nowTime = DateTime.Now;
            if (caseLock == null)
            {
                // Case is not locked 
                caseIsLocked = false;
            }
            else
            {
                if ((caseLock.ExtendedTime.AddSeconds(bufferTime) < nowTime) ||
                    (caseLock.ExtendedTime.AddSeconds(bufferTime) >= nowTime &&
                     caseLock.UserId == userId &&
                     caseLock.BrowserSession == Session.SessionID))
                {
                    // Unlock case because user has leaved the Case in anormal way (Close browser/reset computer)
                    // Unlock case because current user was opened this case last time and recently
                    this._caseLockService.UnlockCaseByCaseId(caseId);
                    caseIsLocked = false;
                }
            }

            if (!caseIsLocked)
            {
                // Lock Case if it's not locked
                var now = DateTime.Now;
                var extendedLockTime = now.AddSeconds(extendedSec);
                var newLockGUID = Guid.NewGuid();
                caseLockGUID = newLockGUID.ToString();
                var user = this._userService.GetUser(userId);
                var newCaseLock = new CaseLock(caseId, userId, newLockGUID, Session.SessionID, now, extendedLockTime, user);
                this._caseLockService.LockCase(newCaseLock);
                caseLock = newCaseLock;
            }

            return caseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval);
        }

        private CaseStatisticsViewModel GetCaseStatisticsModel(CaseAggregateData aggregateData, int caseCount,
                                                               string expandedGroup, List<string> fields)
        {
            if (aggregateData == null)
                return null;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var ret = new CaseStatisticsViewModel();
            ret.ExpandedGroup = expandedGroup;
            ret.CaseData.AttributeId = "Case";
            ret.CaseData.AttributeName = Translation.Get("Ärenden");
            ret.CaseData.AttributeValue = caseCount.ToString();
            int sumValues;

            #region Status

            if (fields.Contains(GlobalEnums.TranslationCaseFields.Status_Id.ToString()))
            {
                sumValues = 0;
                var statusChildren = new List<DataAttributeGroup>();
                var allStatus = this._statusService.GetStatuses(customerId);
                foreach (var status in aggregateData.Status)
                {
                    var name = allStatus.Where(s => s.Id == status.Key).Select(s => s.Name).FirstOrDefault() ?? string.Empty;
                    statusChildren.Add(new DataAttributeGroup(status.Key.ToString(), name, status.Value.ToString()));
                    sumValues += status.Value;
                }

                var statusGroup = new DataAttributeGroup(
                                                    "Status",
                                                    Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                                                                    Enums.TranslationSource.CaseTranslation, customerId),
                                                    sumValues.ToString(),
                                                    statusChildren.OrderBy(s => s.AttributeName).ToList());

                ret.CaseData.AddChild(statusGroup);

            }
            #endregion

            #region SubStatus
            if (fields.Contains(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
            {
                sumValues = 0;
                var subStatusChildren = new List<DataAttributeGroup>();
                var allSubStatus = this._stateSecondaryService.GetStateSecondaries(customerId);
                foreach (var subStatus in aggregateData.SubStatus)
                {
                    var name = allSubStatus.Where(s => s.Id == subStatus.Key).Select(s => s.Name).FirstOrDefault() ?? string.Empty;
                    subStatusChildren.Add(new DataAttributeGroup(subStatus.Key.ToString(), Translation.Get(name), subStatus.Value.ToString()));
                    sumValues += subStatus.Value;
                }

                var subStatusGroup = new DataAttributeGroup(
                                                    "SubStatus",
                                                    Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                                                                    Enums.TranslationSource.CaseTranslation, customerId),
                                                    sumValues.ToString(),
                                                    subStatusChildren.OrderBy(ss => ss.AttributeName).ToList());

                ret.CaseData.AddChild(subStatusGroup);
            }
            #endregion

            return ret;
        }

        private SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<DHDomain.User> users, Setting customerSetting)
        {            
            var emailGroups = _emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = _workingGroupService.GetWorkingGroupsWithActiveEmails(customerId);
            var administrators = new List<ItemOverview>();

            if (users != null)
            {
                var validUsers = users.Where(u => u.IsActive != 0 &&
                                                 u.Performer == 1 &&
                                                 _emailService.IsValidEmail(u.Email) &&
                                                 !String.IsNullOrWhiteSpace(u.Email)).ToList();

                if (customerSetting.IsUserFirstLastNameRepresentation == 1)
                {
                    foreach (var u in users.OrderBy(it=> it.FirstName).ThenBy(it=> it.SurName))                    
                        if (u.IsActive == 1 && u.Performer == 1 && _emailService.IsValidEmail(u.Email) && !String.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.FirstName, u.SurName), u.Email));
                }
                else
                {
                    foreach (var u in users.OrderBy(it => it.SurName).ThenBy(it => it.FirstName))
                        if (u.IsActive == 1 && u.Performer == 1 && _emailService.IsValidEmail(u.Email) && !String.IsNullOrWhiteSpace(u.Email))
                            administrators.Add(new ItemOverview(string.Format("{0} {1}", u.SurName, u.FirstName), u.Email));
                }               
            }

            var emailGroupList = new MultiSelectList(emailGroups, "Id", "Name");
            var emailGroupEmails = emailGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var workingGroupList = new MultiSelectList(workingGroups, "Id", "Name");
            var workingGroupEmails = workingGroups.Select(g => new GroupEmailsModel(g.Id, g.Emails)).ToList();
            var administratorList = new MultiSelectList(administrators, "Value", "Name");

            return new SendToDialogModel(
                emailGroupList,
                emailGroupEmails,
                workingGroupList,
                workingGroupEmails,
                administratorList);
        }        

        private CaseSettingModel GetCaseSettingModel(int customerId, int userId)
        {
            var ret = new CaseSettingModel();
            const bool IsTakeOnlyActive = true;
            ret.CustomerId = customerId;
            ret.UserId = userId;

            var userCaseSettings = _customerUserService.GetUserCaseSettings(customerId, userId);

            var regions = this._regionService.GetRegions(customerId);
            ret.RegionCheck = userCaseSettings.Region != string.Empty;
            ret.Regions = regions;
            ret.SelectedRegion = userCaseSettings.Region;

            var customerSettings = this._settingService.GetCustomerSetting(customerId);

            var departments = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId, IsTakeOnlyActive);
            if (!departments.Any())
            {
                departments =
                    this._departmentService.GetDepartments(customerId)
                        .Where(
                            d =>
                            d.Region_Id == null
                            || IsTakeOnlyActive == false
                            || (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0))
                        .ToList();
            }

            //var departments = this._departmentService.GetDepartments(customerId, ActivationStatus.All);
            ret.IsDepartmentChecked = userCaseSettings.Departments != string.Empty;

            if (customerSettings != null && customerSettings.ShowOUsOnDepartmentFilter != 0)
                ret.Departments = AddOrganizationUnitsToDepartments(departments);
            else
                ret.Departments = departments;

            ret.SelectedDepartments = userCaseSettings.Departments;

            
            
            ret.RegisteredByCheck = userCaseSettings.RegisteredBy != string.Empty;
            ret.RegisteredByUserList = this._userService.GetUserOnCases(customerId, IsTakeOnlyActive).MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.RegisteredBy))
            {
                ret.lstRegisterBy = userCaseSettings.RegisteredBy.Split(',').Select(int.Parse).ToArray();
            }

            ret.CaseTypeCheck = userCaseSettings.CaseType != string.Empty;
            ret.CaseTypes = this._caseTypeService.GetCaseTypes(customerId, IsTakeOnlyActive);
            ret.CaseTypePath = "--";
            int caseType;
            int.TryParse(userCaseSettings.CaseType, out caseType);
            ret.CaseTypeId = caseType;
            if (caseType > 0)
            {
                var ct = this._caseTypeService.GetCaseType(ret.CaseTypeId);
                if (ct != null)
                {
                    ret.CaseTypePath = ct.getCaseTypeParentPath();
                }
            }

            ret.ProductAreas = this._productAreaService.GetTopProductAreasForUser(
                    customerId,
                    SessionFacade.CurrentUser,
                    true);
            ret.ProductAreaPath = "--";

            int pa;
            int.TryParse(userCaseSettings.ProductArea, out pa);
            ret.ProductAreaId = pa;

            if (pa > 0)
            {
                var p = this._productAreaService.GetProductArea(ret.ProductAreaId);
                if (p != null)
                    ret.ProductAreaPath = string.Join(" - ", this._productAreaService.GetParentPath(p.Id, customerId));
            }
            ret.ProductAreaCheck = (userCaseSettings.ProductArea != string.Empty);

            var userWorkingGroup =
                _userService.GetUserWorkingGroups().Where(u => u.User_Id == userId).Select(x => x.WorkingGroup_Id);

            //var isAdmin = SessionFacade.CurrentUser.IsAdministrator();
            //if (isAdmin)
            if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
            {
                var workingGroups = _workingGroupService.GetWorkingGroups(customerId, userId, true).ToList();
                ret.WorkingGroups = workingGroups;
            }
            else
            {
                var workingGroups = _workingGroupService.GetWorkingGroups(customerId, true).ToList();
                ret.WorkingGroups = workingGroups;
            }


            //.Where(w => userWorkingGroup.Contains(w.Id))
            ret.WorkingGroupCheck = (userCaseSettings.WorkingGroup != string.Empty);
            //ret.WorkingGroups = workingGroups;
            ret.SelectedWorkingGroup = userCaseSettings.WorkingGroup;

            ret.ResponsibleCheck = userCaseSettings.Responsible;

            ret.AdministratorCheck = true;

            var performers = this._userService.GetAvailablePerformersOrUserId(customerId);
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            ret.AvailablePerformersList = performers.MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.Administrators))
            {
                ret.lstAdministrator = userCaseSettings.Administrators.Split(',').Select(int.Parse).ToArray();
            }

            var priorities = _priorityService.GetPriorities(customerId).OrderBy(p => p.Code).ToList();
            ret.PriorityCheck = (userCaseSettings.Priority != string.Empty);
            ret.Priorities = priorities;
            ret.SelectedPriority = userCaseSettings.Priority;

            ret.StateCheck = userCaseSettings.State;

            var subStates = _stateSecondaryService.GetStateSecondaries(customerId).OrderBy(s => s.Name).ToList();
            ret.SubStateCheck = (userCaseSettings.SubState != string.Empty);
            ret.SubStates = subStates;
            ret.SelectedSubState = userCaseSettings.SubState;

            var caseRemainingTime = this.GetRemainigTimeList(customerId);
            ret.CaseRemainingTimeChecked = userCaseSettings.CaseRemainingTime != string.Empty;
            ret.filterCaseRemainingTime = caseRemainingTime;
            ret.SelectedCaseRemainingTime = userCaseSettings.CaseRemainingTime;

            ret.CaseRegistrationDateStartFilter = userCaseSettings.CaseRegistrationDateStartFilter;
            ret.CaseRegistrationDateEndFilter = userCaseSettings.CaseRegistrationDateEndFilter;
            ret.CaseWatchDateStartFilter = userCaseSettings.CaseWatchDateStartFilter;
            ret.CaseWatchDateEndFilter = userCaseSettings.CaseWatchDateEndFilter;
            ret.CaseClosingDateStartFilter = userCaseSettings.CaseClosingDateStartFilter;
            ret.CaseClosingDateEndFilter = userCaseSettings.CaseClosingDateEndFilter;
            ret.CaseRegistrationDateFilterShow = userCaseSettings.CaseRegistrationDateFilterShow;
            ret.CaseWatchDateFilterShow = userCaseSettings.CaseWatchDateFilterShow;
            ret.CaseClosingDateFilterShow = userCaseSettings.CaseClosingDateFilterShow;

            /// "Inititator" field
            ret.CaseInitiatorFilterShow = userCaseSettings.CaseInitiatorFilterShow;

            ret.ClosingReasons = this._finishingCauseService.GetFinishingCauses(customerId);
            ret.ClosingReasonPath = "--";
            int closingReason;
            int.TryParse(userCaseSettings.CaseClosingReasonFilter, out closingReason);
            ret.ClosingReasonId = closingReason;
            if (closingReason > 0)
            {

                var fc = this._finishingCauseService.GetFinishingCause(ret.ClosingReasonId);
                if (fc != null)
                {
                    ret.ClosingReasonPath = fc.GetFinishingCauseParentPath();
                }
            }

            ret.ClosingReasonCheck = userCaseSettings.CaseClosingReasonFilter != string.Empty;

            ret.ClosingReasonCheck = userCaseSettings.CaseClosingReasonFilter != string.Empty;
            ret.ClosingReasons = this._finishingCauseService.GetFinishingCauses(customerId);
            ret.ColumnSettingModel = this.caseOverviewSettingsService.GetSettings(customerId, SessionFacade.CurrentUser.UserGroupId, userId);

            return ret;
        }

        private ReportModel GetCaseReportViewerData(string reportName, int caseId, int caseNumber)
        {
            var reportSelectedFilter = new ReportSelectedFilter();
            reportSelectedFilter.SelectedCustomers.Add(SessionFacade.CurrentCustomer.Id);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);

            var ad = userTimeZone.GetAdjustmentRules();            

            //TimeZone.CurrentTimeZone.
            
            var userTimeOffset = Convert.ToInt32((DateTime.UtcNow - localNow).TotalMinutes);
            userTimeOffset = userTimeOffset == 0 ? 0 : -userTimeOffset;
 
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@CaseId", caseId));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@LanguageId", SessionFacade.CurrentLanguageId));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@UserId", SessionFacade.CurrentUser.Id));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@UserTimeOffset", userTimeOffset));

            var reportData = _ReportServiceService.GetReportData(reportName, reportSelectedFilter, SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);

            ReportModel model = new ReportModel();

            if (reportData == null || (reportData != null && !reportData.DataSets.Any()))
            {
                model = null;
            }
            else
            {
                var reportDataModel = new List<ReportDataModel>();
                if (reportData.DataSets[0].DataSet.Rows.Count > 0)
                {
                    foreach (DataRow r in reportData.DataSets[0].DataSet.Rows)
                    {
                        var row = new ReportDataModel(int.Parse(r.ItemArray[0].ToString()), r.ItemArray[1].ToString(), r.ItemArray[2].ToString(),
                                                      r.ItemArray[3].ToString(), int.Parse(r.ItemArray[4].ToString()), r.ItemArray[5].ToString());                         
                        reportDataModel.Add(row);
                    }
                }

                ReportViewer reportViewer = new ReportViewer();

                //Use Html view by data 
                /*var basePath = Request.MapPath(Request.ApplicationPath);
                var fileLocation = Path.Combine(_reportFolderName, string.Format("{0}.rdl", reportData.ReportName));
                var reportFile = Path.Combine(basePath, fileLocation);
                
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowZoomControl = false;                
                reportViewer.LocalReport.ReportPath = reportFile;
                reportViewer.LocalReport.DisplayName = caseNumber.ToString();

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("CaseId", caseId.ToString()));
                parameters.Add(new ReportParameter("LanguageId", SessionFacade.CurrentLanguageId.ToString()));
                parameters.Add(new ReportParameter("UserId", SessionFacade.CurrentUser.Id.ToString()));
                reportViewer.LocalReport.SetParameters(parameters);
                
                reportViewer.LocalReport.Refresh();
                foreach (var dataSet in reportData.DataSets)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSet.DataSetName, dataSet.DataSet));

                model.Report = reportViewer;*/
                model.ReportData = reportDataModel;
            }

            return model;
        }

        private List<SelectListItem> GetCausingPartsModel(int customerId, int? curCausingPartId)
        {
            var allActiveCausinParts = this._causingPartService.GetActiveParentCausingParts(customerId, curCausingPartId);
            var ret = new List<SelectListItem>();

            var parentRet = new List<SelectListItem>();
            var childrenRet = new List<SelectListItem>();

            foreach (var causingPart in allActiveCausinParts)
            {
                var curName = string.Empty; 
                if (causingPart.Parent != null && curCausingPartId.HasValue && causingPart.Id == curCausingPartId.Value)
                {
                    curName = string.Format("{0} - {1}", Translation.Get(causingPart.Parent.Name, Enums.TranslationSource.TextTranslation, customerId), 
                                                         Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId));
                    
                    childrenRet.Add(new SelectListItem(){ Value = causingPart.Id.ToString(), Text = curName, Selected = true });
                }
                else
                {
                    if (causingPart.Children.Any())
                    {
                        foreach (var child in causingPart.Children)
                        {
                            if (child.IsActive)
                            {
                                curName = string.Format("{0} - {1}", Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId),
                                                                     Translation.Get(child.Name, Enums.TranslationSource.TextTranslation, customerId));

                                var isSelected = (child.Id == curCausingPartId);
                                childrenRet.Add(new SelectListItem() { Value = child.Id.ToString(), Text = curName, Selected = isSelected });
                            }
                        }
                    }
                    else
                    {
                        curName = Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId);
                        var isSelected = (causingPart.Id == curCausingPartId);
                        parentRet.Add(new SelectListItem() { Value = causingPart.Id.ToString(), Text = curName, Selected = isSelected });
                    }
                }                
            }
            
            ret = parentRet.OrderBy(p => p.Text).Union(childrenRet.OrderBy(c => c.Text)).ToList();

            return ret.GroupBy(r => r.Value).Select(g => g.First()).ToList();

        }

        private List<SelectListItem> GetProductAreasModel(int customerId, int? curProductAreaId)
        {
            var allActiveProductAreas = this._productAreaService.GetTopProductAreasForUser(customerId, SessionFacade.CurrentUser, false);
            var ret = new List<SelectListItem>();

            var parentRet = new List<SelectListItem>();
            var childrenRet = new List<SelectListItem>();

            var curName = string.Empty;

            foreach (var productArea in allActiveProductAreas)
            {                
                curName = productArea.ResolveFullName();
                parentRet.Add(new SelectListItem() { Value = productArea.Id.ToString(), Text = curName, Selected = (productArea.Id == curProductAreaId) });
                parentRet.AddRange(GetProductAreaChild(productArea, curProductAreaId));
            }

            ret = parentRet.OrderBy(p => p.Text).ToList();
            return ret;

        }
        private List<SelectListItem> GetProductAreaChild(ProductArea productArea, int? curProductAreaId)
        {
            var ret = new List<SelectListItem>();
            if (productArea.SubProductAreas.Any())
            {
                var curName = string.Empty;
                foreach (var child in productArea.SubProductAreas)
                {
                    curName = child.ResolveFullName();
                    ret.Add(new SelectListItem() { Value = child.Id.ToString(), Text = curName, Selected = (child.Id == curProductAreaId) });
                    ret.AddRange(GetProductAreaChild(child, curProductAreaId));
                }
            }
            
            return ret;
                
        }

        #endregion

        #region --General--

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private RouteValueDictionary ExtractRouteInfo(string fullUrl)
        {
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            var qsObject = new NameValueCollection();
            if (questionMarkIndex != -1)
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
                qsObject = HttpUtility.ParseQueryString(queryString);
            }

            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var res = new RouteValueDictionary
            {
                {"action", "index"},
                {"controller", "cases"},
            };
            if (routeData != null && routeData.Values != null)
            {
                if (routeData.Values.ContainsKey("action") && routeData.Values.ContainsKey("controller"))
                {
                    res["action"] = routeData.Values["action"];
                    res["controller"] = routeData.Values["controller"];
                }

                if (routeData.Values.ContainsKey("area"))
                {
                    res["area"] = routeData.Values["area"];
                }

                if (routeData.Values.ContainsKey("id") && !String.IsNullOrEmpty(routeData.Values["id"].ToString()))
                {
                    res["id"] = routeData.Values["id"];
                }
            }

            foreach (var key in qsObject.AllKeys)
            {
                res.Add(key, qsObject[key]);
            }

            return res;
        }

        private RouteValueDictionary ExtractCurrentRouteInfo()
        {
            var fullUrl = Request.Url.AbsoluteUri;
            return this.ExtractRouteInfo(fullUrl);
        }

        private RouteValueDictionary ExtractPreviousRouteInfo()
        {
            var fullUrl = Request.UrlReferrer.ToString();
            return this.ExtractRouteInfo(fullUrl);
        }

        private void AddViewDataValues()
        {
            ViewData["Callback"] = "SendToDialogCaseCallback";
            ViewData["Id"] = "divSendToDialogCase";
        }

        private string GetLinkWithHash(string hash, object routeObject, string action = null, string controller = null)
        {
            var prevInfo = this.ExtractCurrentRouteInfo();
            if (string.IsNullOrEmpty(action))
            {
                action = prevInfo["action"].ToString();
            }

            if (string.IsNullOrEmpty(controller))
            {
                controller = prevInfo["controller"].ToString();
            }

            var url = UrlHelper.GenerateUrl(
                null,
                action,
                controller,
                null,
                null,
                hash,
                new RouteValueDictionary(routeObject),
                Url.RouteCollection,
                Url.RequestContext,
                false);
            return url;
        }

        #endregion       
        
        #endregion
    }
}
