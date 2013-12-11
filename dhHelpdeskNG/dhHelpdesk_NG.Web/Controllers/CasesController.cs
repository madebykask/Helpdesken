using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;
using dhHelpdesk_NG.DTO.Utils;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Infrastructure.Extensions; 
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
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
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IProjectService _projectService;
        private readonly IChangeService _changeService;
        private readonly ILogService _logService;

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
            IImpactService impactService,
            IOUService ouService,
            IProblemService problemService,
            IPriorityService priorityService,
            IProductAreaService productAreaService,
            IRegionService regionService,
            ISettingService settingService,
            IStateSecondaryService stateSecondaryService,
            IStatusService statusService,
            ISupplierService supplierService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IProjectService projectService,
            IChangeService changeService,
            ILogService logService)
            : base(masterDataService)
        {
            _caseService = caseService;
            _caseSearchService = caseSearchService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseFileService = caseFileService;
            _caseSettingService = caseSettingService;
            _caseTypeService = caseTypeService;
            _categoryService = categoryService;
            _computerService = computerService;
            _countryService = countryService;
            _currencyService = currencyService;
            _customerService = customerService;
            _customerUserService = customerUserService;
            _departmentService = departmentService;
            _impactService = impactService; 
            _ouService = ouService;
            _problemService = problemService; 
            _priorityService = priorityService;
            _productAreaService = productAreaService;
            _regionService = regionService;
            _settingService = settingService;
            _stateSecondaryService = stateSecondaryService;
            _statusService = statusService;
            _supplierService = supplierService;
            _systemService = systemService;
            _urgencyService = urgencyService; 
            _userService = userService;
            _workingGroupService = workingGroupService;
            _projectService = projectService;
            _changeService = changeService;
            _logService = logService;
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index(int customerId)
        {
            CaseIndexViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cu = _customerUserService.GetCustomerSettings(customerId, userId);

                // användern får bara se ärenden på kunder som de har behörighet till
                if (cu != null)
                {
                    m = new CaseIndexViewModel();
                    var fd = new CaseSearchFilterData();
                    var srm = new CaseSearchResultModel();

                    fd.customerUserSetting = cu;
                    fd.customerSetting = _settingService.GetCustomerSetting(customerId);   
                    fd.filterCustomerId = customerId;

                    //region
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter)) 
                        fd.filterRegion = _regionService.GetRegions(customerId);
                    //land
                    if (fd.customerSetting.DepartmentFilterFormat == 1) 
                        fd.filterCountry = _countryService.GetCountries(customerId);

                    //avdelningar per användare, är den tom så visa alla som kopplade till kunden
                    fd.filterDepartment = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);
                    if (fd.filterDepartment == null)
                        fd.filterDepartment = _departmentService.GetDepartments(customerId);
                    else if (fd.filterDepartment.Count == 0)
                        fd.filterDepartment = _departmentService.GetDepartments(customerId);

                    //användare
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseUserFilter)) 
                        fd.filterCaseUser = _userService.GetUserOnCases(customerId);
                    //ansvarig
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseResponsibleFilter)) 
                        fd.filterUser = _userService.GetUsers(customerId);
                    //utförare
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePerformerFilter))
                    {
                        fd.filterPerformer = _userService.GetUsers(customerId);
                        // visa även ej tilldelade
                        if (SessionFacade.CurrentUser.UserGroup_Id == 1 || SessionFacade.CurrentUser.RestrictedCasePermission == 0)
                            fd.filterPerformer.Insert(0, ObjectExtensions.notAssignedPerformer());
                    }
                    //ärendetyp
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter)) 
                        fd.filterCaseType = _caseTypeService.GetCaseTypes(customerId);
                    //working group
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseWorkingGroupFilter))
                    {
                        fd.filterWorkingGroup = _workingGroupService.GetWorkingGroups(customerId);
                        // visa även ej tilldelade
                        if (SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups == 1)
                            fd.filterWorkingGroup.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
                    }
                    //produktonmråde
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseProductAreaFilter)) 
                        fd.filterProductArea = _productAreaService.GetProductAreas(customerId);
                    //kategori                        
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCategoryFilter)) 
                        fd.filterCategory = _categoryService.GetCategories(customerId);
                    //prio
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePriorityFilter)) 
                        fd.filterPriority = _priorityService.GetPriorities(customerId) ;
                    //status
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter)) 
                        fd.filterStatus = _statusService.GetStatuses(customerId);
                    //understatus
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter)) 
                        fd.filterStateSecondary = _stateSecondaryService.GetStateSecondaries(customerId);
                    fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser, fd.filterPriority, customerId);    

                    var sm = GetCaseSearchModel(customerId, userId);

                    // hämta parent path för productArea 
                    sm.caseSearchFilter.ParantPath_ProductArea = "--";
                    sm.caseSearchFilter.ParantPath_CaseType = "--";
                    if (!string.IsNullOrWhiteSpace(sm.caseSearchFilter.ProductArea))  
                    {
                        if (sm.caseSearchFilter.ProductArea != "0")
                        {
                            var p = _productAreaService.GetProductArea(sm.caseSearchFilter.ProductArea.convertStringToInt());
                            if (p != null)
                                sm.caseSearchFilter.ParantPath_ProductArea = p.getProductAreaParentPath();
                        }
                    }
                    // hämta parent path för casetype
                    if ((sm.caseSearchFilter.CaseType > 0))
                    {
                        var c = _caseTypeService.GetCaseType(sm.caseSearchFilter.CaseType);   
                        if (c != null)
                            sm.caseSearchFilter.ParantPath_CaseType = c.getCaseTypeParentPath();
                    }

                    fd.caseSearchFilter = sm.caseSearchFilter;
                    srm.caseSettings = _caseSettingService.GetCaseSettingsWithUser(customerId, SessionFacade.CurrentUser.Id);
                    srm.cases = _caseSearchService.Search(sm.caseSearchFilter, srm.caseSettings, SessionFacade.CurrentUser, sm.Search);
                    m.caseSearchResult = srm;
                    m.caseSearchFilterData = fd;
                    SessionFacade.CurrentCaseSearch = sm; 
                }
            }

            return View(m);
        }

        public ActionResult New(int customerId)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                m = GetCaseInputViewModel(userId, customerId, 0);   
            }

            return View(m);
        }

        [HttpPost]
        public RedirectToRouteResult New(Case case_)
        {
            IDictionary<string, string> errors;

            _caseService.SaveCase(case_, out errors);

            if (errors.Count == 0)
                return RedirectToAction("edit", "cases", new { case_.Id });

            return RedirectToAction("new", "cases", new { customerId = case_.Customer_Id });
        }

        public ActionResult Edit(int id)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                m = GetCaseInputViewModel(userId, 0, id);
            }

            return View(m);
        }

        [HttpPost]
        public RedirectToRouteResult Edit(Case case_)
        {
            IDictionary<string, string> errors;
            _caseService.SaveCase(case_, out errors);
            return RedirectToAction("edit", "cases", new { case_.Id });
        }

        [HttpPost]
        public ActionResult Search_User(string query, int customerId)
        {
            var result = _computerService.SearchComputerUsers(customerId, query);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Search_Computer(string query, int customerId)
        {
            var result = _computerService.SearchComputer(customerId, query);
            return Json(result);
        }

        public JsonResult ChangeRegion(int? id, int customerId, int departmentFilterFormat)
        {
            var dep = _departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId) ?? _departmentService.GetDepartments(customerId);

            if (id.HasValue)
                dep = dep.Where(x => x.Region_Id == id).ToList();

            var list = dep.Select(x => new {id = x.Id, name = x.DepartmentDescription(departmentFilterFormat)});

            return Json(new { list });
        }

        public JsonResult ChangeDepartment(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? _ouService.GetOuForDepartment(id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : _ouService.GetOUs(customerId).Select(x => new { id = x.Id, name = x.Name });  
            return Json(new { list });
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? _supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : _supplierService.GetSuppliers(customerId).Select(x => new { id = x.Id, name = x.Name });  
            return Json(new { list });
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
                f.UserResponsible= frm.ReturnFormValue("lstFilterResponsible");
                f.Priority = frm.ReturnFormValue("lstFilterPriority");
                f.Status  = frm.ReturnFormValue("lstFilterStatus");
                f.StateSecondary = frm.ReturnFormValue("lstFilterStateSecondary");
                f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");
                f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");

                var sm = GetCaseSearchModel(f.CustomerId, f.UserId);
                sm.caseSearchFilter = f;
                sm.Search.SortBy = frm.ReturnFormValue("hidSortBy");
                sm.Search.Ascending = frm.ReturnFormValue("hidSortByAsc").convertStringToBool(); 

                m.caseSettings = _caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id);
                m.cases = _caseSearchService.Search(f, m.caseSettings, SessionFacade.CurrentUser, sm.Search);

                SessionFacade.CurrentCaseSearch = sm; 
            }

            return PartialView("_CaseRows", m);
        }

        #endregion

        #region Private Methods and Operators

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

                var cu = _customerUserService.GetCustomerSettings(customerId, userId);

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
                s.SortBy = "CaseNumber";
                s.Ascending = true; 

                m.caseSearchFilter = f;
                m.Search = s;
            }
            else
                m = SessionFacade.CurrentCaseSearch;

            return m;
        }

        private CaseInputViewModel GetCaseInputViewModel(int userId, int customerId, int caseId)
        {
            var m = new CaseInputViewModel();
            
            if (caseId != 0)
            {
                m.case_ = _caseService.GetCaseById(caseId);
                customerId = m.case_.Customer_Id;
            }

            // get departments and workinggroup for the user TODO workinggroups 
            var deps = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);
            
            var cu = _customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
                // user can only see cases on their custmomers     
                m.case_ = null;
            else
            {
                var cs = _settingService.GetCustomerSetting(customerId);
                m.customerUserSetting = cu;
                m.caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
                m.DepartmentFilterFormat = cs.DepartmentFilterFormat;
                m.ParantPath_CaseType = "--";
                m.ParantPath_ProductArea = "--";

                if (caseId == 0)
                    m.case_ = _caseService.InitCase(customerId, userId, SessionFacade.CurrentLanguage, Request.GetIpAddress(), GlobalEnums.RegistrationSource.Case, cs);
                else
                {
                    // hämta parent path för productArea 
                    if ((m.case_.ProductArea_Id.HasValue))
                    {
                        var p = _productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                        if (p != null)
                            m.ParantPath_ProductArea = p.getProductAreaParentPath();
                    }
                    // hämta parent path för casetype
                    if ((m.case_.CaseType_Id > 0))
                    {
                        var c = _caseTypeService.GetCaseType(m.case_.CaseType_Id);
                        if (c != null)
                            m.ParantPath_CaseType = c.getCaseTypeParentPath();
                    }
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1) 
                    m.caseTypes = _caseTypeService.GetCaseTypes(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1) 
                    m.categories = _categoryService.GetCategories(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1) 
                    m.impacts = _impactService.GetImpacts(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1) 
                    m.ous = _ouService.GetOUs(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1) 
                    m.priorities = _priorityService.GetPriorities(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1) 
                    m.productAreas = _productAreaService.GetProductAreas(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1) 
                    m.regions = _regionService.GetRegions(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1) 
                    m.statuses = _statusService.GetStatuses(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1) 
                    m.stateSecondaries = _stateSecondaryService.GetStateSecondaries(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.suppliers = _supplierService.GetSuppliers(customerId);
                    m.countries = _countryService.GetCountries(customerId);
                }
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.System_Id.ToString()).ShowOnStartPage == 1)
                    m.systems = _systemService.GetSystems(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Urgency_Id.ToString()).ShowOnStartPage == 1)
                    m.urgencies = _urgencyService.GetUrgencies(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
                    m.workingGroups = _workingGroupService.GetWorkingGroups(customerId);
                if (cs.ModuleProject == 1)
                    m.projects = _projectService.GetProjects(customerId);
                if (cs.ModuleChangeManagement == 1)
                    m.changes = _changeService.GetChanges(customerId);

                m.problems = _problemService.GetProblems(customerId);
                m.currencies = _currencyService.GetCurrencies();
                m.users = _userService.GetUsers(customerId);
                m.projects = _projectService.GetProjects(customerId);
                m.departments = deps ?? _departmentService.GetDepartments(customerId);

                if (m.case_.Supplier_Id > 0 && m.suppliers != null)
                {
                    var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                    m.CountryId = sup.Country_Id.GetValueOrDefault();
                }

            }

            return m;
        }

        //private bool AllowUserAccessToCase(IList<Department> departmentsForUser, Case case_)
        //{
        //    var ret = true;

        //    if (case_.Department_Id.HasValue && departmentsForUser != null)
        //    {
        //        ret = false;
        //        var d = departmentsForUser.Where(x => x.Id == case_.Department_Id);
        //    }
            
        //    return ret;
        //}

        #endregion

    }
}
