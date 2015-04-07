using System.Collections.Specialized;
using System.IO;
using System.Web.Routing;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Helpers;
    using System.Web.UI.DataVisualization.Charting;
    using System.Web.UI.WebControls.Expressions;

    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Shared;
    
    using DH.Helpdesk.Web.Models.Case.Input;
    using DH.Helpdesk.Web.Infrastructure.Grid;

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
            CaseOverviewGridSettingsService caseOverviewSettingsService, GridSettingsService gridSettingsService, OutputFormatter outputFormatter)
            : base(masterDataService)
        {            
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
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index(int? customerId, bool? clearFilters = false, CasesCustomFilter customFilter = CasesCustomFilter.None, bool? DefaultSearch = false)
        {                        
            if (SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);
            if (clearFilters == true)
            {
                SessionFacade.CurrentCaseSearch = null;                
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                SessionFacade.CurrentCustomer = this._customerService.GetCustomer(customerId.HasValue ? customerId.Value : SessionFacade.CurrentUser.CustomerId);
                SessionFacade.CaseOverviewGridSettings = null;
            }
            else
            {
                if (customerId.HasValue && customerId.Value != SessionFacade.CurrentCustomer.Id)
                {
                    SessionFacade.CurrentCustomer = this._customerService.GetCustomer(customerId.Value);
                    SessionFacade.CaseOverviewGridSettings = null;
                }
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                    this.gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }

            var m = new JsonCaseIndexViewModel
            {
                GridSettings = JsonGridSettingsMapper.ToJsonGridSettingsModel(SessionFacade.CaseOverviewGridSettings, SessionFacade.CurrentCustomer.Id)
            };
            var customerUser = this._customerUserService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            var caseSearchModel = this.GetCaseSearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            m.CaseSearchFilterData = this.CreateCaseSearchFilterData(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id, clearFilters.HasValue && clearFilters.Value, customerUser, caseSearchModel);
            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            m.CaseSetting = this.GetCaseSettingModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);

            //Set refreshcontent
            User user = new User();
            user = _userService.GetUser(SessionFacade.CurrentUser.Id);
            m.CaseSetting.RefreshContent = user.RefreshContent;
            m.ShowRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;

            return this.View("IndexAjax", m);
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
            f.CaseType = frm.ReturnFormValue("hidFilterCaseTypeId").convertStringToInt();
            f.ProductArea = frm.ReturnFormValue("hidFilterProductAreaId").ReturnCustomerUserValue();
            f.Region = frm.ReturnFormValue("lstFilterRegion");
            f.Country = frm.ReturnFormValue("lstFilterCountry");
            f.Department = frm.ReturnFormValue("lstFilterDepartment");
            f.User = frm.ReturnFormValue("lstFilterUser");
            f.Category = frm.ReturnFormValue("lstFilterCategory");
            f.WorkingGroup = frm.ReturnFormValue("lstFilterWorkingGroup");
            f.UserPerformer = frm.ReturnFormValue("lstFilterPerformer");
            f.UserResponsible = frm.ReturnFormValue("lstFilterResponsible");
            f.Priority = frm.ReturnFormValue("lstFilterPriority");
            f.Status = frm.ReturnFormValue("lstFilterStatus");
            f.StateSecondary = frm.ReturnFormValue("lstFilterStateSecondary");
            f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");
            f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");
            f.CaseRegistrationDateStartFilter = frm.GetDate("CaseRegistrationDateStartFilter");
            f.CaseRegistrationDateEndFilter = frm.GetDate("CaseRegistrationDateEndFilter");
            f.CaseWatchDateStartFilter = frm.GetDate("CaseWatchDateStartFilter");
            f.CaseWatchDateEndFilter = frm.GetDate("CaseWatchDateEndFilter");
            f.CaseClosingDateStartFilter = frm.GetDate("CaseClosingDateStartFilter");
            f.CaseClosingDateEndFilter = frm.GetDate("CaseClosingDateEndFilter");
            f.CaseClosingReasonFilter = frm.ReturnFormValue("hidFilterClosingReasonId").ReturnCustomerUserValue();

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
            
            var sm = this.GetCaseSearchModel(f.CustomerId, f.UserId);
            sm.caseSearchFilter = f;
           
            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                    this.gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }
            else
            {
                //////////////////////////////////////////////////////////////////////////
                //// @TODO (alexander.semenischev): put validation for sortOpt.sortBy
                SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy = frm.ReturnFormValue("sortBy");
                var sortDir = 0;
                SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir = (!string.IsNullOrEmpty(frm.ReturnFormValue("sortDir"))
                                   && int.TryParse(frm.ReturnFormValue("sortDir"), out sortDir)
                                   && sortDir == (int)SortingDirection.Asc) ? SortingDirection.Asc : SortingDirection.Desc;
            }
            var gridSettings = SessionFacade.CaseOverviewGridSettings;
            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;
            m.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
            var workTimeCalc = WorkingTimeCalculatorFactory.CreateFromWorkContext(this.workContext);
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTimeData;
            m.cases = this._caseSearchService.Search(
                f,
                m.caseSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                this.workContext.Customer.WorkingDayStart,
                this.workContext.Customer.WorkingDayEnd,
                workTimeCalc,
                this.configuration.Application.ApplicationId,
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
                        jsRow.Add(col.name, this.outputFormatter.FormatField(searchCol));
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }
                }

                data.Add(jsRow);
            }
            var caseRemainingModel = this.caseModelFactory.GetCaseRemainingTimeModel(remainingTimeData);
            return this.Json(new { result = "success", data = data, remainingView = this.RenderPartialViewToString("CaseRemainingTime", caseRemainingModel) });
        }

        private CaseSearchFilterData CreateCaseSearchFilterData(int cusId, int userId, bool clearFilters, CustomerUser cu, CaseSearchModel sm)
        {
            var fd = new CaseSearchFilterData
            {
                IsClearFilters = clearFilters == true,
                customerUserSetting = cu,
                customerSetting = this._settingService.GetCustomerSetting(cusId),
                filterCustomerId = cusId
            };

            //region
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter))
                fd.filterRegion = this._regionService.GetRegions(cusId);
            //land
            if (fd.customerSetting.DepartmentFilterFormat == 1)
                fd.filterCountry = this._countryService.GetCountries(cusId);

            // avdelningar per användare, är den tom så visa alla som kopplade till kunden
            fd.filterDepartment = this._departmentService.GetDepartmentsByUserPermissions(userId, cusId);
            if (!fd.filterDepartment.Any())
            {
                fd.filterDepartment = this._departmentService.GetDepartments(cusId)
                                                                .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                                                                .ToList();
            }

            //användare
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseUserFilter))
                fd.filterCaseUser = this._userService.GetUserOnCases(cusId);
            //ansvarig
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseResponsibleFilter))
                fd.filterUser = this._userService.GetUsers(cusId);
            //utförare
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePerformerFilter))
            {
                fd.filterPerformer = this._userService.GetUsers(cusId);
                // visa även ej tilldelade
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.RestrictedCasePermission == 0)
                    fd.filterPerformer.Insert(0, ObjectExtensions.notAssignedPerformer());
            }
            //ärendetyp
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter))
                fd.filterCaseType = this._caseTypeService.GetCaseTypes(cusId);
            //working group
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseWorkingGroupFilter))
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();

                if (gs.LockCaseToWorkingGroup == 0)
                    fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId);
                else
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId);
                // visa även ej tilldelade
                if (SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups == 1)
                    fd.filterWorkingGroup.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
            }
            //produktonmråde
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseProductAreaFilter))
                fd.filterProductArea = this._productAreaService.GetTopProductAreas(cusId);
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

            return fd;
        }

        public ActionResult New(int customerId, int? templateId, int? copyFromCaseId, int? caseLanguageId, int? templateistrue)
        {
            CaseInputViewModel m = null;
          
            //if (caseLanguageId == null)
               SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            //else
            //   SessionFacade.CurrentCaseLanguageId = (int) caseLanguageId.Value;

            if (SessionFacade.CurrentUser != null)
                if (SessionFacade.CurrentUser.CreateCasePermission == 1)
                {
                    var userId = SessionFacade.CurrentUser.Id;
                    m = this.GetCaseInputViewModel(userId, customerId, 0, 0, "", null, templateId, copyFromCaseId, false, templateistrue);

                    var caseParam = new NewCaseParams
                    {
                        customerId = customerId,
                        templateId = templateId,
                        copyFromCaseId = copyFromCaseId,
                        caseLanguageId = caseLanguageId
                    };

                    m.NewModeParams = caseParam;
                    AddViewDataValues();

                    // Positive: Send Mail to...
                    if (m.CaseMailSetting.DontSendMailToNotifier == false)
                        m.CaseMailSetting.DontSendMailToNotifier = true;
                    else
                        m.CaseMailSetting.DontSendMailToNotifier = false;

                    return this.View(m);
                }

            return this.RedirectToAction("index", "cases", new { id = customerId });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult New(
                                    Case case_, 
                                    CaseLog caseLog, 
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles,
                                    int? templateId)
        {
            int caseId = this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);

            CheckTemplateParameters(templateId, caseId);

            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = updateNotifierInformation });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndClose(
                                    Case case_, 
                                    CaseLog caseLog, 
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles,
                                    int? templateId)
        {
            int caseId = this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);

            CheckTemplateParameters(templateId, caseId);

            return this.RedirectToAction("index", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndAddCase(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles,
                                    int? templateId)
        {
            int caseId = this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);

            CheckTemplateParameters(templateId, caseId);

            return this.RedirectToAction("new", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult Edit(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles)
        {
            int caseId = this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = updateNotifierInformation, updateState = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult EditAndClose(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles,
                                    string BackUrl)
        {
            this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);

            return string.IsNullOrEmpty(BackUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = case_.Customer_Id })) : this.Redirect(BackUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditAndAddCase(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles)
        {
            this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
            return this.RedirectToAction("new", "cases", new { customerId = case_.Customer_Id });
        }

        public ActionResult Edit(int id, string redirectFrom = "", int? moveToCustomerId = null, bool? uni = null, bool updateState = true, string backUrl = null)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                int lockedByUserId = 0;

                var caseUserInfo = ApplicationFacade.GetUserCaseInfo(id);
                if (caseUserInfo == null)
                    ApplicationFacade.AddCaseUserInfo(userId, id);

                caseUserInfo = ApplicationFacade.GetUserCaseInfo(id);
                if (caseUserInfo != null && caseUserInfo.UserId != userId)
                {
                    lockedByUserId = caseUserInfo.UserId;
                }

                int customerId = moveToCustomerId.HasValue ? moveToCustomerId.Value : _caseService.GetCaseById(id).Customer_Id;


                m = this.GetCaseInputViewModel(userId, customerId, id, lockedByUserId, redirectFrom, backUrl, null, null, updateState);
                if (uni.HasValue)
                {
                    m.UpdateNotifierInformation = uni.Value;                    
                }

                if(lockedByUserId == userId || lockedByUserId == 0)
                    ApplicationFacade.UpdateLoggedInUser(Session.SessionID, m.case_.CaseNumber.ToString(), m.case_.Id);

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
                    m.case_.User_Id = 0;
                    m.case_.WorkingGroup_Id = null;
                    m.ParantPath_CaseType = ParentPathDefaultValue;
                    m.ParantPath_ProductArea = ParentPathDefaultValue;
                    m.ParantPath_OU = ParentPathDefaultValue;
                }

                var caseInvoices = this.invoiceArticleService.GetCaseInvoices(id);
                m.InvoiceArticles = this.invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
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
                    m.users = this._userService.GetUsers(customerId);
                    m.LogFilesModel = new FilesModel(id.ToString(), this._logFileService.FindFileNamesByLogId(id));
                    m.SendToDialogModel = CreateNewSendToDialogModel(customerId, m.users);
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

                    m.CustomerSettings = this.workContext.Customer.Settings;
                    m.EditMode = EditMode(m, ModuleName.Log, deps, acccessToGroups);
                    AddViewDataValues();

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
                regionid = (cu.Department != null)? cu.Department.Region_Id == null ? "" : cu.Department.Region_Id.Value.ToString() : "",
                regionname = (cu.Department != null)? cu.Department.Region != null? cu.Department.Region.Name : "" : "",
                departmentid = cu.Department_Id,
                departmentname = cu.Department.DepartmentName,
                ouid = cu.OU_Id,
                ouname = cu.OU != null ? (cu.OU.Parent != null ? cu.OU.Parent.Name + " - " : "") + cu.OU.Name : ""
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
            var dep = this._departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId);
            if (!dep.Any())
            {
                dep = this._departmentService.GetDepartments(customerId)
                                             .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                                             .ToList();
            }

            if (id.HasValue)
            {
                var curRegion = this._regionService.GetRegion(id.Value);
                if (curRegion.IsActive != 0)
                  dep = dep.Where(x => x.Region_Id == id).ToList();
            }

            var list = dep.Select(x => new { id = x.Id, name = x.DepartmentDescription(departmentFilterFormat) });

            return this.Json(new { list });
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            var list = id.HasValue
                           ? this._userService.GetUsersForWorkingGroup(customerId, id.GetValueOrDefault())
                                 .Where(x => x.IsActive == 1)
                                 .Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName })
                           : this._userService.GetUsers(customerId)
                                 .Where(x => x.IsActive == 1)
                                 .Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName });
            return this.Json(new { list });
        }

        public int ChangeWorkingGroupSetStateSecondary(int? id)
        {
            int ret = 0;
            if (id.HasValue)
            {
                WorkingGroupEntity w = this._workingGroupService.GetWorkingGroup(id.Value);
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

        public JsonResult ChangeDepartment(int? id, int customerId, int departmentFilterFormat)
        {
            var prelist =
            id.HasValue ?
                this._ouService.GetOUs(customerId)
                         .Where(e => e.IsActive == 1 && id.GetValueOrDefault() == e.Department_Id)
                :
                null;
                //this._ouService.GetOUs(customerId);            
            
            var unionList = new Dictionary<int,string>();
            if (prelist != null)
            {
                foreach (var ou in prelist)
                {
                    unionList.Add(ou.Id, ou.Name);
                    foreach (var s in ou.SubOUs.Where(e => e.IsActive == 1))
                        unionList.Add(s.Id, ou.Name + " - " + s.Name);
                }
            }

            var list = unionList.Select(x => new { id = x.Key, name = x.Value });
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._supplierService.GetSuppliers(customerId).Select(x => new { id = x.Id, name = x.Name });
            return this.Json(new { list });
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
                    if (e.SubProductAreas != null && e.SubProductAreas.Where(s=> s.IsActive != 0).ToList().Count > 0)
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
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases)
                                  : this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);
            return new UnicodeFileContentResult(fileContent, fileName);
        }

        [HttpGet]
        public UnicodeFileContentResult DownloadLogFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log)
                                  : this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);

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
                    //return;
                    //throw new HttpException(409 , "Already file exist!"); because it take a long time.

                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Cases);                               
                    name = DateTime.Now.ToString() + '-' + name;
                }                
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Cases);                               
            }
            else
            {
                if (this._caseFileService.FileExists(int.Parse(id), name))
                {
                    //return;
                    //throw new HttpException(409, "Already file exist!");    because it take a long time.
                    //this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), name);   
                    name =  DateTime.Now.ToString() + '_' + name;
                }

                var caseFileDto = new CaseFileDto(
                                uploadedData, 
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

//        public ActionResult Search(FormCollection frm)
//        {
//            var f = new CaseSearchFilter();
//            var m = new CaseSearchResultModel();
//            
//            if (SessionFacade.CurrentUser != null)
//            {
//                m.GridSettings =
//                    this.caseOverviewSettingsService.GetSettings(
//                        SessionFacade.CurrentCustomer.Id,
//                        SessionFacade.CurrentUser.UserGroupId,
//                        SessionFacade.CurrentUser.Id);
//                f.CustomerId = frm.ReturnFormValue("hidFilterCustomerId").convertStringToInt();
//                f.UserId = SessionFacade.CurrentUser.Id;
//                f.CaseType = frm.ReturnFormValue("hidFilterCaseTypeId").convertStringToInt();
//                f.ProductArea = frm.ReturnFormValue("hidFilterProductAreaId").ReturnCustomerUserValue();
//                f.Region = frm.ReturnFormValue("lstFilterRegion");
//                f.Country = frm.ReturnFormValue("lstFilterCountry");
//                f.Department = frm.ReturnFormValue("lstFilterDepartment");
//                f.User = frm.ReturnFormValue("lstFilterUser");
//                f.Category = frm.ReturnFormValue("lstFilterCategory");
//                f.WorkingGroup = frm.ReturnFormValue("lstFilterWorkingGroup");
//                f.UserPerformer = frm.ReturnFormValue("lstFilterPerformer");
//                f.UserResponsible = frm.ReturnFormValue("lstFilterResponsible");
//                f.Priority = frm.ReturnFormValue("lstFilterPriority");
//                f.Status = frm.ReturnFormValue("lstFilterStatus");
//                f.StateSecondary = frm.ReturnFormValue("lstFilterStateSecondary");
//                f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");
//                f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");
//                f.CaseRegistrationDateStartFilter = frm.GetDate("CaseRegistrationDateStartFilter");
//                f.CaseRegistrationDateEndFilter = frm.GetDate("CaseRegistrationDateEndFilter");
//                f.CaseWatchDateStartFilter = frm.GetDate("CaseWatchDateStartFilter");
//                f.CaseWatchDateEndFilter = frm.GetDate("CaseWatchDateEndFilter");
//                f.CaseClosingDateStartFilter = frm.GetDate("CaseClosingDateStartFilter");
//                f.CaseClosingDateEndFilter = frm.GetDate("CaseClosingDateEndFilter");
//                f.CaseClosingReasonFilter = frm.ReturnFormValue("hidFilterClosingReasonId").ReturnCustomerUserValue();
//
//                int caseRemainingTimeFilter;
//                if (int.TryParse(frm.ReturnFormValue("CaseRemainingTime"), out caseRemainingTimeFilter))
//                {
//                    f.CaseRemainingTimeFilter = caseRemainingTimeFilter;
//                }
//
//                int caseRemainingTimeUntilFilter;
//                if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeUntil"), out caseRemainingTimeUntilFilter))
//                {
//                    f.CaseRemainingTimeUntilFilter = caseRemainingTimeUntilFilter;
//                }
//
//                int caseRemainingTimeMaxFilter;
//                if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeMax"), out caseRemainingTimeMaxFilter))
//                {
//                    f.CaseRemainingTimeMaxFilter = caseRemainingTimeMaxFilter;
//                }
//
//                bool caseRemainingTimeHoursFilter;
//                if (bool.TryParse(frm.ReturnFormValue("CaseRemainingTimeHours"), out caseRemainingTimeHoursFilter))
//                {
//                    f.CaseRemainingTimeHoursFilter = caseRemainingTimeHoursFilter;
//                }
//
//                var sm = this.GetCaseSearchModel(f.CustomerId, f.UserId);
//                sm.caseSearchFilter = f;
//                sm.Search.SortBy = frm.ReturnFormValue("hidSortBy");
//                sm.Search.Ascending = frm.ReturnFormValue("hidSortByAsc").convertStringToBool();
//
//                m.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
//                var workTimeCalc = WorkingTimeCalculatorFactory.CreateFromWorkContext(this.workContext);
//                var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
//                CaseRemainingTimeData remainingTime;
//                m.cases = this._caseSearchService.Search(
//                    f,
//                    m.caseSettings,
//                    SessionFacade.CurrentUser.Id,
//                    SessionFacade.CurrentUser.UserId,
//                    SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
//                    SessionFacade.CurrentUser.UserGroupId,
//                    SessionFacade.CurrentUser.RestrictedCasePermission,
//                    sm.Search,
//                    this.workContext.Customer.WorkingDayStart,
//                    this.workContext.Customer.WorkingDayEnd,
//                    workTimeCalc,
//                    this.configuration.Application.ApplicationId,
//                    showRemainingTime,
//                    out remainingTime);
//
//                m.cases = this.TreeTranslate(m.cases, f.CustomerId);
//                sm.Search.IdsForLastSearch = GetIdsFromSearchResult(m.cases);
//                SessionFacade.CurrentCaseSearch = sm;
//                m.ShowRemainingTime = showRemainingTime;
//                m.RemainingTime = this.caseModelFactory.GetCaseRemainingTimeModel(remainingTime);
//            }
//
//            return this.PartialView("_CaseRows", m);
//        }

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
                this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), fileName.Trim());
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this.userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);
            else
                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), fileName.Trim());
        }

        [HttpPost]
        public RedirectToRouteResult DeleteCase(int caseId, int customerId)
        {
            var caseGuid = this._caseService.Delete(caseId);
            this.userTemporaryFilesStorage.ResetCacheForObject(caseGuid.ToString());
            return this.RedirectToAction("index", "cases", new { customerId = customerId });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            var logGuid = this._logService.Delete(id);
            this.userTemporaryFilesStorage.ResetCacheForObject(logGuid.ToString());
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

            //bool administratorCheck = frm["AdministratorCheck"].Contains("true"); it is always true  
            var administrator = (frm.ReturnFormValue("lstAdministrator") == string.Empty)
                ? "0"
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
                            closingReason);

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

            if(language != null)
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
                productArea.SubProductAreas.Where(p=> p.IsActive != 0).ToList().Count > 0)
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
                    string relatedCasesUserId = null)
        {
            var searchResult = new CaseSearchResultModel();
            searchResult.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(
                                    SessionFacade.CurrentCustomer.Id,
                                    SessionFacade.CurrentUser.Id,
                                    SessionFacade.CurrentUser.UserGroupId);
            var search = this.GetEmptySearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            search.Search.SortBy = sortBy ?? string.Empty;
            search.Search.Ascending = sortByAsc.convertStringToBool();
            search.caseSearchFilter.CaseProgress = CaseProgressFilter.None;
            var workTimeCalculator = WorkingTimeCalculatorFactory.CreateFromWorkContext(this.workContext);

            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTime;
            searchResult.cases = this._caseSearchService.Search(
                search.caseSearchFilter,
                searchResult.caseSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                search.Search,
                this.workContext.Customer.WorkingDayStart,
                this.workContext.Customer.WorkingDayEnd,
                workTimeCalculator,
                this.configuration.Application.ApplicationId,
                showRemainingTime,
                out remainingTime,
                relatedCasesCaseId,
                relatedCasesUserId);

            searchResult.cases = this.TreeTranslate(searchResult.cases, SessionFacade.CurrentCustomer.Id);
            searchResult.GridSettings = this.caseOverviewSettingsService.GetSettings(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.Id);
            search.Search.IdsForLastSearch = this.GetIdsFromSearchResult(searchResult.cases);

//            searchResult.ShowRemainingTime = showRemainingTime;
//            searchResult.RemainingTime = this.caseModelFactory.GetCaseRemainingTimeModel(remainingTime);
            searchResult.BackUrl = Url.Action("RelatedCasesFull", "Cases", new { area = string.Empty, caseId = relatedCasesCaseId, userId = relatedCasesUserId });
//            SessionFacade.CurrentCaseSearch = search;

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
            if(templateId.HasValue)
            {
                // check template parameters
                var template = _caseSolutionService.GetCaseSolution(templateId.Value);

                // if formGuid this indicates that we want to initate a form by this guid.
                if(template.FormGUID.HasValue)
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
        
        private int Save(
                    Case case_, 
                    CaseLog caseLog, 
                    CaseMailSetting caseMailSetting, 
                    bool? updateNotifierInformation,
                    string caseInvoiceArticles)
        {
            case_.Ou = null;
            case_.Department = null;
            case_.Region = null;
            bool edit = case_.Id == 0 ? false : true;
            IDictionary<string, string> errors;

            var mailSenders = new MailSenders();

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

            
            //var user = _userService.GetUser(case_.User_Id);
            //if (user.Default_WorkingGroup_Id.HasValue)            
            if (case_.DefaultOwnerWG_Id.HasValue && case_.DefaultOwnerWG_Id.Value > 0)
            {
                var defaultWGEmail = _workingGroupService.GetWorkingGroup(case_.DefaultOwnerWG_Id.Value).EMail;
                //var defaultWGEmail = _workingGroupService.GetWorkingGroup(user.Default_WorkingGroup_Id.Value).EMail;                
                mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
            }
            
            // get case as it was before edit
            Case oldCase = new Case();
            if (edit)
            {                
                oldCase = this._caseService.GetDetachedCaseById(case_.Id);
                var cu = this._customerUserService.GetCustomerSettings(case_.Customer_Id, SessionFacade.CurrentUser.Id);
                if (cu != null)
                {
                    if (cu.UserInfoPermission == 0)
                    {
                        // current user are not allowed to see user information, update from old case
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
                        var workTimeCalc = WorkingTimeCalculatorFactory.CreateFromWorkContext(this.workContext);
                        var workingTimeMins = workTimeCalc.CalcWorkTimeMinutes(
                            oldCase.Department_Id,
                            oldCase.ChangeTime,
                            DateTime.UtcNow);
                        case_.ExternalTime = oldCase.ExternalTime + workingTimeMins;
                    }
                }
            }

            if (caseLog.FinishingDate != null)
            {
                case_.FinishingDate = caseLog.FinishingDate;
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

                var fName = "";
                var lName = "";

                if (names.Length > 0)
                {
                    for (int i = 0; i < names.Length - 1; i++)
                        if (i == 0)
                            lName = names[i];
                        else
                            lName += " " + names[i];
                }

                if (names.Length > 1)
                    fName = names[names.Length-1];
                
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

            if (case_.FinishingDate.HasValue)
            {
                if (case_.RegTime > case_.FinishingDate)
                {
                    case_.FinishingDate = case_.RegTime;
                }

                var workTimeCalc = WorkingTimeCalculatorFactory.CreateFromWorkContext(this.workContext);
                case_.LeadTime = workTimeCalc.CalcWorkTimeMinutes(
                    case_.Department_Id,
                    case_.RegTime,
                    case_.FinishingDate.Value) - case_.ExternalTime;
            }

            // save log
            var temporaryLogFiles = this.userTemporaryFilesStorage.FindFiles(caseLog.LogGuid.ToString(), ModuleName.Log);
            caseLog.CaseId = case_.Id;
            caseLog.CaseHistoryId = caseHistoryId;
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
            

            // save case files
            if (!edit)
            {
                var temporaryFiles = this.userTemporaryFilesStorage.FindFiles(case_.CaseGUID.ToString(), ModuleName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, case_.Id, this.workContext.User.UserId)).ToList();
                this._caseFileService.AddFiles(newCaseFiles);
            }            

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, caseLog.Id, this.workContext.User.UserId)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            caseMailSetting.CustomeMailFromAddress = mailSenders;
            // send emails
            this._caseService.SendCaseEmail(case_.Id, caseMailSetting, caseHistoryId, oldCase, caseLog, newLogFiles);

            // delete temp folders                
            this.userTemporaryFilesStorage.ResetCacheForObject(case_.CaseGUID.ToString());
            this.userTemporaryFilesStorage.ResetCacheForObject(caseLog.LogGuid.ToString());

            return case_.Id;
        }

        private CaseSearchModel GetCaseSearchModel(int customerId, int userId)
        {
            CaseSearchModel m;
            var newSearch = false;

            if (SessionFacade.CurrentCaseSearch == null)
                newSearch = true;
            else
                if (SessionFacade.CurrentCaseSearch.caseSearchFilter.CustomerId != customerId)
                    newSearch = true;

            if (newSearch || (!(SessionFacade.CurrentCaseSearch.caseSearchFilter.CustomFilter == CasesCustomFilter.None)))
            {
                ISearch s = new Search();
                var f = new CaseSearchFilter();
                m = new CaseSearchModel();
                var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
                if (cu == null)
                {
                    throw new Exception("It looks that something has happened with your session. Refresh page to fix it.");
                }

                f.CustomerId = customerId;
                f.UserId = userId;
                f.CaseType = cu.CaseCaseTypeFilter.ReturnCustomerUserValue().convertStringToInt();
                f.Category = cu.CaseCategoryFilter.ReturnCustomerUserValue();
                f.Priority = cu.CasePriorityFilter.ReturnCustomerUserValue();
                f.ProductArea = cu.CaseProductAreaFilter.ReturnCustomerUserValue();
                f.Region = cu.CaseRegionFilter.ReturnCustomerUserValue();
                f.StateSecondary = cu.CaseStateSecondaryFilter.ReturnCustomerUserValue();
                f.Status = cu.CaseStatusFilter.ReturnCustomerUserValue();
                f.User = cu.CaseUserFilter.ReturnCustomerUserValue();
                f.UserPerformer = cu.CasePerformerFilter.ReturnCustomerUserValue();
                f.UserResponsible = cu.CaseResponsibleFilter.ReturnCustomerUserValue();
                f.WorkingGroup = cu.CaseWorkingGroupFilter.ReturnCustomerUserValue();
                f.CaseProgress = "2";
                f.CaseRegistrationDateStartFilter = cu.CaseRegistrationDateStartFilter;
                f.CaseRegistrationDateEndFilter = cu.CaseRegistrationDateEndFilter;
                f.CaseWatchDateStartFilter = cu.CaseWatchDateStartFilter;
                f.CaseWatchDateEndFilter = cu.CaseWatchDateEndFilter;
                f.CaseClosingDateStartFilter = cu.CaseClosingDateStartFilter;
                f.CaseClosingDateEndFilter = cu.CaseClosingDateEndFilter;
                f.CaseClosingReasonFilter = cu.CaseClosingReasonFilter.ReturnCustomerUserValue();
                s.SortBy = "CaseNumber";
                s.Ascending = true;

                m.caseSearchFilter = f;
                m.Search = s;
            }
            else
            {
                m = SessionFacade.CurrentCaseSearch;
            }

            return m;
        }

        private CaseSearchModel GetEmptySearchModel(int customerId, int userId)
        {
            CaseSearchModel m;
            
                ISearch s = new Search();
                var f = new CaseSearchFilter();
                m = new CaseSearchModel();
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
                //f.CaseProgress = "2";
                f.CaseRegistrationDateStartFilter = cu.CaseRegistrationDateStartFilter;
                f.CaseRegistrationDateEndFilter = cu.CaseRegistrationDateEndFilter;
                f.CaseWatchDateStartFilter = cu.CaseWatchDateStartFilter;
                f.CaseWatchDateEndFilter = cu.CaseWatchDateEndFilter;
                f.CaseClosingDateStartFilter = cu.CaseClosingDateStartFilter;
                f.CaseClosingDateEndFilter = cu.CaseClosingDateEndFilter;
                f.CaseClosingReasonFilter = null;
                s.SortBy = "CaseNumber";
                s.Ascending = true;

                m.caseSearchFilter = f;
                m.Search = s;
            

            return m;
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
        private CaseInputViewModel GetCaseInputViewModel(int userId, int customerId, int caseId, int lockedByUserId = 0, 
                                                         string redirectFrom = "", 
                                                         string backUrl = null,
                                                         int? templateId = null, 
                                                         int? copyFromCaseId = null, bool updateState = true, int? templateistrue = 0)
        {
            var m = new CaseInputViewModel();
            m.BackUrl = backUrl;
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
            var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);
            if (caseId != 0)
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
                var customer = this._customerService.GetCustomer(customerId);
                var cs = this._settingService.GetCustomerSetting(customerId);
                m.CaseIsLockedByUserId = lockedByUserId;
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

                if (caseId == 0)
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
                            GlobalEnums.RegistrationSource.Case,
                            windowsUser);
                    }
                    else
                    {
                        m.case_ = this._caseService.InitCase(
                            customerId, 
                            userId, 
                            SessionFacade.CurrentLanguageId, 
                            this.Request.GetIpAddress(), 
                            GlobalEnums.RegistrationSource.Case, 
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
                    m.CaseFilesModel = new CaseFilesModel(caseId.ToString(global::System.Globalization.CultureInfo.InvariantCulture), this._caseFileService.GetCaseFiles(caseId).OrderBy(x=> x.CreatedDate).ToArray());
                    m.RegByUser = this._userService.GetUser(m.case_.User_Id);
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

                m.CaseMailSetting.DontSendMailToNotifier = !_customerService.GetCustomer(customerId).CommunicateWithNotifier.ToBool();

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.caseTypes = this._caseTypeService.GetCaseTypes(customerId);
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.categories = this._categoryService.GetCategories(customerId);
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.impacts = this._impactService.GetImpacts(customerId);
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.ous = this._ouService.GetOUs(customerId);                    
                }

                //if (m.case_.OU_Id != null && m.case_.OU_Id.Value > 0)
                //{                    
                //    var o = this._ouService.GetOU(m.case_.OU_Id.Value);
                //    if (o != null)
                //    {
                //        m.ParantPath_OU = o.getOUParentPath();
                //    }
                //}

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.priorities = this._priorityService.GetPriorities(customerId);
                }
                
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.productAreas = this._productAreaService.GetTopProductAreas(customerId);
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

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId);
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
                m.users = this._userService.GetUsers(customerId);
                m.projects = this._projectService.GetCustomerProjects(customerId);
                m.departments = deps.Any() ? deps : 
                      this._departmentService.GetDepartments(customerId)
                                             .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                                             .ToList();                
                m.standardTexts = this._standardTextService.GetStandardTexts(customerId);
                m.Languages = this._languageService.GetActiveLanguages();

                if (cs.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0 && m.case_.Id != 0)
                {
                    m.performers = this._userService.GetUsersForWorkingGroup(customerId, m.case_.WorkingGroup_Id.Value);                    
                }
                else
                {
                    m.performers = m.users;                    
                }

                if (caseId != 0)
                {
                    var admUser = _userService.GetUser(m.case_.Performer_User_Id);
                    if (!m.performers.Contains(admUser) && admUser != null)
                        m.performers.Insert(0, admUser);
                }

                

                m.SendToDialogModel = this.CreateNewSendToDialogModel(customerId, m.users);
                m.CaseLog = this._logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty);
                m.CaseKey = m.case_.Id == 0 ? m.case_.CaseGUID.ToString() : m.case_.Id.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
                m.LogKey = m.CaseLog.LogGuid.ToString();

                if (lockedByUserId > 0)
                {
                    var lbu = this._userService.GetUser(lockedByUserId);
                    m.CaseIsLockedByUserName = lbu != null ? lbu.FirstName + " " + lbu.SurName : string.Empty;
                }

                if (m.case_.Supplier_Id > 0 && m.suppliers != null)
                {
                    var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                    m.CountryId = sup.Country_Id.GetValueOrDefault();
                }

                // Load template info
                if (templateId != null && m.case_.Id == 0)
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
                           m.UpdateNotifierInformation = caseTemplate.UpdateNotifierInformation.Value.ToBool(); 
                        m.case_.ReportedBy = caseTemplate.ReportedBy;
                        m.case_.Department_Id = caseTemplate.Department_Id;
                        m.CaseMailSetting.DontSendMailToNotifier = caseTemplate.NoMailToNotifier.ToBool();
                        m.case_.ProductArea_Id = caseTemplate.ProductArea_Id;
                        m.case_.Caption = caseTemplate.Caption;
                        m.case_.Description = caseTemplate.Description;
                        m.case_.Miscellaneous = caseTemplate.Miscellaneous;
                        m.case_.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
                        m.case_.Priority_Id = caseTemplate.Priority_Id;
                        m.case_.Project_Id = caseTemplate.Project_Id;
                        m.CaseLog.TextExternal = caseTemplate.Text_External;
                        m.CaseLog.TextInternal = caseTemplate.Text_Internal;
                        m.CaseLog.FinishingType = caseTemplate.FinishingCause_Id;
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
                        m.case_.CausingPartId = caseTemplate.FinishingCause_Id;
                        m.case_.ContactBeforeAction = caseTemplate.ContactBeforeAction;
                        m.case_.WatchDate = caseTemplate.WatchDate;
                        m.case_.Project_Id = caseTemplate.Project_Id;
                        m.case_.Problem_Id = caseTemplate.Problem_Id;
                        m.case_.Change_Id = caseTemplate.Change_Id;
                        m.case_.FinishingDate = caseTemplate.FinishingDate;
                        m.case_.FinishingDescription = caseTemplate.FinishingDescription;
                        m.case_.CausingPartId = caseTemplate.FinishingCause_Id;
                        m.case_.PlanDate = caseTemplate.PlanDate;
                      
                        m.CaseTemplateName = caseTemplate.Name;

                        //To get the right users for perfomers when creating a case from a template
                        if (m.case_.WorkingGroup_Id.HasValue)
                            m.performers = this._userService.GetUsersForWorkingGroup(customerId, m.case_.WorkingGroup_Id.Value);

                        // This is used for hide fields(which are not in casetemplate) in new case input
                        m.templateistrue = templateistrue;
                    }
                } // Load Case Template

                // hämta parent path för productArea 
                m.ProductAreaHasChild = 0;
                if (m.case_.ProductArea_Id.HasValue)
                {
                    var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                    if (p != null)
                    {
                        var names = this._productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.Get(name));
                        m.ParantPath_ProductArea = string.Join(" - ", names);
                        if (p.SubProductAreas != null && p.SubProductAreas.Where(s=> s.IsActive != 0).ToList().Count > 0)
                            m.ProductAreaHasChild = 1;
                    }
                }

                // hämta parent path för casetype
                if (m.case_.CaseType_Id > 0)
                {
                    var c = this._caseTypeService.GetCaseType(m.case_.CaseType_Id);                    
                   //c = TranslateCaseType(c);
                    
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
                m.CaseLog.SendMailAboutCaseToNotifier = SessionFacade.CurrentCustomer.CommunicateWithNotifier.ToBool();

                m.Disable_SendMailAboutCaseToNotifier = false;
                if (m.case_.StateSecondary_Id > 0)
                {
                    if (m.case_.StateSecondary != null)
                    {
                        m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                    }                    
                }

                
                m.EditMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);

                if (m.case_.Id == 0)  // new mode
                {
                    m.case_.DefaultOwnerWG_Id = null;
                    if (m.case_.User_Id != 0)
                    {
                        // http://redmine.fastdev.se/issues/10997
                        /*var curUser = _userService.GetUser(m.case_.User_Id);                        

                        if (curUser.Default_WorkingGroup_Id != null)
                           m.case_.DefaultOwnerWG_Id = curUser.Default_WorkingGroup_Id;*/

                        var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(m.case_.User_Id, m.case_.Customer_Id);
                        if (userDefaultWorkingGroupId.HasValue)
                        {
                            m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
                        }
                    }
                }
                else
                {
                    if (m.case_.DefaultOwnerWG_Id.HasValue && m.case_.DefaultOwnerWG_Id.Value > 0)
                        m.CaseOwnerDefaultWorkingGroup = this._workingGroupService.GetWorkingGroup(m.case_.DefaultOwnerWG_Id.Value);                
                }

                //if (m.RegByUser != null && m.RegByUser.Default_WorkingGroup_Id.HasValue)
                //{
                //    m.CaseOwnerDefaultWorkingGroup = this._workingGroupService.GetWorkingGroup(m.RegByUser.Default_WorkingGroup_Id.Value);
                //}

                m.CustomerSettings = this.workContext.Customer.Settings;
                m.DynamicCase = _caseService.GetDynamicCase(m.case_.Id);

                if(m.DynamicCase != null)
                {
                    var l = m.Languages.Where(x => x.Id == SessionFacade.CurrentLanguageId).FirstOrDefault();
                    m.DynamicCase.FormPath = m.DynamicCase.FormPath.Replace("[CaseId]"
                        , m.case_.Id.ToString()).Replace("[UserId]", SessionFacade.CurrentUser.UserId.ToString()).Replace("[Language]", l.LanguageId);
                    //m.DynamicCase.FormPath += "&clearcache=1";
                    //m.DynamicCase.FormPath += "&apa=" + (DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                }
            }

            m.CaseTemplateTreeButton = GetCaseTemplateTreeModel(customerId, userId);
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

        private SendToDialogModel CreateNewSendToDialogModel(int customerId, IList<User> users)
        {
            var emailGroups = _emailGroupService.GetEmailGroupsWithEmails(customerId);
            var workingGroups = _workingGroupService.GetWorkingGroupsWithEmails(customerId);
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
        private Enums.AccessMode EditMode(CaseInputViewModel m, string topic, IList<Department> departmensForUser, List<CustomerWorkingGroupForUser> accessToWorkinggroups)
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

            if (accessToWorkinggroups != null)
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

            if (m.CaseIsLockedByUserId > 0)
            {
                return Enums.AccessMode.ReadOnly;
            }

            if (SessionFacade.CurrentUser.UserGroupId < 2)
            {
                return Enums.AccessMode.ReadOnly;
            }

            if (topic == ModuleName.Log)
            {
                if (SessionFacade.CurrentUser.UserGroupId == 2)
                {
                    if (SessionFacade.CurrentUser.Id != m.CaseLog.UserId)
                    {
                        return Enums.AccessMode.ReadOnly;
                    }                                    
                }
            }

            return Enums.AccessMode.FullAccess;
        }

        private CaseSettingModel GetCaseSettingModel(int customerId, int userId)
        {
            var ret = new CaseSettingModel();

            ret.CustomerId = customerId;
            ret.UserId = userId;

            var userCaseSettings = _customerUserService.GetUserCaseSettings(customerId, userId);

            var regions = _regionService.GetRegions(customerId);
            ret.RegionCheck = (userCaseSettings.Region != string.Empty);
            ret.Regions = regions;
            ret.SelectedRegion = userCaseSettings.Region;

            var registeredBys = _userService.GetUsers(customerId);
            ret.RegisteredByCheck = (userCaseSettings.RegisteredBy != string.Empty);
            ret.RegisteredBy = registeredBys;
            ret.SelectedRegisteredBy = userCaseSettings.RegisteredBy;

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

            ret.ProductAreas = this._productAreaService.GetTopProductAreas(customerId);
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
                _workingGroupService.GetWorkingGroups(customerId).Where(w => userWorkingGroup.Contains(w.Id)).ToList();
            ret.WorkingGroupCheck = (userCaseSettings.WorkingGroup != string.Empty);
            ret.WorkingGroups = workingGroups;
            ret.SelectedWorkingGroup = userCaseSettings.WorkingGroup;

            ret.ResponsibleCheck = userCaseSettings.Responsible;

            var administrators = _userService.GetAdministrators(customerId);
            ret.AdministratorCheck = true;
            ret.Administrators = administrators;
            ret.SelectedAdministrator = userCaseSettings.Administrators;

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
              
        private CaseType TranslateCaseType(CaseType caseType)
        {
            if (caseType.ParentCaseType != null)
                caseType.ParentCaseType = TranslateCaseType(caseType.ParentCaseType);

            caseType.Name = Translation.Get(caseType.Name);

            return caseType;
        }

        private IList<CaseSearchResult> TreeTranslate(IList<CaseSearchResult> cases, int customerId)
        {
            var ret = cases;
            foreach (CaseSearchResult r in ret)
            {
                foreach (var c in r.Columns)
                {
                    if (c.TreeTranslation)
                    {
                        switch (c.Key.ToLower())
                        {
                            case "productarea_id":
                                var p = _productAreaService.GetProductArea(c.Id);
                                if (p != null)
                                {
                                    var names = this._productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.Get(name));
                                    c.StringValue = string.Join(" - ", names);
                                }
                                break;
                        }
                    }
                }
            }

            return ret;
        }

        #endregion
    }
}
