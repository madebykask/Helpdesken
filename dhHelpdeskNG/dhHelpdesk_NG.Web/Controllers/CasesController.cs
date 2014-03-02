using Ninject;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete; 
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Common;

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
        private readonly IUserTemporaryFilesStorage userTemporaryFilesStorage;
        private readonly IEmailGroupService _emailGroupService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IEmailService _emailService;

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
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ICaseSolutionService caseSolutionService,
            ILogService logService,
            IEmailGroupService emailGroupService,
            IEmailService emailService,
            ILogFileService logFileService)
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
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create(TopicName.Cases);
            this._caseSolutionService = caseSolutionService;
            this._emailGroupService = emailGroupService;
            this._emailService = emailService; 
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index(int? customerId)
        {
            CaseIndexViewModel m = null;

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
                        fd.filterPriority = this._priorityService.GetPriorities(cusId) ;
                    //status
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter)) 
                        fd.filterStatus = this._statusService.GetStatuses(cusId);
                    //understatus
                    if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter)) 
                        fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);
                    fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser.FollowUpPermission, fd.filterPriority, cusId);    

                    var sm = this.GetCaseSearchModel(cusId, userId);

                    // hämta parent path för productArea 
                    sm.caseSearchFilter.ParantPath_ProductArea = "--";
                    sm.caseSearchFilter.ParantPath_CaseType = "--";
                    if (!string.IsNullOrWhiteSpace(sm.caseSearchFilter.ProductArea))  
                    {
                        if (sm.caseSearchFilter.ProductArea != "0")
                        {
                            var p = this._productAreaService.GetProductArea(sm.caseSearchFilter.ProductArea.convertStringToInt());
                            if (p != null)
                                sm.caseSearchFilter.ParantPath_ProductArea = p.getProductAreaParentPath();
                        }
                    }
                    // hämta parent path för casetype
                    if ((sm.caseSearchFilter.CaseType > 0))
                    {
                        var c = this._caseTypeService.GetCaseType(sm.caseSearchFilter.CaseType);   
                        if (c != null)
                            sm.caseSearchFilter.ParantPath_CaseType = c.getCaseTypeParentPath();
                    }

                    fd.caseSearchFilter = sm.caseSearchFilter;
                    srm.caseSettings = this._caseSettingService.GetCaseSettingsWithUser(cusId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
                    srm.cases = this._caseSearchService.Search(
                        sm.caseSearchFilter,
                        srm.caseSettings,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserId,
                        SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.RestrictedCasePermission,
                        sm.Search);
                    m.caseSearchResult = srm;
                    m.caseSearchFilterData = fd;
                    SessionFacade.CurrentCaseSearch = sm;
                    
                    var caseTemplateTree = GetCaseTemplateTreeModel(cusId, SessionFacade.CurrentUser.Id);
                    m.CaseTemplateTreeButton = caseTemplateTree;
                }
            }

            return this.View(m);
        }

        public ActionResult New(int customerId)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                m = this.GetCaseInputViewModel(userId, customerId, 0);   
            }
            AddViewDataValues();
            return this.View(m);
        }

        [HttpPost]
        public RedirectToRouteResult New(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save" });
        }

        [HttpPost]
        public RedirectToRouteResult NewAndClose(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting); 
            return this.RedirectToAction("index", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        public RedirectToRouteResult NewAndAddCase(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting); 
            return this.RedirectToAction("new", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        public RedirectToRouteResult Edit(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting); 
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save" });
        }

        [HttpPost]
        public RedirectToRouteResult EditAndClose(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting); 
            return this.RedirectToAction("index", "cases", new { customerId = case_.Customer_Id });
        }

        [HttpPost]
        public RedirectToRouteResult EditAndAddCase(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            int caseId = Save(case_, caseLog, caseMailSetting); 
            return this.RedirectToAction("new", "cases", new { customerId = case_.Customer_Id });
        }

        public ActionResult Edit(int id, string redirectFrom = "")
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

                m = this.GetCaseInputViewModel(userId, 0, id, lockedByUserId, redirectFrom);
            }
            AddViewDataValues();
            return this.View(m);
        }

        public ActionResult EditLog(int id, int customerId)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cu = this._customerUserService.GetCustomerSettings(customerId, userId);

                if (cu != null)
                {
                    m = new CaseInputViewModel();
                    m.CaseLog = this._logService.GetLogById(id);
                    m.customerUserSetting = cu;
                    m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                    m.finishingCauses = this._finishingCauseService.GetFinishingCauses(customerId);  
                    m.case_ = this._caseService.GetCaseById(m.CaseLog.CaseId);
                    m.users = this._userService.GetUsers(customerId);   
                    m.LogFilesModel = new FilesModel(id.ToString(), this._logFileService.FindFileNamesByLogId(id));
                    m.SendToDialogModel = CreateNewSendToDialogModel(customerId, m.users);
                    m.ShowInvoiceFields = 0;
                    if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)    
                    {
                        var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                        if (d != null)
                            m.ShowInvoiceFields = d.Charge;

                    }
                    m.EditMode = EditMode(m, TopicName.Log);
                    AddViewDataValues();
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
                caseHistoryId = this._caseService.SaveCase(c, caseLog, null,SessionFacade.CurrentUser.Id, this.User.Identity.Name, out errors);
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

            var list = dep.Select(x => new {id = x.Id, name = x.DepartmentDescription(departmentFilterFormat)});

            return this.Json(new { list });
        }

        public JsonResult ChangeWorkingGroup(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._userService.GetUsersForWorkingGroup(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName }) : this._userService.GetUsers(customerId).Select(x => new { id = x.Id, name = x.SurName + ' ' + x.FirstName });  
            return this.Json(new { list });
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
            var list = id.HasValue ? this._ouService.GetOuForDepartment(id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name}) : this._ouService.GetOUs(customerId).Select(x => new { id = x.Id, name = x.Name});  
            return this.Json(new { list });
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._supplierService.GetSuppliers(customerId).Select(x => new { id = x.Id, name = x.Name });  
            return this.Json(new { list });
        }

        public JsonResult ChangePriority(int? id, int customerId)
        {
            //TODO beroende på inställning på prio ska External log text fyllas i 
            return this.Json(new { string.Empty });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, id, TopicName.Cases)
                                  : this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);
            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadLogFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, id, TopicName.Log)
                                  : this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ActionResult Files(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.GetFileNames(id, TopicName.Cases)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            var model = new FilesModel(id, files);
            return this.PartialView("_CaseFiles", model);
        }

        [HttpGet]
        public ActionResult LogFiles(string id)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this.userTemporaryFilesStorage.GetFileNames(id, TopicName.Log)
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
                if (this.userTemporaryFilesStorage.FileExists(name, id, TopicName.Cases))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, TopicName.Cases);
            }
            else
            {
                if (this._caseFileService.FileExists(int.Parse(id), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                var caseFileDto = new CaseFileDto(uploadedData, name, DateTime.Now, int.Parse(id));
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
                if (this.userTemporaryFilesStorage.FileExists(name, id, TopicName.Log))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, TopicName.Log);
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
                f.UserResponsible= frm.ReturnFormValue("lstFilterResponsible");
                f.Priority = frm.ReturnFormValue("lstFilterPriority");
                f.Status  = frm.ReturnFormValue("lstFilterStatus");
                f.StateSecondary = frm.ReturnFormValue("lstFilterStateSecondary");
                f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");
                f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");

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
                    sm.Search);

                SessionFacade.CurrentCaseSearch = sm; 
            }

            return this.PartialView("_CaseRows", m);
        }

        [HttpPost]
        public void DeleteCaseFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this.userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, TopicName.Cases);
            else
                this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), fileName.Trim());  
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this.userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, TopicName.Log);
            else
                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), fileName.Trim());
        }

        [HttpPost]
        public RedirectToRouteResult DeleteCase(int caseId, int customerId)
        {
            var caseGuid = this._caseService.Delete(caseId);
            // TODO kolla om temp filer tas bort
            this.userTemporaryFilesStorage.DeleteFiles(caseGuid.ToString());
            return this.RedirectToAction("index", "cases", new { customerId = customerId });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            //TODO fungerar delete, kolla funktionen, får null ref fel
            //var logGuid = this._logService.Delete(id);  
            //this.userTemporaryFilesStorage.DeleteFiles(logGuid.ToString());
            return this.RedirectToAction("edit", "cases", new { id = caseId });
        }

        #endregion

        #region Private Methods and Operators

        private int Save(Case case_, CaseLog caseLog, CaseMailSetting caseMailSetting)
        {
            bool edit = case_.Id == 0 ? false : true; 
            IDictionary<string, string> errors;

            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(case_, caseLog, caseMailSetting, SessionFacade.CurrentUser.Id, this.User.Identity.Name, out errors);

            // save log
            var temporaryLogFiles = this.userTemporaryFilesStorage.GetFiles(caseLog.LogGuid.ToString(), TopicName.Log);
            caseLog.CaseId = case_.Id;
            caseLog.CaseHistoryId = caseHistoryId;
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);

            // save case files
            if (!edit)
            {
                var temporaryFiles = this.userTemporaryFilesStorage.GetFiles(case_.CaseGUID.ToString(), TopicName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, case_.Id)).ToList();
                this._caseFileService.AddFiles(newCaseFiles);
            }

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            // delete temp folders                
            this.userTemporaryFilesStorage.DeleteFiles(case_.CaseGUID.ToString());
            this.userTemporaryFilesStorage.DeleteFiles(caseLog.LogGuid.ToString());

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
                s.SortBy = "CaseNumber";
                s.Ascending = true; 

                m.caseSearchFilter = f;
                m.Search = s;
            }
            else
                m = SessionFacade.CurrentCaseSearch;

            return m;
        }

        private CaseInputViewModel GetCaseInputViewModel(int userId, int customerId, int caseId, int lockedByUserId = 0, string redirectFrom = "")
        {
            var m = new CaseInputViewModel();

            if (caseId != 0)
            {
                bool markCaseAsRead = string.IsNullOrWhiteSpace(redirectFrom) ? true : false;
                m.case_ = this._caseService.GetCaseById(caseId, markCaseAsRead);
                customerId = m.case_.Customer_Id;
            }

            // TODO check if user has access to department and workinggroup on the case

            var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);
            var customer = this._customerService.GetCustomer(customerId);   
            
            var cu = this._customerUserService.GetCustomerSettings(customerId, userId);
            if (cu == null)
                // user can only see cases on their custmomers     
                m.case_ = null;
            else
            {
                var cs = this._settingService.GetCustomerSetting(customerId);
                m.CaseIsLockedByUserId = lockedByUserId; 
                m.customerUserSetting = cu;
                m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                m.DepartmentFilterFormat = cs.DepartmentFilterFormat;
                m.ParantPath_CaseType = "--";
                m.ParantPath_ProductArea = "--";
                m.CaseFilesModel = new FilesModel();
                m.LogFilesModel = new FilesModel();

                if (caseId == 0)
                    m.case_ = this._caseService.InitCase(customerId, userId, SessionFacade.CurrentLanguageId, this.Request.GetIpAddress(), GlobalEnums.RegistrationSource.Case, cs, global::System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                else
                {
                    m.Logs = this._logService.GetLogsByCaseId(caseId);
                    //m.caseHistories = this._caseService.GetCaseHistoryByCaseId(caseId);
                    m.CaseFilesModel = new FilesModel(caseId.ToString(), this._caseFileService.FindFileNamesByCaseId(caseId));
                    m.RegByUser = this._userService.GetUser(m.case_.User_Id);   
                }

                m.CaseMailSetting = new CaseMailSetting(
                                                        customer.NewCaseEmailList
                                                        , customer.HelpdeskEmail
                                                        , RequestExtension.GetAbsoluteUrl()
                                                        , cs.DontConnectUserToWorkingGroup 
                                                        );

                // hämta parent path för productArea 
                if ((m.case_.ProductArea_Id.HasValue))
                {
                    var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                    if (p != null)
                        m.ParantPath_ProductArea = p.getProductAreaParentPath();
                }
                // hämta parent path för casetype
                if ((m.case_.CaseType_Id > 0))
                {
                    var c = this._caseTypeService.GetCaseType(m.case_.CaseType_Id);
                    if (c != null)
                        m.ParantPath_CaseType = c.getCaseTypeParentPath();
                }

                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1) 
                    m.caseTypes = this._caseTypeService.GetCaseTypes(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1) 
                    m.categories = this._categoryService.GetCategories(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1) 
                    m.impacts = this._impactService.GetImpacts(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1) 
                    m.ous = this._ouService.GetOUs(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1) 
                    m.priorities = this._priorityService.GetPriorities(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1) 
                    m.productAreas = this._productAreaService.GetProductAreas(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1)
                    m.regions = this._regionService.GetRegions(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1) 
                    m.statuses = this._statusService.GetStatuses(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1) 
                    m.stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString()).ShowOnStartPage == 1)
                {
                    m.suppliers = this._supplierService.GetSuppliers(customerId);
                    m.countries = this._countryService.GetCountries(customerId);
                }
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.System_Id.ToString()).ShowOnStartPage == 1)
                    m.systems = this._systemService.GetSystems(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Urgency_Id.ToString()).ShowOnStartPage == 1)
                    m.urgencies = this._urgencyService.GetUrgencies(customerId);
                if (m.caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
                    m.workingGroups = this._workingGroupService.GetWorkingGroups(customerId);
                if (cs.ModuleProject == 1)
                    m.projects = this._projectService.GetCustomerProjects(customerId);
                if (cs.ModuleChangeManagement == 1)
                    m.changes = this._changeService.GetChanges(customerId);

                m.finishingCauses = this._finishingCauseService.GetFinishingCauses(customerId);
                m.problems = this._problemService.GetCustomerProblems(customerId);
                m.currencies = this._currencyService.GetCurrencies();
                m.users = this._userService.GetUsers(customerId);
                m.projects = this._projectService.GetCustomerProjects(customerId);
                m.departments = deps ?? this._departmentService.GetDepartments(customerId);
                m.standardTexts = this._standardTextService.GetStandardTexts(customerId);

                if (cs.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
                    m.performers = _userService.GetUsersForWorkingGroup(customerId, m.case_.WorkingGroup_Id.Value);   
                else
                    m.performers = m.users;

                m.SendToDialogModel = CreateNewSendToDialogModel(customerId, m.users);
                m.CaseLog = this._logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty);
                m.CaseKey = m.case_.Id == 0 ? m.case_.CaseGUID.ToString() : m.case_.Id.ToString();
                m.LogKey = m.CaseLog.LogGuid.ToString();

                if (lockedByUserId > 0)
                {
                    var lbu = _userService.GetUser(lockedByUserId);
                    m.CaseIsLockedByUserName = lbu != null ? lbu.FirstName + " " + lbu.SurName : string.Empty;   
                }
                
                if (m.case_.Supplier_Id > 0 && m.suppliers != null)
                {
                    var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                    m.CountryId = sup.Country_Id.GetValueOrDefault();
                }

                m.ShowInvoiceFields = 0;
                if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)    
                {
                    var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                    if (d != null)
                        m.ShowInvoiceFields = d.Charge; 
                }
                m.EditMode = EditMode(m, TopicName.Cases); 
            }

            return m;
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

        private bool EditMode(CaseInputViewModel m, string topic)
        {
            if (m == null)
                return false;
            if (SessionFacade.CurrentUser == null)
                return false;
            if (m.case_.FinishingDate.HasValue)
                return false;
            if (m.CaseIsLockedByUserId > 0)
                return false;
            if (SessionFacade.CurrentUser.UserGroupId < 2)
                return false;
            if (topic == TopicName.Log)  
                if (SessionFacade.CurrentUser.UserGroupId == 2)  
                    if (SessionFacade.CurrentUser.Id != m.CaseLog.UserId)
                        return false;

            return true;
        }

        #endregion

    }
}
