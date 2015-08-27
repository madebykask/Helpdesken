using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
    using DH.Helpdesk.Web.Infrastructure.Grid;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Case.Input;
    using DH.Helpdesk.Web.Models.Case.Output;
    using DH.Helpdesk.Web.Models.CaseLock;
    using DH.Helpdesk.Web.Models.Shared;

    using DHDomain = DH.Helpdesk.Domain;

    public class CasesController : BaseController
    {
        #region Private variables

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

        private const string ParentPathDefaultValue = "--";

        private readonly ICaseNotifierModelFactory caseNotifierModelFactory;

        private readonly INotifierService notifierService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IInvoiceArticlesModelFactory invoiceArticlesModelFactory;

        private readonly IConfiguration configuration;

        private readonly ICaseSolutionSettingService caseSolutionSettingService;

        private readonly IInvoiceHelper invoiceHelper;

        private readonly ICaseModelFactory caseModelFactory;

        private readonly CaseOverviewGridSettingsService caseOverviewSettingsService;

        private readonly GridSettingsService gridSettingsService;

        private readonly OutputFormatter outputFormatter;

        private readonly IOrganizationService _organizationService;

        private readonly IMasterDataService _masterDataService;

        private readonly OrganizationJsonService _orgJsonService;

        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;

        private readonly ICaseLockService _caseLockService;

        private readonly int _defaultMaxRows;

        private readonly int _defaultCaseLockBufferTime;

        private readonly int _defaultExtendCaseLockTime;

        private readonly IWatchDateCalendarService watchDateCalendarServcie;

        #endregion

        #region Constructor

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
            IInvoiceArticlesModelFactory invoiceArticlesModelFactory,
            IConfiguration configuration,
            ICaseSolutionSettingService caseSolutionSettingService,
            IInvoiceHelper invoiceHelper,
            ICaseModelFactory caseModelFactory,
            CaseOverviewGridSettingsService caseOverviewSettingsService,
            GridSettingsService gridSettingsService,
            OutputFormatter outputFormatter,
            IOrganizationService organizationService,
            OrganizationJsonService orgJsonService,
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            ICaseLockService caseLockService, 
            IWatchDateCalendarService watchDateCalendarServcie)
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
            this.invoiceArticlesModelFactory = invoiceArticlesModelFactory;
            this.configuration = configuration;
            this.caseSolutionSettingService = caseSolutionSettingService;
            this.invoiceHelper = invoiceHelper;
            this.caseModelFactory = caseModelFactory;
            this.caseOverviewSettingsService = caseOverviewSettingsService;
            this.gridSettingsService = gridSettingsService;
            this.outputFormatter = outputFormatter;
            this._organizationService = organizationService;
            this._orgJsonService = orgJsonService;
            this._registrationSourceCustomerService = registrationSourceCustomerService;
            this._caseLockService = caseLockService;
            this.watchDateCalendarServcie = watchDateCalendarServcie;
            this._defaultMaxRows = 10;
            this._defaultCaseLockBufferTime = 30; // Second
            this._defaultExtendCaseLockTime = 60; // Second
        }

        #endregion

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
            CaseSearchModel csm = null;

            if (!resetFilter)
                selectedCustomerId = 0;

            var model = CreateAdvancedSearchSpecificFilterData(SessionFacade.CurrentUser.Id, selectedCustomerId);
            return PartialView("AdvancedSearch/_SpecificSearchTab", model);
        }

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
            f.WorkingGroup = frm.ReturnFormValue("lstFilterWorkingGroup");
            f.UserPerformer = frm.ReturnFormValue("CaseSearchFilterData.lstFilterPerformer");
            f.StateSecondary = frm.ReturnFormValue("lstFilterStateSecondary");
            f.Initiator = frm.ReturnFormValue("CaseInitiatorFilter");
            f.CaseRegistrationDateStartFilter = frm.GetDate("CaseRegistrationDateStartFilter");
            f.CaseRegistrationDateEndFilter = frm.GetDate("CaseRegistrationDateEndFilter");
            f.CaseClosingDateStartFilter = frm.GetDate("CaseClosingDateStartFilter");
            f.CaseClosingDateEndFilter = frm.GetDate("CaseClosingDateEndFilter");

            //Apply & save specific filters only when user has selected one customer 
            if (!string.IsNullOrEmpty(f.Customer) && !f.Customer.Contains(","))
            {
                f.Department = frm.ReturnFormValue("lstfilterDepartment");
                f.Priority = frm.ReturnFormValue("lstfilterPriority");
                f.StateSecondary = frm.ReturnFormValue("lstfilterStateSecondary");
                f.CaseType = frm.ReturnFormValue("hid_CaseTypeDropDown").convertStringToInt();
                f.ProductArea = f.ProductArea = frm.ReturnFormValue("hid_ProductAreaDropDown").ReturnCustomerUserValue();
                f.CaseClosingReasonFilter = frm.ReturnFormValue("hid_ClosingReasonDropDown").ReturnCustomerUserValue();
            }
            else
            {
                f.Department = string.Empty;
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
                f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");

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
                                                                            name = c.field
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

            SessionFacade.CurrentAdvancedSearch = sm;
            #endregion

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
                        jsRow.Add(col.name, this.outputFormatter.FormatField(searchCol, userTimeZone));
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
            if (SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                    this.gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }

            if (SessionFacade.CurrentCaseSearch == null)
            {
                SessionFacade.CurrentCaseSearch = this.InitCaseSearchModel(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentUser.Id);
            }

            var m = new JsonCaseIndexViewModel();
            var customerUser = this._customerUserService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            m.CaseSearchFilterData = this.CreateCaseSearchFilterData(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser, customerUser, SessionFacade.CurrentCaseSearch);
            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            m.CaseSetting = this.GetCaseSettingModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            m.PageSettings = new PageSettingsModel()
                                 {
                                     searchFilter = JsonCaseSearchFilterData.MapFrom(m.CaseSetting),
                                     gridSettings =
                                         JsonGridSettingsMapper.ToJsonGridSettingsModel(
                                             SessionFacade.CaseOverviewGridSettings,
                                             SessionFacade.CurrentCustomer.Id,
                                             m.CaseSetting.ColumnSettingModel.AvailableColumns.Count()),
                                     refreshContent = user.RefreshContent
                                 };

            return this.View("Index", m);
        }

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
            f.Initiator = frm.ReturnFormValue(CaseSearchFilter.InitiatorNameAttribute);
            f.CaseType = frm.ReturnFormValue(CaseSearchFilter.CaseTypeIdNameAttribute).convertStringToInt();
            f.ProductArea = frm.ReturnFormValue(CaseSearchFilter.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            f.Region = frm.ReturnFormValue(CaseSearchFilter.RegionNameAttribute);
            f.Department = frm.ReturnFormValue(CaseSearchFilter.DepartmentNameAttribute);
            f.User = frm.ReturnFormValue(CaseSearchFilter.RegisteredByNameAttribute);
            f.Category = frm.ReturnFormValue(CaseSearchFilter.CategoryNameAttribute);
            f.WorkingGroup = frm.ReturnFormValue(CaseSearchFilter.WorkingGroupNameAttribute);
            f.UserResponsible = frm.ReturnFormValue(CaseSearchFilter.ResponsibleNameAttribute);
            f.UserPerformer = frm.ReturnFormValue(CaseSearchFilter.PerformerNameAttribute);

            f.Priority = frm.ReturnFormValue(CaseSearchFilter.PriorityNameAttribute);
            f.Status = frm.ReturnFormValue(CaseSearchFilter.StatusNameAttribute);
            f.StateSecondary = frm.ReturnFormValue(CaseSearchFilter.StateSecondaryNameAttribute);

            f.CaseRegistrationDateStartFilter = frm.GetDate(CaseSearchFilter.CaseRegistrationDateStartFilterNameAttribute);
            f.CaseRegistrationDateEndFilter = frm.GetDate(CaseSearchFilter.CaseRegistrationDateEndFilterFilterNameAttribute);

            f.CaseWatchDateStartFilter = frm.GetDate(CaseSearchFilter.CaseWatchDateStartFilterNameAttribute);
            f.CaseWatchDateEndFilter = frm.GetDate(CaseSearchFilter.CaseWatchDateEndFilterNameAttribute);
            f.CaseClosingDateStartFilter = frm.GetDate(CaseSearchFilter.CaseClosingDateStartFilterNameAttribute);
            f.CaseClosingDateEndFilter = frm.GetDate(CaseSearchFilter.CaseClosingDateEndFilterNameAttribute);
            f.CaseClosingReasonFilter = frm.ReturnFormValue(CaseSearchFilter.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            f.SearchInMyCasesOnly = frm.IsFormValueTrue("SearchInMyCasesOnly");

            f.CaseProgress = frm.ReturnFormValue(CaseSearchFilter.FilterCaseProgressNameAttribute);
            f.FreeTextSearch = frm.ReturnFormValue(CaseSearchFilter.FreeTextSearchNameAttribute);

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
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
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
                out remainingTimeData);
            m.cases = this.TreeTranslate(m.cases, f.CustomerId);
            sm.Search.IdsForLastSearch = this.GetIdsFromSearchResult(m.cases);
            SessionFacade.CurrentCaseSearch = sm;
            #endregion

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
                        jsRow.Add(col.name, this.outputFormatter.FormatField(searchCol, userTimeZone));
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

            return this.Json(new { result = "success", data = data, remainingView = remainingView });
        }

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
            }

            //ärendetyp
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter))
                fd.filterCaseType = this._caseTypeService.GetCaseTypes(cusId);
            //working group
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseWorkingGroupFilter))
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
                const bool isTakeOnlyActive = true;
                if (gs.LockCaseToWorkingGroup == 0)
                    fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId);
                else
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);
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
                fd.filterCategory = this._categoryService.GetCategories(cusId);
            //prio
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePriorityFilter))
                fd.filterPriority = this._priorityService.GetPriorities(cusId);
            //status
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter))
                fd.filterStatus = this._statusService.GetStatuses(cusId);
            //understatus
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter))
                fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);
            fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser.FollowUpPermission, fd.filterPriority, cusId);

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
                fd.RegisteredByUserList = this._userService.GetUserOnCases(cusId).MapToSelectList(fd.customerSetting);
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

            //ansvarig
            var performers = this._userService.GetAvailablePerformersOrUserId(cusId);
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            fd.AvailablePerformersList = performers.MapToSelectList(fd.customerSetting);
            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

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
            fd.AvailablePerformersList = this._userService.GetUsers(cusId).MapToSelectList(fd.customerSetting);
            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            fd.filterCaseProgress = ObjectExtensions.GetFilterForAdvancedSearch();

            //Working group            
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            if (gs.LockCaseToWorkingGroup == 0)
                fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId);
            else
                fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, true);

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
                customerId = csm.caseSearchFilter.CustomerId;
            }            

            var specificFilter = new AdvancedSearchSpecificFilterData();

            specificFilter.CustomerId = customerId;
            specificFilter.CustomerSetting = this._settingService.GetCustomerSetting(customerId);

            specificFilter.FilteredCaseTypeText = ParentPathDefaultValue;
            specificFilter.FilteredProductAreaText = ParentPathDefaultValue;
            specificFilter.FilteredClosingReasonText = ParentPathDefaultValue;

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
                        this._departmentService.GetDepartments(customerId)
                            .Where(
                                d =>
                                d.Region_Id == null
                                || IsTakeOnlyActive == false
                                || (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0))
                            .ToList();
                }
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
                const bool isTakeOnlyActive = true;
                specificFilter.ProductAreaList = this._productAreaService.GetTopProductAreasForUser(
                    customerId,
                    SessionFacade.CurrentUser,
                    isTakeOnlyActive);
            }
                        
            if (csm != null && csm.caseSearchFilter != null)
            {
                specificFilter.FilteredDepartment = csm.caseSearchFilter.Department;
                specificFilter.FilteredPriority = csm.caseSearchFilter.Priority;
                specificFilter.FilteredStateSecondary = csm.caseSearchFilter.StateSecondary;
                specificFilter.FilteredCaseType = csm.caseSearchFilter.CaseType;                
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

        public JsonResult UnLockCase(string lockGUID)
        {
            if (!string.IsNullOrEmpty(lockGUID))
                this._caseLockService.UnlockCaseByGUID(new Guid(lockGUID));
            return Json("Success");
        }

        public JsonResult IsCaseAvailable(int caseId, string lockGuid)
        {
            var caseLock = this._caseLockService.GetCaseLockByCaseId(caseId);
            if (caseLock != null && caseLock.LockGUID == new Guid(lockGuid) && caseLock.ExtendedTime >= DateTime.Now)
                return Json(true);
            else
                return Json(false);
        }

        public JsonResult ReExtendCaseLock(string lockGuid, int extendValue)
        {
            return Json(this._caseLockService.ReExtendLockCase(new Guid(lockGuid), extendValue));
        }

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
                if (SessionFacade.CurrentUser.CreateCasePermission == 1)
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

                    return this.View(m);
                }
            }

            return this.RedirectToAction("index", "cases", new { id = customerId });
        }


        #region Case save actions
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
        public RedirectToRouteResult NewAndClose(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            return this.RedirectToAction("index", "cases", new { customerId = m.case_.Customer_Id });
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
        #endregion

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

                var caseInvoices = this.invoiceArticleService.GetCaseInvoices(id);
                m.InvoiceArticles = this.invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                //foreach (var invoice in m.InvoiceArticles.Invoices)
                //{
                //    foreach (var order in invoice.Orders)
                //    {
                //        var UserId = 0;
                //        if (order.InvoicedByUserId.HasValue && order.InvoicedByUserId != 0)
                //        {
                //            UserId = order.InvoicedByUserId.Value;
                //            var user = _userService.GetUser(UserId);
                //            order.InvoicedByUser = user.FirstName + " " + user.SurName;
                //        }
                //        else
                //        {
                //            order.InvoicedByUser = "";
                //        }
                        
                //    }
                //}
                m.CustomerSettings = this.workContext.Customer.Settings;
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
                    m.LogFilesModel = new FilesModel(id.ToString(), this._logFileService.FindFileNamesByLogId(id));
                    const bool isAddEmpty = true;
                    var responsibleUsersAvailable = this._userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
                    var customerSettings = this._settingService.GetCustomerSetting(customerId);
                    m.ResponsibleUsersAvailable = responsibleUsersAvailable.MapToSelectList(customerSettings, isAddEmpty);
                    m.SendToDialogModel = this.CreateNewSendToDialogModel(customerId, responsibleUsersAvailable.ToList());
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
                    m.EditMode = EditMode(m, ModuleName.Log, deps, acccessToGroups);
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
        public RedirectToRouteResult EditLog(DHDomain.Case case_, CaseLog caseLog)
        {
            IDictionary<string, string> errors;
            int caseHistoryId;

            if (caseLog.FinishingType != null && caseLog.FinishingType != 0)
            {
                var c = this._caseService.GetCaseById(caseLog.CaseId);
                // save case and case history
                c.FinishingDescription = case_.FinishingDescription;
                caseHistoryId = this._caseService.SaveCase(c, caseLog, null, SessionFacade.CurrentUser.Id, this.User.Identity.Name, out errors);
                caseLog.CaseHistoryId = caseHistoryId;
            }
            this._logService.SaveLog(caseLog, 0, out errors);
            return this.RedirectToAction("edit", "cases", new { id = caseLog.CaseId });
        }

        [HttpPost]
        public ActionResult Search_User(string query, int customerId)
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

            return this.Json(result);
        }

        [HttpPost]
        public JsonResult Get_User(int id)
        {
            var cu = this._computerService.GetComputerUser(id);
            var u = new
            {
                num = cu.UserId,
                name = cu.SurName + ' ' + cu.FirstName,
                email = cu.Email,
                place = cu.Location,
                phone = cu.Phone,
                usercode = cu.UserCode,
                cellphone = cu.Cellphone,
                regionid = (cu.Department != null) ? cu.Department.Region_Id == null ? string.Empty : cu.Department.Region_Id.Value.ToString() : string.Empty,
                regionname = (cu.Department != null) ? cu.Department.Region != null ? cu.Department.Region.Name : string.Empty : string.Empty,
                departmentid = cu.Department_Id,
                departmentname = cu.Department.DepartmentName,
                ouid = cu.OU_Id,
                ouname = cu.OU != null ? (cu.OU.Parent != null ? cu.OU.Parent.Name + " - " : string.Empty) + cu.OU.Name : string.Empty
            };
            return this.Json(u);
        }

        [HttpPost]
        public ActionResult Search_Computer(string query, int customerId)
        {
            var result = this._computerService.SearchComputer(customerId, query);
            return this.Json(result);
        }

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
            var list = id.HasValue ? this._supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._supplierService.GetSuppliers(customerId).Select(x => new { id = x.Id, name = x.Name });
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

            if (id.HasValue)
            {
                var s = _stateSecondaryService.GetStateSecondary(id.Value);
                noMailToNotifier = s != null ? s.NoMailToNotifier : 0;
                workinggroupId = s != null ? s.WorkingGroup_Id.HasValue ? s.WorkingGroup_Id.Value : 0 : 0;
            }
            return Json(new { NoMailToNotifier = noMailToNotifier, WorkingGroup_Id = workinggroupId });
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

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        [HttpGet]
        public ActionResult Files(string id)
        {
            //var files = this._caseFileService.GetCaseFiles(int.Parse(id));
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Cases)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            var cfs = MakeCaseFileModel(files);

            var model = new CaseFilesModel(id, cfs.ToArray());
            return this.PartialView("_CaseFiles", model);
        }

        public JsonResult MarkAsUnread(int id, int customerId)
        {
            this._caseService.MarkAsUnread(id);
            //return this.RedirectToAction("index", "cases", new { customerId = customerId });
            return Json("Success");
        }

        [HttpGet]
        public ActionResult LogFiles(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Log)
                                : this._logFileService.FindFileNamesByLogId(int.Parse(id));

            var model = new FilesModel(id, files);
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
                    this._caseService.Activate(id, SessionFacade.CurrentUser.Id, adUser, out errors);

            return this.RedirectToAction("edit", "cases", new { id, redirectFrom = "save", backUrl });
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
                this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, out errors, string.Empty, extraField);
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
                    this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, out errors, string.Empty, extraField);
                }
            }
        }

        [HttpPost]
        public RedirectToRouteResult DeleteCase(int caseId, int customerId)
        {
            var basePath = _masterDataService.GetFilePath(customerId);
            var caseGuid = this._caseService.Delete(caseId, basePath);
            this.userTemporaryFilesStorage.ResetCacheForObject(caseGuid.ToString());
            return this.RedirectToAction("index", "cases", new { customerId = customerId });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            var tmpLog = this._logService.GetLogById(id);
            var logFiles = this._logFileService.FindFileNamesByLogId(id);
            var c = this._caseService.GetCaseById(caseId);
            var basePath = _masterDataService.GetFilePath(c.Customer_Id);
            var logGuid = this._logService.Delete(id, basePath);
            this.userTemporaryFilesStorage.ResetCacheForObject(logGuid.ToString());

            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;


            var logFileStr = string.Empty;
            if (logFiles.Any())
            {
                logFileStr = string.Format("{0}{1}", StringTags.LogFile, string.Join(StringTags.Seperator, logFiles.ToArray()));
            }

            var logStr = string.Format("{0}{1}{2}{3}{4}{5}",
                                       StringTags.Delete,
                                       StringTags.ExternalLog,
                                       tmpLog.TextExternal,
                                       StringTags.InternalLog,
                                       tmpLog.TextInternal,
                                       logFileStr);

            var extraField = new ExtraFieldCaseHistory { CaseLog = logStr };
            this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, out errors, string.Empty, extraField);

            return this.RedirectToAction("edit", "cases", new { id = caseId });
        }

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
                            frm.GetDate("CaseRegistrationDateStartFilter"),
                            frm.GetDate("CaseRegistrationDateEndFilter"),
                            frm.GetDate("CaseWatchDateStartFilter"),
                            frm.GetDate("CaseWatchDateEndFilter"),
                            frm.GetDate("CaseClosingDateStartFilter"),
                            frm.GetDate("CaseClosingDateEndFilter"),
                            frm.IsFormValueTrue("CaseRegistrationDateFilterShow"),
                            frm.IsFormValueTrue("CaseWatchDateFilterShow"),
                            frm.IsFormValueTrue("CaseClosingDateFilterShow"),
                            closingReason,
                            frm.IsFormValueTrue("CaseInitiatorFilterShow"));

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
            var res = new RedirectToRouteResult(prevInfo);
            return res;
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

        #region Private Methods and Operators

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

        private RouteValueDictionary ExtractPreviousRouteInfo()
        {
            var fullUrl = Request.UrlReferrer.ToString();
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

        private int Save(CaseEditInput m)
        {
            var utcNow = DateTime.UtcNow;
            var case_ = m.case_;
            var caseLog = m.caseLog;
            var caseMailSetting = m.caseMailSetting;
            var updateNotifierInformation = m.updateNotifierInformation;
            var caseInvoiceArticles = m.caseInvoiceArticles;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            case_.Performer_User_Id = m.Performer_Id;
            case_.CaseResponsibleUser_Id = m.ResponsibleUser_Id;
            case_.RegistrationSourceCustomer_Id = m.customerRegistrationSourceId;
            case_.Ou = null;
            case_.Department = null;
            case_.Region = null;
            bool edit = case_.Id == 0 ? false : true;
            IDictionary<string, string> errors;

            var mailSenders = new MailSenders();

            if (case_.RegLanguage_Id == 0)
            {
                case_.RegLanguage_Id = SessionFacade.CurrentLanguageId;
            }

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

                    if (cu.PriorityPermission == 0)
                    {
                        case_.Priority_Id = oldCase.Priority_Id;
                    }

                    if (cu.StateSecondaryPermission == 0)
                    {
                        case_.StateSecondary_Id = oldCase.StateSecondary_Id;
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
                                SessionFacade.CurrentCustomer.WorkingDayStart,
                                SessionFacade.CurrentCustomer.WorkingDayEnd,
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
                    SessionFacade.CurrentCustomer.WorkingDayStart,
                    SessionFacade.CurrentCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                int[] deptIds = null;
                if (case_.Department_Id.HasValue)
                {
                    deptIds = new int[] { case_.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, case_.FinishingDate.Value, deptIds);
                case_.LeadTime = workTimeCalc.CalculateWorkTime(
                    case_.RegTime,
                    case_.FinishingDate.Value.ToUniversalTime(),
                    case_.Department_Id) - case_.ExternalTime;
            }

            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(
                        case_,
                        caseLog,
                        caseMailSetting,
                        SessionFacade.CurrentUser.Id,
                        this.User.Identity.Name,
                        out errors,
                        this.invoiceHelper.ToCaseInvoices(caseInvoiceArticles, null, null));

            if (updateNotifierInformation.HasValue && updateNotifierInformation.Value)
            {
                var names = case_.PersonsName.Split(' ');

                var fName = string.Empty;
                var lName = string.Empty;

                if (names.Length > 0)
                {
                    for (int i = 0; i < names.Length - 1; i++)
                        if (i == 0)
                            lName = names[i];
                        else
                            lName += " " + names[i];
                }

                if (names.Length > 1)
                    fName = names[names.Length - 1];

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
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);

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
            this._caseService.SendCaseEmail(case_.Id, caseMailSetting, caseHistoryId, basePath, oldCase, caseLog, newLogFiles);

            //Unlock Case            
            if (m.caseLock != null && !string.IsNullOrEmpty(m.caseLock.LockGUID))
                this._caseLockService.UnlockCaseByGUID(new Guid(m.caseLock.LockGUID));

            // delete temp folders                
            this.userTemporaryFilesStorage.ResetCacheForObject(case_.CaseGUID.ToString());
            this.userTemporaryFilesStorage.ResetCacheForObject(caseLog.LogGuid.ToString());

            return case_.Id;
        }

        private CaseSearchModel InitCaseSearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter();
            var m = new CaseSearchModel();
            var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
            {
                throw new Exception("An error occurred, please refresh your page. Please, logout and login again.");
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

        /// <summary>
        /// The get case input view model.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <param name="lockedByUserId">
        /// The locked by user id.
        /// </param>
        /// <param name="redirectFrom">
        /// The redirect from.
        /// </param>
        /// <param name="templateId">
        /// The template id.
        /// </param>
        /// <param name="copyFromCaseId">
        /// The copy from case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseInputViewModel"/>.
        /// </returns>
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
            int? templateistrue = 0)
        {
            var m = new CaseInputViewModel();
            m.BackUrl = backUrl;
            m.CanGetRelatedCases = SessionFacade.CurrentUser.IsAdministrator();
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
            var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);
            var isCreateNewCase = caseId == 0;
            m.CaseLock = caseLocked;
            if (!isCreateNewCase)
            {
                var markCaseAsRead = string.IsNullOrWhiteSpace(redirectFrom);
                m.case_ = this._caseService.GetCaseById(caseId);

                var editMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);
                if (m.case_.Unread != 0 && updateState && editMode == Enums.AccessMode.FullAccess)
                    this._caseService.MarkAsRead(caseId);

                customerId = customerId == 0 ? m.case_.Customer_Id : customerId;
                //SessionFacade.CurrentCaseLanguageId = m.case_.RegLanguage_Id;
            }

            var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
            {
                m.case_ = null;
            }
            else
            {
                var case_ = m.case_;
                var customer = this._customerService.GetCustomer(customerId);
                var cs = this._settingService.GetCustomerSetting(customerId);
                m.customerUserSetting = cu;
                m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
                m.DepartmentFilterFormat = cs.DepartmentFilterFormat;
                m.ParantPath_CaseType = ParentPathDefaultValue;
                m.ParantPath_ProductArea = ParentPathDefaultValue;
                m.ParantPath_OU = ParentPathDefaultValue;
                m.MinWorkingTime = cs.MinRegWorkingTime != 0 ? cs.MinRegWorkingTime : 30;
                m.CaseFilesModel = new CaseFilesModel();
                m.LogFilesModel = new FilesModel();
                m.CaseFileNames = GetCaseFileNames(caseId.ToString());
                m.CaseFileNames = GetLogFileNames(caseId.ToString());
                if (isCreateNewCase)
                {
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
                    else
                    {
                        m.case_ = this._caseService.InitCase(
                            customerId,
                            userId,
                            SessionFacade.CurrentLanguageId,
                            this.Request.GetIpAddress(),
                            CaseRegistrationSource.Administrator,
                            cs,
                            windowsUser);
                    }

                    var defaultStateSecondary = this._stateSecondaryService.GetDefaultOverview(customerId);
                    if (defaultStateSecondary != null)
                    {
                        m.case_.StateSecondary_Id = int.Parse(defaultStateSecondary.Value);
                    }
                }
                else
                {
                    m.Logs = this._logService.GetCaseLogOverviews(caseId);
                    m.CaseFilesModel = new CaseFilesModel(caseId.ToString(global::System.Globalization.CultureInfo.InvariantCulture), this._caseFileService.GetCaseFiles(caseId).OrderBy(x => x.CreatedDate).ToArray());
                    if (m.case_.User_Id.HasValue)
                        m.RegByUser = this._userService.GetUser(m.case_.User_Id.Value);
                    if (m.Logs != null)
                    {
                        var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
                        var lastLog = m.Logs.FirstOrDefault();
                        if (lastLog != null)
                        {
                            m.FinishingCause = this.GetFinishingCauseFullPath(finishingCauses.ToArray(), lastLog.FinishingType);
                        }
                    }
                }

                m.CaseMailSetting = new CaseMailSetting(
                                                    customer.NewCaseEmailList,
                                                    customer.HelpdeskEmail,
                                                    RequestExtension.GetAbsoluteUrl(),
                                                    cs.DontConnectUserToWorkingGroup);

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

                // "Workging group" field
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId);
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
                                Text = it.SourceName,
                                Value = it.Id.ToString(),
                                Selected = it.Id == m.CustomerRegistrationSourceId
                            }));
                }

                if (cs.ModuleProject == 1)
                {
                    m.projects = this._projectService.GetCustomerProjects(customerId);
                }

                if (cs.ModuleChangeManagement == 1)
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
                m.SendToDialogModel = this.CreateNewSendToDialogModel(customerId, responsibleUsersList.ToList());
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

                            if (caseTemplate.Category_Id != null)
                            {
                                m.case_.Category_Id = caseTemplate.Category_Id.Value;
                            }

                            if (caseTemplate.UpdateNotifierInformation.HasValue)
                            {
                                m.UpdateNotifierInformation = caseTemplate.UpdateNotifierInformation.Value.ToBool();
                            }

                            m.case_.ReportedBy = caseTemplate.ReportedBy;
                            m.case_.Department_Id = caseTemplate.Department_Id;
                            //m.CaseMailSetting.DontSendMailToNotifier = caseTemplate.NoMailToNotifier.ToBool();
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
                            //m.case_.RegistrationSourceCustomer_Id = caseTemplate.RegistrationSource;
                            if (caseTemplate.RegistrationSource > 0)
                            {
                                m.CustomerRegistrationSourceId = caseTemplate.RegistrationSource;
                                var RegistrationSource = this._registrationSourceCustomerService.GetRegistrationSouceCustomer(caseTemplate.RegistrationSource);
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
                            // This is used for hide fields(which are not in casetemplate) in new case input
                            m.templateistrue = templateistrue;
                            var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
                            m.FinishingCause = this.GetFinishingCauseFullPath(
                                finishingCauses.ToArray(),
                                caseTemplate.FinishingCause_Id);

                            
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
                if (cs.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
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

                // hämta parent path för productArea 
                m.ProductAreaHasChild = 0;
                if (m.case_.ProductArea_Id.HasValue)
                {
                    var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                    if (p != null)
                    {
                        var names = this._productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.Get(name));
                        m.ParantPath_ProductArea = string.Join(" - ", names);
                        if (p.SubProductAreas != null && p.SubProductAreas.Where(s => s.IsActive != 0).ToList().Count > 0)
                            m.ProductAreaHasChild = 1;
                    }
                }

                // hämta parent path för casetype
                if (m.case_.CaseType_Id > 0)
                {
                    var c = this._caseTypeService.GetCaseType(m.case_.CaseType_Id);
                    if (c != null)
                    {
                        c = TranslateCaseType(c);
                        m.ParantPath_CaseType = c.getCaseTypeParentPath();
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

                m.EditMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);

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
                m.Setting = cs;

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
                        .Replace("[Language]", l.LanguageId);
                }

                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
                if (case_ != null)
                {
                    m.MapCaseToCaseInputViewModel(case_, userTimeZone);
                }
            }

            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(customerId, userId);
            return m;
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

        private CaseTemplateTreeModel GetCaseTemplateTreeModel(int customerId, int userId)
        {
            var model = new CaseTemplateTreeModel();
            model.CustomerId = customerId;
            model.CaseTemplateCategoryTree = _caseSolutionService.GetCaseSolutionCategoryTree(customerId, userId);
            return model;
        }

        private SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<DHDomain.User> users)
        {
            var emailGroups = _emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = _workingGroupService.GetWorkingGroupsWithActiveEmails(customerId);
            var administrators = new List<ItemOverview>();

            if (users != null)
                foreach (var u in users)
                {
                    if (u.IsActive == 1 && u.Performer == 1 && _emailService.IsValidEmail(u.Email) && !String.IsNullOrWhiteSpace(u.Email))
                        administrators.Add(new ItemOverview(u.SurName + " " + u.FirstName, u.Email));
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

        private void AddViewDataValues()
        {
            ViewData["Callback"] = "SendToDialogCaseCallback";
            ViewData["Id"] = "divSendToDialogCase";
        }

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
        private Enums.AccessMode EditMode(CaseInputViewModel m, string topic, IList<DHDomain.Department> departmensForUser, List<CustomerWorkingGroupForUser> accessToWorkinggroups)
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
                            return Enums.AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {
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

        private CaseSettingModel GetCaseSettingModel(int customerId, int userId)
        {
            var ret = new CaseSettingModel();

            ret.CustomerId = customerId;
            ret.UserId = userId;

            var userCaseSettings = _customerUserService.GetUserCaseSettings(customerId, userId);

            var regions = this._regionService.GetRegions(customerId);
            ret.RegionCheck = userCaseSettings.Region != string.Empty;
            ret.Regions = regions;
            ret.SelectedRegion = userCaseSettings.Region;

            var departments = this._departmentService.GetDepartments(customerId, ActivationStatus.All);
            ret.IsDepartmentChecked = userCaseSettings.Departments != string.Empty;
            ret.Departments = departments;
            ret.SelectedDepartments = userCaseSettings.Departments;

            var customerSettings = this._settingService.GetCustomerSetting(customerId);
            ret.RegisteredByCheck = userCaseSettings.RegisteredBy != string.Empty;
            ret.RegisteredByUserList = this._userService.GetUserOnCases(customerId).MapToSelectList(customerSettings);

            if (!string.IsNullOrEmpty(userCaseSettings.RegisteredBy))
            {
                ret.lstRegisterBy = userCaseSettings.RegisteredBy.Split(',').Select(int.Parse).ToArray();
            }

            ret.CaseTypeCheck = userCaseSettings.CaseType != string.Empty;
            ret.CaseTypes = this._caseTypeService.GetCaseTypes(customerId);
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
            var workingGroups =
                _workingGroupService.GetWorkingGroups(customerId, true).ToList();
            //.Where(w => userWorkingGroup.Contains(w.Id))
            ret.WorkingGroupCheck = (userCaseSettings.WorkingGroup != string.Empty);
            ret.WorkingGroups = workingGroups;
            ret.SelectedWorkingGroup = userCaseSettings.WorkingGroup;

            ret.ResponsibleCheck = userCaseSettings.Responsible;

            ret.AdministratorCheck = true;
            ret.AvailablePerformersList = this._userService.GetAvailablePerformersOrUserId(customerId).MapToSelectList(customerSettings);
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


        private string GetIdsFromSearchResult(IList<CaseSearchResult> cases)
        {
            if (cases == null)
                return string.Empty;

            return string.Join(",", cases.Select(c => c.Id));
        }

        private List<CaseFileModel> MakeCaseFileModel(List<string> files)
        {
            var res = new List<CaseFileModel>();
            int i = 0;

            foreach (var f in files)
            {
                i++;
                var cf = new CaseFileModel(i, i, f, DateTime.Now, SessionFacade.CurrentUser.FirstName + " " + SessionFacade.CurrentUser.SurName);
                res.Add(cf);
            }

            return res;
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

        private DHDomain.CaseType TranslateCaseType(DHDomain.CaseType caseType)
        {
            if (caseType.ParentCaseType != null)
                caseType.ParentCaseType = TranslateCaseType(caseType.ParentCaseType);

            caseType.Name = Translation.Get(caseType.Name);

            return caseType;
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
                                    var names = this._productAreaService.GetParentPath(c.Id, customerId).Select(name => Translation.Get(name));
                                    c.StringValue = string.Join(" - ", names);
                                }

                                break;
                        }
                    }
                }
            }

            return ret;
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
            var extendedSec = (gs != null && gs.CaseLockTimer > 0 ? gs.CaseLockTimer : this._defaultExtendCaseLockTime);
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

            return caseLock.MapToViewModel(caseIsLocked, extendedSec);
        }

        #endregion
    }
}
