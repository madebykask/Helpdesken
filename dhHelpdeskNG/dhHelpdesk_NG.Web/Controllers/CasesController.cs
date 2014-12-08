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

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Utils;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Shared;
    using System.Web.Script.Serialization;

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
            IInvoiceHelper invoiceHelper)
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
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index(int? customerId, bool? clearFilters = false, string customFilter = "")
        {                        
            if (clearFilters == true)
            {
                SessionFacade.CurrentCaseSearch = null;                
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, "");

            CaseIndexViewModel m = null;
            //c => c.FinishingDate == null && c.Performer_User_Id == userId
            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cusId = customerId.HasValue ? customerId.Value : SessionFacade.CurrentCustomer.Id;
                var cu = this._customerUserService.GetCustomerSettings(cusId, userId);

                // update session info
                if (SessionFacade.CurrentCustomer == null)
                    SessionFacade.CurrentCustomer = this._customerService.GetCustomer(cusId);
                else if (SessionFacade.CurrentCustomer.Id != cusId)
                    SessionFacade.CurrentCustomer = this._customerService.GetCustomer(cusId);

                // användern får bara se ärenden på kunder som de har behörighet till
                if (cu != null)
                {
                    m = new CaseIndexViewModel();
                    var fd = new CaseSearchFilterData();
                    fd.IsClearFilters = clearFilters == true;
                    var srm = new CaseSearchResultModel();

                    fd.customerUserSetting = cu;
                    fd.customerSetting = this._settingService.GetCustomerSetting(cusId);
                    fd.filterCustomerId = cusId;
                    

                    //region
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter))
                        fd.filterRegion = this._regionService.GetRegions(cusId);
                    //land
                    if (fd.customerSetting.DepartmentFilterFormat == 1)
                        fd.filterCountry = this._countryService.GetCountries(cusId);

                    //avdelningar per användare, är den tom så visa alla som kopplade till kunden
                    fd.filterDepartment = this._departmentService.GetDepartmentsByUserPermissions(userId, cusId);
                    if (fd.filterDepartment == null)
                        fd.filterDepartment = this._departmentService.GetDepartments(cusId);
                    else if (fd.filterDepartment.Count == 0)
                        fd.filterDepartment = this._departmentService.GetDepartments(cusId);

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
                        fd.filterProductArea = this._productAreaService.GetProductAreas(cusId);
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

                    var sm = this.GetCaseSearchModel(cusId, userId);

                    // hämta parent path för productArea 
                    sm.caseSearchFilter.ParantPath_ProductArea = ParentPathDefaultValue;
                    sm.caseSearchFilter.ParentPathClosingReason = ParentPathDefaultValue;
                    sm.caseSearchFilter.ParantPath_CaseType = ParentPathDefaultValue;
                    if (!string.IsNullOrWhiteSpace(sm.caseSearchFilter.ProductArea))
                    {
                        if (sm.caseSearchFilter.ProductArea != "0")
                        {
                            var p = this._productAreaService.GetProductArea(sm.caseSearchFilter.ProductArea.convertStringToInt());
                            if (p != null)
                                sm.caseSearchFilter.ParantPath_ProductArea = p.getProductAreaParentPath();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(sm.caseSearchFilter.CaseClosingReasonFilter))
                    {
                        if (sm.caseSearchFilter.CaseClosingReasonFilter != "0")
                        {
                            var fc = this._finishingCauseService.GetFinishingCause(sm.caseSearchFilter.CaseClosingReasonFilter.convertStringToInt());
                            if (fc != null)
                            {
                                sm.caseSearchFilter.ParentPathClosingReason = fc.GetFinishingCauseParentPath();
                            }
                        }
                    }

                    // hämta parent path för casetype
                    if ((sm.caseSearchFilter.CaseType > 0))
                    {
                        var c = this._caseTypeService.GetCaseType(sm.caseSearchFilter.CaseType);
                        if (c != null)
                            sm.caseSearchFilter.ParantPath_CaseType = c.getCaseTypeParentPath();
                    }

                    

                    switch (customFilter)
                    {
                        case "MyCases":
                            sm.caseSearchFilter.UserPerformer = SessionFacade.CurrentUser.Id.ToString();                            
                            sm.caseSearchFilter.CaseProgress = "2";                            
                            break;                        
                        case "UnreadCases":
                            sm.caseSearchFilter.CaseProgress = "4";
                            sm.caseSearchFilter.UserPerformer = "";                            
                            break;
                        case "HoldCases":                            
                            sm.caseSearchFilter.CaseProgress = "3";
                            sm.caseSearchFilter.UserPerformer = "";                            
                            break;
                        case "InProcessCases":                            
                            sm.caseSearchFilter.CaseProgress = "2";
                            sm.caseSearchFilter.UserPerformer = "";                           
                            break;                        
                    }

                    fd.caseSearchFilter = sm.caseSearchFilter;
                    fd.CaseClosingDateEndFilter = sm.caseSearchFilter.CaseClosingDateEndFilter;
                    fd.CaseClosingDateStartFilter = sm.caseSearchFilter.CaseClosingDateStartFilter;
                    fd.CaseRegistrationDateEndFilter = sm.caseSearchFilter.CaseRegistrationDateEndFilter;
                    fd.CaseRegistrationDateStartFilter = sm.caseSearchFilter.CaseRegistrationDateStartFilter;
                    fd.CaseWatchDateEndFilter = sm.caseSearchFilter.CaseWatchDateEndFilter;
                    fd.CaseWatchDateStartFilter = sm.caseSearchFilter.CaseWatchDateStartFilter;
                    
                    srm.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(cusId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
                    srm.cases = this._caseSearchService.Search(
                        sm.caseSearchFilter,
                        srm.caseSettings,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserId,
                        SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.RestrictedCasePermission,
                        sm.Search,
                        this.workContext.Customer.WorkingDayStart,
                        this.workContext.Customer.WorkingDayEnd,
                        this.workContext.Cache.Holidays,
                        this.configuration.Application.ApplicationId);
                    m.caseSearchResult = srm;
                    m.caseSearchFilterData = fd;
                    sm.Search.IdsForLastSearch = GetIdsFromSearchResult(srm.cases);

                    SessionFacade.CurrentCaseSearch = sm;

                    var caseTemplateTree = GetCaseTemplateTreeModel(cusId, userId);
                    m.CaseTemplateTreeButton = caseTemplateTree;
                    m.CaseSetting = GetCaseSettingModel(cusId, userId);
                }
            }

            return this.View(m);
        }

        public ActionResult New(int customerId, int? templateId, int? copyFromCaseId, int? caseLanguageId)
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
                    m = this.GetCaseInputViewModel(userId, customerId, 0, 0, "", templateId, copyFromCaseId);

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
                                    string caseInvoiceArticles)
        {
            int caseId = this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = updateNotifierInformation });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndClose(
                                    Case case_, 
                                    CaseLog caseLog, 
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles)
        {
            this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
            return this.RedirectToAction("index", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndAddCase(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles)
        {
            this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
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
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = updateNotifierInformation });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditAndClose(
                                    Case case_,
                                    CaseLog caseLog,
                                    CaseMailSetting caseMailSetting,
                                    bool? updateNotifierInformation,
                                    string caseInvoiceArticles)
        {
            this.Save(case_, caseLog, caseMailSetting, updateNotifierInformation, caseInvoiceArticles);
            return this.RedirectToAction("index", "cases", new { customerId = case_.Customer_Id });
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

        public ActionResult Edit(int id, string redirectFrom = "", int? moveToCustomerId = null, bool? uni = null)
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

                int customerId = moveToCustomerId.HasValue ? moveToCustomerId.Value : 0;
                m = this.GetCaseInputViewModel(userId, customerId, id, lockedByUserId, redirectFrom, null, null);
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
                    m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, customer.Language_Id);
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
                num = cu.UserId
                ,
                name = cu.SurName + ' ' + cu.FirstName
                ,
                email = cu.Email
                ,
                place = cu.Location
                ,
                phone = cu.Phone
                ,
                usercode = cu.UserCode
                ,
                cellphone = cu.Cellphone
                ,
                regionid = (cu.Department != null ? cu.Department.Region_Id : 0)
                ,
                departmentid = cu.Department_Id
                ,
                ouid = cu.OU_Id
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
            var dep = this._departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId) ?? this._departmentService.GetDepartments(customerId);

            if (id.HasValue)
                dep = dep.Where(x => x.Region_Id == id).ToList();

            var list = dep.Select(x => new { id = x.Id, name = x.DepartmentDescription(departmentFilterFormat) });

            return this.Json(new { list });
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._userService.GetUsersForWorkingGroup(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName }) : this._userService.GetUsers(customerId).Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName });
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
            var list = id.HasValue ? this._ouService.GetOuForDepartment(id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._ouService.GetOUs(customerId).Select(x => new { id = x.Id, name = x.Name });
            return this.Json(new { list });
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

            if (id.HasValue)
            {
                var e = _productAreaService.GetProductArea(id.Value);
                if (e != null)
                {
                    workinggroupId = e.WorkingGroup_Id.HasValue ? e.WorkingGroup_Id.Value : 0;
                    priorityId = e.Priority_Id.HasValue ? e.Priority_Id.Value : 0;
                }
            }
            return Json(new { WorkingGroup_Id = workinggroupId, Priority_Id = priorityId });
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

        public ActionResult Search(FormCollection frm)
        {
            var f = new CaseSearchFilter();
            var m = new CaseSearchResultModel();

            if (SessionFacade.CurrentUser != null)
            {
                f.CustomerId = frm.ReturnFormValue("hidFilterCustomerId").convertStringToInt();
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

                var sm = this.GetCaseSearchModel(f.CustomerId, f.UserId);
                sm.caseSearchFilter = f;
                sm.Search.SortBy = frm.ReturnFormValue("hidSortBy");
                sm.Search.Ascending = frm.ReturnFormValue("hidSortByAsc").convertStringToBool();

                m.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
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
                    this.workContext.Cache.Holidays,
                    this.configuration.Application.ApplicationId);

                sm.Search.IdsForLastSearch = GetIdsFromSearchResult(m.cases);
                SessionFacade.CurrentCaseSearch = sm;
            }

            return this.PartialView("_CaseRows", m);
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

        public RedirectToRouteResult Activate(int id)
        {
            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (SessionFacade.CurrentUser != null)
                if (SessionFacade.CurrentUser.ActivateCasePermission == 1)
                    this._caseService.Activate(id, SessionFacade.CurrentUser.Id, adUser, out errors);

            return this.RedirectToAction("edit", "cases", new { id = id, redirectFrom = "save" });
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
                            caseTypeCheck,
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

        public void SaveColSetting(FormCollection frm)
        {

            // Update Rows one by one ordered as a showed 

            int customerId = int.Parse(frm["CustomerId"]);
            int userId = int.Parse(frm["UserId"]);

            if (frm["uc.Id"] != null)
            {
                var updatedId = frm["uc.Id"].Split(',');
                var updatedName = frm["uc.Name"].Split(',');
                var updatedRow = frm.ReturnFormValue("rows").Split(',');
                var updatedMinWith = frm["uc.MinWidth"].Split(',');
                var updatedColOrder = frm["uc.ColOrder"].Split(',');
                var updatedUserGroup = frm["uc.UserGroup"].Split(',');

                IDictionary<string, string> errors = new Dictionary<string, string>();
                DateTime nowTime = DateTime.Now;
                var newColSetting = new CaseSettings();
                for (int ii = 0; ii < updatedId.Length; ii++)
                {
                    newColSetting.Id = int.Parse(updatedId[ii]);
                    newColSetting.Customer_Id = customerId;
                    newColSetting.User_Id = userId;
                    newColSetting.Name = updatedName[ii];
                    newColSetting.Line = int.Parse(updatedRow[ii]);
                    newColSetting.MinWidth = int.Parse(updatedMinWith[ii]);
                    newColSetting.ColOrder = int.Parse(updatedColOrder[ii]);
                    newColSetting.UserGroup = int.Parse(updatedUserGroup[ii]);
                    newColSetting.ChangeTime = nowTime;

                    _caseSettingService.UpdateCaseSetting(newColSetting, out errors);
                }

            }
        }

        [HttpPost]
        public ActionResult AddCaseSettingColumn(int customerId, int userId, string labellist, int linelist, int minWidthValue, int colOrderValue, int userGroup)
        {

            IDictionary<string, string> errors = new Dictionary<string, string>();

            DateTime nowTime = DateTime.Now;

            var newCaseSetting = new CaseSettings();

            newCaseSetting.Id = 0;
            newCaseSetting.Customer_Id = customerId;
            newCaseSetting.User_Id = userId;
            newCaseSetting.Name = labellist;
            newCaseSetting.Line = linelist;
            newCaseSetting.MinWidth = minWidthValue;
            newCaseSetting.ColOrder = colOrderValue;
            newCaseSetting.UserGroup = userGroup;
            newCaseSetting.RegTime = nowTime;
            newCaseSetting.ChangeTime = nowTime;

            _caseSettingService.SaveCaseSetting(newCaseSetting, out errors);

            var model = new CaseColumnsSettingsModel();
            model = GetCaseColumnSettingModel(customerId, userId);

            return PartialView("_ColumnCaseSetting", model);

        }

        [HttpPost]
        public ActionResult DeleteRowFromCaseSettings(int id, int userId, int customerId)
        {
            if (this._caseSettingService.DeleteCaseSetting(id) != DeleteMessage.Success)
                this.TempData.Add("Error", "");

            var model = new CaseColumnsSettingsModel();

            model = GetCaseColumnSettingModel(customerId, userId);

            return PartialView("_ColumnCaseSetting", model);
        }

        [HttpGet]
        public RedirectToRouteResult ChangeCurrentLanguage(int languageId)        
        {
            SessionFacade.CurrentLanguageId = languageId;
            SessionFacade.CurrentUser.LanguageId = languageId;
            var prevInfo = this.ExtractPreviousRouteInfo();
            var res = new RedirectToRouteResult(prevInfo);
            return res;
        }

        #endregion

        #region Private Methods and Operators

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
                case_.LeadTime = CaseUtils.CalculateLeadTime(
                                                            case_.RegTime,
                                                            case_.WatchDate,
                                                            case_.FinishingDate,
                                                            case_.ExternalTime,
                                                            this.workContext.Customer.WorkingDayStart,
                                                            this.workContext.Customer.WorkingDayEnd,
                                                            this.workContext.Cache.Holidays);
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

            if (newSearch)
            {
                ISearch s = new Search();
                var f = new CaseSearchFilter();
                m = new CaseSearchModel();

                var cu = this._customerUserService.GetCustomerSettings(customerId, userId);

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
        private CaseInputViewModel GetCaseInputViewModel(int userId, int customerId, int caseId, int lockedByUserId = 0, string redirectFrom = "", int? templateId = null, int? copyFromCaseId = null)
        {
            var m = new CaseInputViewModel();
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            if (caseId != 0)
            {
                var markCaseAsRead = string.IsNullOrWhiteSpace(redirectFrom);
                m.case_ = this._caseService.GetCaseById(caseId, markCaseAsRead);
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
                m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, customer.Language_Id);
                m.DepartmentFilterFormat = cs.DepartmentFilterFormat;
                m.ParantPath_CaseType = ParentPathDefaultValue;
                m.ParantPath_ProductArea = ParentPathDefaultValue;
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

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.priorities = this._priorityService.GetPriorities(customerId);
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.productAreas = this._productAreaService.GetProductAreas(customerId);
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
                var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);
                m.departments = deps ?? this._departmentService.GetDepartments(customerId);
                m.standardTexts = this._standardTextService.GetStandardTexts(customerId);
                m.Languages = this._languageService.GetActiveLanguages();

                if (cs.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
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

                        if (caseTemplate.Category_Id != null)
                        {
                            m.case_.Category_Id = caseTemplate.Category_Id.Value;
                        }

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
                    }
                } // Load Case Template

                // related cases
                if (caseId != 0 && !string.IsNullOrWhiteSpace(m.case_.ReportedBy))
                {
                    m.RelatedCases = this._caseService.GetRelatedCases(caseId, customerId, m.case_.ReportedBy, SessionFacade.CurrentUser);
                }

                // hämta parent path för productArea 
                if (m.case_.ProductArea_Id.HasValue)
                {
                    var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                    if (p != null)
                    {
                        m.ParantPath_ProductArea = p.getProductAreaParentPath();
                    }
                }

                // hämta parent path för casetype
                if (m.case_.CaseType_Id > 0)
                {
                    var c = this._caseTypeService.GetCaseType(m.case_.CaseType_Id);
                    if (c != null)
                    {
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
                m.Disable_SendMailAboutCaseToNotifier = false;
                if (m.case_.StateSecondary_Id > 0)
                {
                    if (m.case_.StateSecondary != null)
                    {
                        m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                    }                    
                }

                var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
                m.EditMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);

                if (m.case_.Id == 0)  // new mode
                {
                    m.case_.DefaultOwnerWG_Id = null;
                    if (m.case_.User_Id != 0)
                    {
                        var curUser = _userService.GetUser(m.case_.User_Id);
                        if (curUser.Default_WorkingGroup_Id != null)
                           m.case_.DefaultOwnerWG_Id = curUser.Default_WorkingGroup_Id;                        
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

            ret.CaseTypeCheck = userCaseSettings.CaseType;

            ret.ProductAreas = this._productAreaService.GetProductAreas(customerId);
            ret.ProductAreaPath = "--";
         
            int pa;
            int.TryParse(userCaseSettings.ProductArea, out pa);
            ret.ProductAreaId = pa;

            if (pa > 0)
            {

                var p = this._productAreaService.GetProductArea(ret.ProductAreaId);
                if (p != null)
                    ret.ProductAreaPath = p.getProductAreaParentPath();
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
            ret.ColumnSettingModel = this.GetCaseColumnSettingModel(customerId, userId);

            return ret;
        }

        private CaseColumnsSettingsModel GetCaseColumnSettingModel(int customerId, int userId)
        {
            CaseColumnsSettingsModel colSettingModel = new CaseColumnsSettingsModel();

            colSettingModel.CustomerId = customerId;
            colSettingModel.UserId = userId;

            var showColumns = _caseFieldSettingService.ListToShowOnCasePage(customerId, SessionFacade.CurrentUser.LanguageId)
                                       .Where(c => c.ShowOnStartPage == 1)
                                       .Select(s => s.CFS_Id)
                                       .ToList();

            IList<CaseFieldSettingsWithLanguage> allColumns = new List<CaseFieldSettingsWithLanguage>();
            allColumns = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentUser.LanguageId)
                                                 .Where(c => showColumns.Contains(c.Id))
                                                 .ToList();

            //<option value="tblProblem.ResponsibleUser_Id">@Translation.Get("Problem", Enums.TranslationSource.TextTranslation)</option>

            var fixValue1 = new CaseFieldSettingsWithLanguage
            {
                Id = 9998,
                Label = Translation.Get("Tid kvar", Enums.TranslationSource.TextTranslation),
                Name = "_temporary_.LeadTime",
                Language_Id = SessionFacade.CurrentUser.LanguageId
            };
            allColumns.Add(fixValue1);

            var fixValue2 = new CaseFieldSettingsWithLanguage
            {
                Id = 9999,
                Label = Translation.Get("Problem", Enums.TranslationSource.TextTranslation),
                Name = "tblProblem.ResponsibleUser_Id",
                Language_Id = SessionFacade.CurrentUser.LanguageId
            };

            allColumns.Add(fixValue2);
            colSettingModel.CaseFieldSettingLanguages = allColumns;


            IList<CaseSettings> userColumns = new List<CaseSettings>();
            userColumns = _caseSettingService.GetCaseSettingsWithUser(customerId, userId, SessionFacade.CurrentUser.UserGroupId);

            IList<CaseFieldSetting> userCaseFieldSettings = new List<CaseFieldSetting>();
            userCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            colSettingModel.UserColumns = userColumns;
            colSettingModel.CaseFieldSettings = userCaseFieldSettings;

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Info", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            //li.Add(new SelectListItem()
            //{
            //    Text = Translation.Get("Utökad info", Enums.TranslationSource.TextTranslation),
            //    Value = "2",
            //    Selected = false
            //});

            colSettingModel.LineList = li;

            return colSettingModel;

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
        #endregion

    }
}
