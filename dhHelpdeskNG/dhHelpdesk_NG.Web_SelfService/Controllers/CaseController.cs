﻿namespace DH.Helpdesk.SelfService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Extensions;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Services.utils;
    using DH.Helpdesk.SelfService.Infrastructure;
    using Models.Shared;

    public class CaseController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IInfoService _infoService;
        private readonly ICaseService _caseService;
        private readonly ILogService _logService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserTemporaryFilesStorage _userTemporaryFilesStorage;
        private readonly ICaseFileService _caseFileService;
        private readonly ILogFileService _logFileService;
        private readonly IRegionService _regionService;
        private readonly IDepartmentService _departmentService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IImpactService _impactService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ISupplierService _supplierService;
        private readonly ISettingService _settingService;
        private readonly IComputerService _computerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly IEmailService _emailService;        
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;

        private const string ParentPathDefaultValue = "--";
        private const string EnterMarkup = "<br />";
        private readonly IOrganizationService _orgService;
        private readonly OrganizationJsonService _orgJsonService;
        private readonly char[] EMAIL_SEPARATOR = new char[] { ';' };

        public CaseController(
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IMasterDataService masterDataService,
            ILogService logService,
            IInfoService infoService,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ICaseFileService caseFileService,
            IRegionService regionService,
            IDepartmentService departmentService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IImpactService impactService,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ISupplierService supplierService,
            ICustomerService customerService,
            ISettingService settingService,
            IComputerService computerService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseSearchService caseSearchService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IStateSecondaryService stateSecondaryService,
            ILogFileService logFileService,
            ICaseSolutionService caseSolutionService,
            IOrganizationService orgService,            
            OrganizationJsonService orgJsonService,
            ICaseSolutionSettingService caseSolutionSettingService,
            IEmailService emailService,
            ICaseExtraFollowersService caseExtraFollowersService)
            : base(masterDataService, caseSolutionService)
        {
            _masterDataService = masterDataService;
            _caseService = caseService;
            _logService = logService;
            _caseFieldSettingService = caseFieldSettingService;
            _infoService = infoService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create("Case");
            _caseFileService = caseFileService;
            _regionService = regionService;
            _departmentService = departmentService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _currencyService = currencyService;
            _logFileService = logFileService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _systemService = systemService;
            _settingService = settingService;
            _computerService = computerService;
            _customerUserService = customerUserService;
            _customerService = customerService;
            _caseSettingService = caseSettingService;
            _caseSearchService = caseSearchService;
            _workingGroupService = workingGroupService;
            _userService = userService;
            _stateSecondaryService = stateSecondaryService;
            _caseSolutionService = caseSolutionService;
            _orgService = orgService;
            _orgJsonService = orgJsonService;
            _emailService = emailService;
            _urgencyService = urgencyService;
            _impactService = impactService;
            _caseSolutionSettingService = caseSolutionSettingService;
            _caseExtraFollowersService = caseExtraFollowersService;   
        }


        [HttpGet]
        public ActionResult Index(string id, bool showRegistrationMessage = false)
        {
            if(string.IsNullOrEmpty(id))
                return null;

            int customerId;

            Case currentCase = null;

            if(id.Is<Guid>())
            {
                var guid = new Guid(id);
                currentCase = _caseService.GetCaseByEMailGUID(guid);

                var dynamicCases = _caseService.GetAllDynamicCases();
                var dynamicUrl = "";
                if (dynamicCases != null && dynamicCases.Any())
                {
                    dynamicUrl = dynamicCases.Where(w => w.CaseId == currentCase.Id).Select(d => "/" + d.FormPath).FirstOrDefault();
                    if (!string.IsNullOrEmpty(dynamicUrl)) // Show Case in eform
                    {
                        var urlStr = dynamicUrl.SetUrlParameters(currentCase.Id);
                        var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, urlStr);                         
                        return Redirect(url);
                    }
                }
            }
            else
            {
                currentCase = _caseService.GetCaseById(Int32.Parse(id));

                if (currentCase == null)
                {
                    ErrorGenerator.MakeError("Case not found!");
                    return RedirectToAction("Index", "Error");
                }

                bool userHasAccessToCase = false;
                var currentApplicationType = ConfigurationManager.AppSettings[AppSettingsKey.CurrentApplicationType].ToString().ToLower();
                var curUserId = SessionFacade.CurrentUserIdentity.UserId;
                var caseListCondition = ConfigurationManager.AppSettings[AppSettingsKey.CaseList].ToString().ToLower().Split(',');

                if (currentApplicationType == ApplicationTypes.LineManager)
                {
                    var coWorkers = SessionFacade.CurrentCoWorkers != null ?
                                        SessionFacade.CurrentCoWorkers.Select(c => c.EmployeeNumber).ToList() : 
                                        new List<string>();
                                        
                    if (caseListCondition.Contains(CaseListTypes.ManagerCases))
                    {
                        if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                        {
                            if ((!string.IsNullOrEmpty(currentCase.RegUserId) && currentCase.RegUserId.ToLower() == curUserId.ToLower()) 
                                || coWorkers.Contains(currentCase.ReportedBy))
                                userHasAccessToCase = true;               
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(currentCase.RegUserId) && currentCase.RegUserId.ToLower() == curUserId.ToLower())
                                || currentCase.ReportedBy == SessionFacade.CurrentUserIdentity.EmployeeNumber)
                                userHasAccessToCase = true;                                                                       
                        }                        
                    }
                    else
                    if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                    {
                        if (coWorkers.Contains(currentCase.ReportedBy) || 
                            (string.IsNullOrEmpty(currentCase.ReportedBy) && 
                            (!string.IsNullOrEmpty(currentCase.RegUserId) && currentCase.RegUserId.ToLower() == curUserId.ToLower())))
                            userHasAccessToCase = true;                        
                    }                                    
                }
                else
                {                                        
                    if (caseListCondition.Contains(CaseListTypes.ManagerCases))
                    {                        
                        if (!string.IsNullOrEmpty(currentCase.ReportedBy) && 
                            currentCase.ReportedBy.ToLower() == curUserId.ToLower())
                        { 
                                userHasAccessToCase = true;
                        }
                    }

                    if ((!string.IsNullOrEmpty(currentCase.RegUserId) && currentCase.RegUserId.ToLower() == curUserId.ToLower()))
                        userHasAccessToCase = true;   
                }

                if (!userHasAccessToCase)
                {
                        ErrorGenerator.MakeError("Case not found among your cases!");
                        return RedirectToAction("Index", "Error");
                }                
            }

            if(currentCase == null)
            {
                ErrorGenerator.MakeError("Case not found!");
                return RedirectToAction("Index", "Error");                
            }
            else
            {
                if(currentCase.FinishingDate == null)
                {
                    ViewBag.CurrentCaseId = currentCase.Id;
                }

                currentCase.Description = currentCase.Description.Replace("\n", EnterMarkup);
                customerId = currentCase.Customer_Id;
            }

            Customer currentCustomer = default(Customer);

            if (SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");                
            }

            currentCase.Customer = currentCustomer;


            var languageId = SessionFacade.CurrentLanguageId;
            var caseReceipt = GetCaseReceiptModel(currentCase, languageId);

            caseReceipt.ShowRegistringMessage = showRegistrationMessage;
            caseReceipt.ExLogFileGuid = currentCase.CaseGUID.ToString();
            caseReceipt.MailGuid = id;

            _userTemporaryFilesStorage.DeleteFiles(caseReceipt.ExLogFileGuid);
            _userTemporaryFilesStorage.DeleteFiles(id);
                        
            if(id.Is<Guid>())
            {
                if(currentCase.StateSecondary_Id.HasValue && caseReceipt.CasePreview.FinishingDate == null)
                {
                    var stateSecondary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);
                    if(stateSecondary.NoMailToNotifier == 1)
                        caseReceipt.CasePreview.FinishingDate = DateTime.UtcNow; 
                }
                caseReceipt.CanAddExternalNote = true;
            }
            else
            {
                var htmlFooterData = string.Empty;
                var registrationInfoText = _infoService.GetInfoText((int)InfoTextType.SelfServiceRegistrationMessage, SessionFacade.CurrentCustomer.Id, languageId);

                if (registrationInfoText != null && !string.IsNullOrEmpty(registrationInfoText.Name))
                    htmlFooterData = registrationInfoText.Name;

                caseReceipt.CaseRegistrationMessage = htmlFooterData;
                caseReceipt.CanAddExternalNote = false;
            }

            return View(caseReceipt);
        }

        [HttpGet]
        public ActionResult NewCase(int customerId, int? caseTemplateId)
        {
            // *** New Case ***
            var currentCustomer = default(Customer);
            if (SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }
            var languageId = SessionFacade.CurrentLanguageId;

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId)
                                                          .Where(c => c.ShowExternal == 1)
                                                          .ToList();

            
            var model = GetNewCaseModel(currentCustomer.Id, languageId, caseFieldSetting);
            model.ExLogFileGuid = Guid.NewGuid().ToString();

            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);

            if (SessionFacade.CurrentUserIdentity != null)
            {                
                model.NewCase = _caseService.InitCase(
                    currentCustomer.Id,
                    0,
                    SessionFacade.CurrentLanguageId,
                    Request.GetIpAddress(),
                    CaseRegistrationSource.SelfService,
                    cs, SessionFacade.CurrentUserIdentity.UserId);

                model.NewCase.Customer = currentCustomer;
                model.CaseMailSetting = new CaseMailSetting(
                    currentCustomer.NewCaseEmailList,
                    currentCustomer.HelpdeskEmail,
                    ConfigurationManager.AppSettings[AppSettingsKey.HelpdeskPath].ToString(),
                    cs.DontConnectUserToWorkingGroup);

                model.NewCase.RegUserId = SessionFacade.CurrentUserIdentity.UserId;
                model.NewCase.RegUserDomain = SessionFacade.CurrentUserIdentity.Domain;
 
            }

            model.CaseTypeParantPath = ParentPathDefaultValue;
            model.ProductAreaParantPath = ParentPathDefaultValue;

            // Load template info
            if(caseTemplateId != null && caseTemplateId.Value > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId.Value);
                var caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseTemplateId.Value).ToList();

                var jsFieldSettings = GetFieldSettingsModel(caseFieldSetting, caseTemplateSettings);
                model.JsFieldSettings = jsFieldSettings;

                if (caseTemplate.Status == 0 || !caseTemplate.ShowInSelfService)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return RedirectToAction("Index", "Error");
                }

                if(caseTemplate != null)
                {
                    if(caseTemplate.CaseType_Id != null)
                    {
                        model.NewCase.CaseType_Id = caseTemplate.CaseType_Id.Value;
                    }

                    if(caseTemplate.PerformerUser_Id != null)
                    {
                        model.NewCase.Performer_User_Id = caseTemplate.PerformerUser_Id.Value;
                    }

                    var notifier = _computerService.GetInitiatorByUserId(SessionFacade.CurrentUserIdentity.UserId, customerId);

                    model.NewCase.ReportedBy = string.IsNullOrEmpty(caseTemplate.ReportedBy)? notifier?.UserId : caseTemplate.ReportedBy;
                    model.NewCase.PersonsName = string.IsNullOrEmpty(caseTemplate.PersonsName) ? 
                                                    (cs.IsUserFirstLastNameRepresentation != 0 ? string.Format("{0} {1}", notifier?.FirstName, notifier?.LastName) :
                                                                                                 string.Format("{0} {1}", notifier?.LastName, notifier?.FirstName))  :
                                                    caseTemplate.PersonsName;

                    model.NewCase.PersonsEmail = string.IsNullOrEmpty(caseTemplate.PersonsEmail) ? notifier?.Email: caseTemplate.PersonsEmail;
                    model.NewCase.PersonsPhone = string.IsNullOrEmpty(caseTemplate.PersonsPhone) ? notifier?.Phone : caseTemplate.PersonsPhone;
                    model.NewCase.PersonsCellphone = string.IsNullOrEmpty(caseTemplate.PersonsCellPhone) ? notifier?.CellPhone : caseTemplate.PersonsCellPhone;
                    model.NewCase.Region_Id = caseTemplate.Region_Id;
                    model.NewCase.Department_Id = caseTemplate.Department_Id.HasValue ? caseTemplate.Department_Id.Value : notifier?.DepartmentId;
                    model.NewCase.OU_Id = caseTemplate.OU_Id.HasValue ? caseTemplate.OU_Id.Value : notifier?.OrganizationUnitId;
                    model.NewCase.Place = string.IsNullOrEmpty(caseTemplate.Place) ? notifier?.Place : caseTemplate.Place;
                    model.NewCase.UserCode = string.IsNullOrEmpty(caseTemplate.UserCode) ? notifier?.Code : caseTemplate.UserCode;
                    model.NewCase.CostCentre = string.IsNullOrEmpty(caseTemplate.CostCentre) ? notifier?.CostCentre : caseTemplate.CostCentre;

                    model.NewCase.InventoryNumber = string.IsNullOrEmpty(caseTemplate.InventoryNumber)? Request.GetComputerName() : caseTemplate.InventoryNumber;
                    model.NewCase.InventoryType = caseTemplate.InventoryType;
                    model.NewCase.InventoryLocation = caseTemplate.InventoryLocation;

                    model.NewCase.ProductArea_Id = caseTemplate.ProductArea_Id;
                    model.NewCase.System_Id = caseTemplate.System_Id;
                    model.NewCase.Caption = caseTemplate.Caption;
                    model.NewCase.Description = caseTemplate.Description;
                    model.NewCase.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
                    model.NewCase.Priority_Id = caseTemplate.Priority_Id;
                    model.NewCase.Project_Id = caseTemplate.Project_Id;
                    model.NewCase.Urgency_Id = caseTemplate.Urgency_Id;
                    model.NewCase.Impact_Id = caseTemplate.Impact_Id;
                    model.NewCase.Category_Id = caseTemplate.Category_Id;
                    model.NewCase.Supplier_Id = caseTemplate.Supplier_Id;

                    model.NewCase.InvoiceNumber = caseTemplate.InvoiceNumber;
                    model.NewCase.ReferenceNumber = caseTemplate.ReferenceNumber;
                    model.NewCase.Miscellaneous = caseTemplate.Miscellaneous;
                    model.NewCase.ContactBeforeAction = caseTemplate.ContactBeforeAction;
                    model.NewCase.SMS = caseTemplate.SMS;
                    model.NewCase.Available = caseTemplate.Available;
                    model.NewCase.Cost = caseTemplate.Cost;
                    model.NewCase.OtherCost = caseTemplate.OtherCost;
                    model.NewCase.Currency = caseTemplate.Currency;

                }

                if(model.NewCase.ProductArea_Id.HasValue)
                {
                    var p = _productAreaService.GetProductArea(model.NewCase.ProductArea_Id.GetValueOrDefault());
                    if(p != null)
                    {
                        var pathTexts = _productAreaService.GetParentPath(p.Id, currentCustomer.Id).ToList();
                        var translatedText = pathTexts;
                        if (pathTexts.Any())
                        {
                            translatedText = new List<string>();
                            foreach (var pathText in pathTexts.ToList())
                                translatedText.Add(Translation.Get(pathText, Enums.TranslationSource.TextTranslation));
                        }
                        model.ProductAreaParantPath = string.Join(" - ", translatedText);
                    }
                }

                if(model.NewCase.CaseType_Id > 0)
                {
                    var ct = _caseTypeService.GetCaseType(model.NewCase.CaseType_Id);
                    var tempCTs = new List<CaseType>();
                    tempCTs.Add(ct);
                    tempCTs = CaseTypeTreeTranslation(tempCTs).ToList();
                    model.CaseTypeParantPath = tempCTs[0].getCaseTypeParentPath();                   
                }

                if (model.NewCase.Department_Id.HasValue)
                {
                    if (!model.NewCase.Region_Id.HasValue)
                    {
                        var reg = _departmentService.GetDepartment(model.NewCase.Department_Id.Value);
                        if (reg != null)
                        {
                            model.NewCase.Region_Id = reg.Region_Id;
                            if (model.NewCase.Region_Id.HasValue)
                                model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                        }
                            
                        var ous = _orgService.GetOUs(model.NewCase.Department_Id);
                        model.OrganizationUnits = ous;
                    }
                    else
                    {
                        model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                        if (model.Departments.Select(d=> d.Id).Contains(model.NewCase.Department_Id.Value))
                        {
                            var ous = _orgService.GetOUs(model.NewCase.Department_Id.Value);
                            model.OrganizationUnits = ous;
                        }
                    }
                }
                else
                {
                    if (model.NewCase.Region_Id.HasValue)                    
                        model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();                                        
                }

            } // Load Case Template

            return View("NewCase", model);
        }

        public ActionResult GetDepartmentsByRegion(int? id, int customerId, int departmentFilterFormat)
        {
            var list = _orgJsonService.GetActiveDepartmentForRegion(id, customerId, departmentFilterFormat);
            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrgUnitsByDepartments(int? id, int customerId)
        {
            var list = _orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserCases(int customerId, string progressId = "")
        {
            var currentCustomer = default(Customer);
            if (SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }

            var pharaseSearch = "";        
            if (progressId == "")
            {
                if (SessionFacade.CurrentCaseSearch != null)
                {
                    var lastprogressId = SessionFacade.CurrentCaseSearch.caseSearchFilter?.CaseProgress;
                    pharaseSearch = SessionFacade.CurrentCaseSearch.caseSearchFilter?.FreeTextSearch;
                    progressId = (!string.IsNullOrEmpty(lastprogressId) ? lastprogressId : CaseProgressFilter.CasesInProgress);
                }
                else
                {
                    progressId = CaseProgressFilter.CasesInProgress;
                }
            }

            var languageId = SessionFacade.CurrentLanguageId;

            UserCasesModel model = null;

            if (progressId != CaseProgressFilter.ClosedCases && 
                progressId != CaseProgressFilter.CasesInProgress &&
                progressId != CaseProgressFilter.None)
            {
                ErrorGenerator.MakeError("Process is not valid!", 202);
                return RedirectToAction("Index", "Error");                
            }

            if(SessionFacade.CurrentUserIdentity != null)
            {

                model = GetUserCasesModel(currentCustomer.Id, languageId, SessionFacade.CurrentUserIdentity.UserId, pharaseSearch, 20, progressId);

            }
            else
            {
                ErrorGenerator.MakeError("You don't have access to cases, please login again.", 203);
                return RedirectToAction("Index", "Error");                
            }

            return View("CaseOverview", model);
        }

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
               fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = _caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadNewCaseFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent =  _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = _caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }
            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(_userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void NewCaseUploadFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(_userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void DeleteFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var log = _logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = _caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                _logFileService.DeleteByLogIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpPost]
        public void DeleteNewCaseFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                _caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpGet]
        public JsonResult Files(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id)
                                : _logFileService.FindFileNamesByLogId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult NewCaseFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id)
                                : _caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult DownloadLogFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log);
            else
            {
                var log = _logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = _caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                } 

                fileContent = _logFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ActionResult _CaseLogNote(int caseId, string note, string logFileGuid)
        {
            SaveExternalMessage(caseId, note, logFileGuid);            
            CaseLogModel model = new CaseLogModel()
                  {
                      CaseId = caseId,
                      CaseLogs = _logService.GetLogsByCaseId(caseId).OrderByDescending(l=> l.RegTime).ToList()                      
                  };            
            return PartialView(model);
        }

        private void SaveExternalMessage(int caseId, string extraNote, string logFileGuid) 
        {
            IDictionary<string, string> errors;            
            var currentCase = _caseService.GetCaseById(caseId);
            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);
            // save case history

            // unread/status flag update if not case is closed
            if (!currentCase.FinishingDate.HasValue)
                currentCase.Unread = 1;

            if (currentCase.FinishingDate.HasValue)
            {
                string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                this._caseService.Activate(currentCase.Id, 0, adUser, CreatedByApplications.SelfService5, out errors);
            }
                

            // if statesecondary has ResetOnExternalUpdate
            if (currentCase.StateSecondary_Id.HasValue)
            {
                //get substatus
                var casestatesecundary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);

                if (casestatesecundary.ResetOnExternalUpdate == 1)
                    currentCase.StateSecondary_Id = null;
            }


            currentCase.ChangeTime = DateTime.UtcNow;
            int caseHistoryId = _caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, CreatedByApplications.SelfService5,  out errors, SessionFacade.CurrentUserIdentity.UserId);            
            // save log
            var caseLog = new CaseLog
                              {
                                  CaseHistoryId = caseHistoryId,
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  TextExternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseExternalLogNote? extraNote : string.Empty),
                                  UserId = null,
                                  TextInternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseInternalLogNote? extraNote : string.Empty),
                                  WorkingTime = 0,
                                  EquipmentPrice = 0,
                                  Price = 0,
                                  Charge = false,
                                  RegUser = SessionFacade.CurrentSystemUser,
                                  SendMailAboutCaseToNotifier = true,
                                  SendMailAboutLog = true                                 
                              };
            
            if(currentCase.WorkingGroup_Id != null)
            {
                var curWorkingGroup = _workingGroupService.GetWorkingGroup(currentCase.WorkingGroup_Id.Value);
                if(curWorkingGroup.AllocateCaseMail == 1) // Send To Users Working Group EMail
                {
                    // Send Mail to all working group users
                    var usersInWorkingGroup = _userService.GetUsersForWorkingGroup(currentCase.WorkingGroup_Id.Value).Where(u => u.Email.Trim() != string.Empty).ToList();
                    var emailTo = new List<string>();                    
                    if(usersInWorkingGroup != null && usersInWorkingGroup.Count > 0)
                    {
                        if(currentCase.Department_Id.HasValue)
                        {
                            foreach(var user in usersInWorkingGroup)
                            {
                                var departments = _departmentService.GetDepartmentsByUserPermissions(user.Id, currentCase.Customer_Id);

                                if(departments != null && departments.FirstOrDefault(x=>x.Id == currentCase.Department_Id.Value) != null)
                                {
                                    emailTo.Add(user.Email);
                                }

                                if(departments == null)
                                    emailTo.Add(user.Email);
                            }
                        }
                        else
                        {                            
                            emailTo = usersInWorkingGroup.Select(u => u.Email).ToList();
                        }
                    }                    
                    if(emailTo.Count > 0)
                        caseLog.EmailRecepientsExternalLog = string.Join(Environment.NewLine, emailTo);
                }
            }

            var caseMailSetting = new CaseMailSetting(
                                                          currentCustomer.NewCaseEmailList,
                                                          currentCustomer.HelpdeskEmail,
                                                          ConfigurationManager.AppSettings[AppSettingsKey.HelpdeskPath].ToString(),
                                                          cs.DontConnectUserToWorkingGroup
                                                        );

            var temporaryLogFiles = new List<WebTemporaryFile>();
            if (!string.IsNullOrEmpty(logFileGuid))
            {
                temporaryLogFiles = _userTemporaryFilesStorage.GetFiles(logFileGuid, ModuleName.Log);
                caseLog.Id = _logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                var basePath = _masterDataService.GetFilePath(currentCase.Customer_Id);
                // save log files
                var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
                _logFileService.AddFiles(newLogFiles);
                // send emails
                var userTimeZone = TimeZoneInfo.Local;
                _caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, basePath, userTimeZone, newLogFiles);
                _userTemporaryFilesStorage.DeleteFiles(logFileGuid);
            }
            else
            {
                caseLog.Id = _logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                _caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, string.Empty, null);
            }
        }

        [HttpPost]
        public RedirectToRouteResult NewCase(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey, string followerUsers)
        {
            int caseId = Save(newCase, caseMailSetting, caseFileKey, followerUsers);
            return RedirectToAction("Index", "case", new { id = newCase.Id, showRegistrationMessage = true });
        }

        [HttpPost]
        public ActionResult SearchUser(string query, int customerId, string searchKey)
        {
            var result = _computerService.SearchComputerUsers(customerId, query);            
            return Json(new { searchKey = searchKey, result = result });              
        }

        [HttpPost]
        public ActionResult SearchComputer(string query, int customerId)
        {
            var result = _computerService.SearchComputer(customerId, query);
            return Json(result);
        }

        public ActionResult SearchUserCase(FormCollection frm)
        {
            try
            {
                var customerId = frm.ReturnFormValue("customerId").convertStringToInt();
                var languageId = frm.ReturnFormValue("languageId").convertStringToInt();
                var userId = frm.ReturnFormValue("userId");
                var pharasSearch = frm.ReturnFormValue("pharasSearch");
                var maxRecords = frm.ReturnFormValue("maxRecords").convertStringToInt();
                var progressId = frm.ReturnFormValue("progressId");
                var sortBy = frm.ReturnFormValue("hidSortBy");
                var ascending = frm.ReturnFormValue("hidSortByAsc").ConvertStringToBool();
                var id = frm.ReturnFormValue("MailGuid");

                if (progressId != CaseProgressFilter.ClosedCases && 
                    progressId != CaseProgressFilter.CasesInProgress &&
                    progressId != CaseProgressFilter.None)
                {                    
                    ErrorGenerator.MakeError("Process is not valid!", 202);
                    return RedirectToAction("Index", "Error");                                 
                }

                var model = GetUserCasesModel(customerId, languageId, userId,
                                              pharasSearch, maxRecords, progressId,
                                              sortBy, ascending);

                model.ProgressId = progressId;
                return PartialView("CaseOverview", model);
            }
            catch(Exception e)
            {
                ErrorGenerator.MakeError("Not able to load user cases.", 204);
                return RedirectToAction("Index", "Error");                
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CaseSearchUserEmails(string query, string searchKey)
        {
            var models = _caseSearchService.GetUserEmailsForCaseSend(SessionFacade.CurrentCustomer.Id, query, false, true, false, false);
            return Json(new { searchKey = searchKey, result = models });
        }

        public List<CaseSolution> GetCaseSolutions(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t => t.ShowInSelfService).ToList();
        }   

        [HttpPost]
        public void UploadLogFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (_userTemporaryFilesStorage.FileExists(name, id, ModuleName.Log))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                _userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Log);
            }
        }

        [HttpGet]
        public JsonResult GetLogFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id, ModuleName.Log)
                                : _logFileService.FindFileNamesByLogId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);            
        }

        
        private int Save(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey, string followerUsers)
        {
            IDictionary<string, string> errors;

            // save case and case history
            if (newCase.User_Id <= 0)
                newCase.User_Id = null;

            var mailSenders = new MailSenders();            
            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;
            if (newCase.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(newCase.WorkingGroup_Id.Value);
                if (curWG != null)
                    if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                        mailSenders.WGEmail = curWG.EMail;
            }
            caseMailSetting.CustomeMailFromAddress = mailSenders;

            var basePath = _masterDataService.GetFilePath(newCase.Customer_Id);
            newCase.LatestSLACountDate = CalculateLatestSLACountDate(newCase.StateSecondary_Id);
            var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.SelfService5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0};

            if (newCase.Urgency_Id.HasValue && newCase.Impact_Id.HasValue)
            {
                var prio_Impact_Urgent = _urgencyService.GetPriorityImpactUrgencies(newCase.Customer_Id);
                var prioInfo = prio_Impact_Urgent.FirstOrDefault(p=> p.Impact_Id == newCase.Impact_Id && p.Urgency_Id == newCase.Urgency_Id);
                if (prioInfo != null)
                {
                    newCase.Priority_Id = prioInfo.Priority_Id;
                }
            }

            if (newCase.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(newCase.ProductArea_Id.Value);                
                if (productArea != null && productArea.WorkingGroup_Id.HasValue)
                {
                    newCase.WorkingGroup_Id = productArea.WorkingGroup_Id;
                }
            }

            int caseHistoryId = _caseService.SaveCase(newCase, null, caseMailSetting, 0, SessionFacade.CurrentUserIdentity.UserId, ei, out errors);
            
            // save case files            
            var temporaryFiles = _userTemporaryFilesStorage.GetFiles(caseFileKey, ModuleName.Cases);
            var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, newCase.Id)).ToList();
            _caseFileService.AddFiles(newCaseFiles);

            // delete temp folders                
            _userTemporaryFilesStorage.DeleteFiles(caseFileKey);
            var oldCase = new Case();            
            // send emails
            var userTimeZone = TimeZoneInfo.Local;

            //save extra followers            
            if (!string.IsNullOrEmpty(followerUsers))
            {
                var _followerUsers = followerUsers.Split(EMAIL_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                _caseExtraFollowersService.SaveExtraFollowers(newCase.Id, _followerUsers, null);
            }

            _caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase);

            return newCase.Id;
        }

        private DateTime? CalculateLatestSLACountDate(int? newSubStateId)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;            

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else
                ret = null;
            
            return ret;
        }

        private CaseOverviewModel GetCaseReceiptModel(Case currentCase, int languageId)
        {
            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString() ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.RegTime.ToString()) 
                                                           .ToList();
            
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);            
            var infoText = _infoService.GetInfoText((int) InfoTextType.SelfServiceInformation, currentCase.Customer_Id, languageId);

            // get customersettings
            var customersettings = _settingService.GetCustomerSetting(currentCase.Customer_Id);

            var regions = _regionService.GetRegions(currentCase.Customer_Id);
            var suppliers = _supplierService.GetSuppliers(currentCase.Customer_Id);
            var systems = _systemService.GetSystems(currentCase.Customer_Id);
            if (currentCase.CaseType != null)
            {
                var tempCTs = new List<CaseType>();
                tempCTs.Add(currentCase.CaseType);
                tempCTs = CaseTypeTreeTranslation(tempCTs).ToList();
                currentCase.CaseType.Name = tempCTs[0].getCaseTypeParentPath();                
            }

            if (currentCase.ProductArea_Id.HasValue && currentCase.ProductArea != null)
            {
                var pathTexts = _productAreaService.GetParentPath(currentCase.ProductArea_Id.Value, currentCase.Customer_Id).ToList();
                var translatedText = pathTexts;
                if (pathTexts.Any())
                {
                    translatedText = new List<string>();
                    foreach (var pathText in pathTexts.ToList())
                        translatedText.Add(Translation.Get(pathText, Enums.TranslationSource.TextTranslation));
                }
                currentCase.ProductArea.Name = string.Join(" - ", translatedText);                
            }

            var newLogFile = new FilesModel();

            CaseOverviewModel model = null;

            if(currentCase != null)
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).OrderByDescending(l => l.LogDate).ToList();
                if (currentCase.Impact_Id.HasValue && currentCase.Impact == null)
                    currentCase.Impact = _impactService.GetImpact(currentCase.Impact_Id.Value);

                model = new CaseOverviewModel
                {
                    InfoText = infoText != null ? infoText.Name : null,
                    CasePreview = currentCase,
                    CaseFieldGroups = caseFieldGroups,
                    CaseLogs = caselogs,
                    FieldSettings = caseFieldSetting,
                    LogFilesModel = newLogFile,
                    Regions = regions,
                    Suppliers = suppliers,
                    Systems = systems,
                    LogFileGuid = Guid.NewGuid().ToString(),
                    CustomerSettings = customersettings
                };
            }

            var caseFolowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(currentCase.Id).Select(x => x.Follower).ToArray();
            var followerUsers = caseFolowerUsers.Any() ? string.Join(";", caseFolowerUsers) + ";" : string.Empty;
            model.FollowerUsers = followerUsers;

            return model;
        }

        private UserCasesModel GetUserCasesModel(int customerId, int languageId, string curUser,
                                                 string pharasSearch, int maxRecords, string progressId = "",
                                                 string sortBy = "", bool ascending = false)
        {
            var cusId = customerId;

            var model = new UserCasesModel
                    {
                        CustomerId = cusId,
                        LanguageId = languageId,
                        UserId = curUser,
                        MaxRecords = maxRecords,
                        PharasSearch = pharasSearch,
                        ProgressId = progressId
                    };

            var srm = new CaseSearchResultModel();
            var sm = new CaseSearchModel();
            var cs = new CaseSearchFilter();
            var search = new Search();

            if(string.IsNullOrEmpty(sortBy)) sortBy = "Casenumber";

            cs.CustomerId = cusId;
            cs.FreeTextSearch = pharasSearch;
            cs.CaseProgress = progressId;
            cs.ReportedBy = "";
            var caseListType = string.Empty;
            var currentApplicationType = ConfigurationManager.AppSettings[AppSettingsKey.CurrentApplicationType].ToString().ToLower();

            var caseListCondition = ConfigurationManager.AppSettings[AppSettingsKey.CaseList].ToString().ToLower().Split(',');

            if (currentApplicationType == ApplicationTypes.LineManager)
            {                
                if (caseListCondition.Contains(CaseListTypes.ManagerCases))
                {
                    caseListType = CaseListTypes.ManagerCases;
                    cs.RegUserId = curUser;
                    cs.ReportedBy = string.Format("'{0}'", SessionFacade.CurrentUserIdentity.EmployeeNumber);
                }

                if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                {
                    cs.RegUserId = curUser;

                    if (caseListType == CaseListTypes.ManagerCases)
                        caseListType = CaseListTypes.ManagerCoWorkerCases;
                    else
                        caseListType = CaseListTypes.CoWorkerCases;
                    
                    var employees = SessionFacade.CurrentCoWorkers;
                    if (employees != null)
                    {
                        foreach (var emp in employees)
                        {
                            if (!string.IsNullOrEmpty(emp.EmployeeNumber))
                            {
                                if (string.IsNullOrEmpty(cs.ReportedBy))
                                    cs.ReportedBy = string.Format("'{0}'", emp.EmployeeNumber);
                                else
                                    cs.ReportedBy = string.Format("{0},'{1}'", cs.ReportedBy, emp.EmployeeNumber);
                            }
                        }
                    }

                }

                cs.CaseListType = caseListType;
            } 
            else// Self Service 
            {                
                if (caseListCondition.Contains(CaseListTypes.ManagerCases))
                {
                    caseListType = CaseListTypes.ManagerCases;
                    cs.ReportedBy = string.Format("'{0}'", curUser);
                }

                /* CoWorker is not defined in SelfService Mode */
                if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                {
                    caseListType = CaseListTypes.UserCases;
                }

                if (caseListCondition.Contains(CaseListTypes.UserCases) || !caseListCondition.Any())
                    caseListType = CaseListTypes.UserCases;

                cs.RegUserId = curUser;
                cs.CaseListType = caseListType;
            }            

            search.SortBy = sortBy;
            search.Ascending = ascending;

            sm.Search = search;
            sm.caseSearchFilter = cs;

            // 1: User in Customer Setting
            srm.CaseSettings = _caseSettingService.GetCaseSettingsByUserGroup(cusId, 1);

            if (caseListType == string.Empty && currentApplicationType == ApplicationTypes.LineManager)
                srm.Cases = null;
            else
            {
                var workTimeCalc = TimeZoneInfo.Local;
                var showRemainingTime = false;
                CaseRemainingTimeData remainingTimeData;
                CaseAggregateData aggregateData;
                var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(cusId).ToArray();
                srm.Cases = _caseSearchService.Search(
                    sm.caseSearchFilter,
                    srm.CaseSettings,
                    caseFieldSettings,
                    -1,
                    curUser,
                    1,
                    1,
                    1,
                    search,
                    SessionFacade.CurrentCustomer.WorkingDayStart,
                    SessionFacade.CurrentCustomer.WorkingDayEnd,
                    workTimeCalc,
                    currentApplicationType,
                    showRemainingTime,
                    out remainingTimeData,
                    out aggregateData).Items.ToList(); // Take(maxRecords)

                if (currentApplicationType == ApplicationTypes.LineManager)
                {
                    var dynamicCases = _caseService.GetAllDynamicCases();
                    model.DynamicCases = dynamicCases;
                }

            }
            srm.Cases = CaseSearchTranslate(srm.Cases, cusId);
            model.CaseSearchResult = srm;

            SessionFacade.CurrentCaseSearch = sm;
            SessionFacade.CurrentCaseSearch.caseSearchFilter.CaseProgress = progressId;
            SessionFacade.CurrentCaseSearch.caseSearchFilter.FreeTextSearch = pharasSearch;

            model.NumberOfCases = srm.Cases.Count;

            return model;
        }

        private NewCaseModel GetNewCaseModel(int customerId, int languageId, List<CaseListToCase> caseFieldSetting)
        {           
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);

            var newCase = new Case { Customer_Id = customerId };
            var caseFile = new FilesModel { Id = Guid.NewGuid().ToString() };

            //Region list            
            var regions = _regionService.GetRegions(customerId);

            //Department list
            var departments = _departmentService.GetDepartments(customerId);

            //Organization unit list
            var orgUnits = _orgService.GetOUs(customerId).ToList();

            //Case Type tree            
            var caseTypes = _caseTypeService.GetCaseTypes(customerId).Where(c=> c.ShowOnExternalPage != 0).ToList();
            caseTypes = CaseTypeTreeTranslation(caseTypes).ToList();

            //Product Area tree            
            var productAreas = _productAreaService.GetTopProductAreas(customerId).Where(p=> p.IsActive != 0 && p.ShowOnExternalPage != 0).ToList();
            var traversedData = ProductAreaTreeTranslation(productAreas);
            productAreas = traversedData.Item1.ToList();

            //System list            
            var systems = _systemService.GetSystems(customerId);

            //Urgent List
            var ugencies = _urgencyService.GetUrgencies(customerId);

            //Impact List
            var impacts = _impactService.GetImpacts(customerId);

            //Category List
            var categories = _categoryService.GetCategories(customerId);

            //Currency List
            var currencies = _currencyService.GetCurrencies();

            //Country list
            var suppliers = _supplierService.GetSuppliers(customerId);

            //Field Settings
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var caseFieldSettingsWithLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);

            var cs = _settingService.GetCustomerSetting(customerId);
            
            var model = new NewCaseModel(
                newCase, 
                regions, 
                departments,
                orgUnits,
                caseTypes, 
                productAreas, 
                systems,
                ugencies,
                impacts,
                categories, 
                currencies, 
                suppliers, 
                caseFieldGroups, 
                caseFieldSetting, 
                caseFile, 
                caseFieldSettings,
                new JsApplicationOptions()
                    {
                        customerId = customerId,
                        departmentFilterFormat = cs.DepartmentFilterFormat,
                        departmentsURL = Url.Content("~/Case/GetDepartmentsByRegion"),
                        orgUnitURL = Url.Content("~/Case/GetOrgUnitsByDepartments")
                    },
                caseFieldSettingsWithLanguages);

            model.CaseTypeParantPath = "--";
            model.ProductAreaParantPath = "--";
            model.CaseFileKey = Guid.NewGuid().ToString();
            model.ProductAreaChildren = traversedData.Item2.ToList();
            model.SendToDialogModel = new SendToDialogModel();            

            var _caseTypes = _caseTypeService.GetAllCaseTypes(customerId).Where(c => c.ShowOnExternalPage != 0).ToList();
            model.CaseTypeRelatedFields = _caseTypes.Select(c => new KeyValuePair<int, string>(c.Id, c.RelatedField)).ToList();
            return model;
        }
        
        private List<string> GetVisibleFieldGroups(List<CaseListToCase> fieldList)
        {
            List<string> ret = new List<string>();
            var t = GlobalEnums.TranslationCaseFields.Persons_Phone.ToString();
            string[] userInformationGroup = new string[] 
                        {   
                            GlobalEnums.TranslationCaseFields.ReportedBy.ToString(),                             
                            GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(), 
                            GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(),
                            GlobalEnums.TranslationCaseFields.Customer_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Region_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Department_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.OU_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Place.ToString(), 
                            GlobalEnums.TranslationCaseFields.UserCode.ToString(),
                            GlobalEnums.TranslationCaseFields.CostCentre.ToString()
                        };

            string[] computerInformationGroup = new string[] 
                        { 
                            GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(), 
                            GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.InventoryLocation.ToString()
                        };
                            
            string[] caseInfoGroup = new string[] 
                        { 
                            GlobalEnums.TranslationCaseFields.CaseNumber.ToString(), 
                            GlobalEnums.TranslationCaseFields.RegTime.ToString(), 
                            GlobalEnums.TranslationCaseFields.ChangeTime.ToString(), 
                            GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.System_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Urgency_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Impact_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Category_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(), 
                            GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString(), 
                            GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(), 
                            GlobalEnums.TranslationCaseFields.Caption.ToString(), 
                            GlobalEnums.TranslationCaseFields.Description.ToString(), 
                            GlobalEnums.TranslationCaseFields.Miscellaneous.ToString(), 
                            GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(), 
                            GlobalEnums.TranslationCaseFields.SMS.ToString(), 
                            GlobalEnums.TranslationCaseFields.AgreedDate.ToString(), 
                            GlobalEnums.TranslationCaseFields.Available.ToString(), 
                            GlobalEnums.TranslationCaseFields.Cost.ToString(),
                            GlobalEnums.TranslationCaseFields.Filename.ToString(),
                        };

            string[] otherGroup = new string[] 
                        { 
                            GlobalEnums.TranslationCaseFields.PlanDate.ToString(),
                            GlobalEnums.TranslationCaseFields.WatchDate.ToString(),
                            GlobalEnums.TranslationCaseFields.Verified.ToString(),
                            GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                            GlobalEnums.TranslationCaseFields.SolutionRate.ToString()
                            
                        };

            string[] caseLogGroup = new string[] 
                        { 
                            GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(),
                            GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(),
                            GlobalEnums.TranslationCaseFields.FinishingDescription.ToString(),
                            GlobalEnums.TranslationCaseFields.FinishingDate.ToString(),
                        };

            
            foreach(var field in fieldList)
            {                
                if(userInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.UserInformation);

                if(computerInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.ComputerInformation);

                if(caseInfoGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseInfo);

                if(otherGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.Other);

                if(caseLogGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseLog);
            }

            return ret;
        }
       
        private List<FieldSettingJSModel> GetFieldSettingsModel(List<CaseListToCase> customerFieldSettings, List<CaseSolutionSettingOverview> templateSettings)
        {
            var ret = new List<FieldSettingJSModel>();
            foreach (var field in customerFieldSettings)
            {
                var isVisible = field.ShowExternal.ConvertIntToBool();
                var isRequired = field.Required.ConvertIntToBool();
                var isReadonly = false;                
                
                if (templateSettings != null && templateSettings.Any())
                {
                    var curFieldName = field.Name.ToLower();
                    var templateField = templateSettings.Where(t => t.CaseSolutionField.MapToCaseField().ToString().ToLower() == curFieldName).SingleOrDefault(); 
                    if (templateField != null)
                    {
                        isReadonly = templateField.CaseSolutionMode == Common.Enums.Settings.CaseSolutionModes.ReadOnly;
                    }

                    if (templateField != null && isVisible)
                    {
                        isVisible = templateField.CaseSolutionMode != Common.Enums.Settings.CaseSolutionModes.Hide;
                    }
                }

                ret.Add(new FieldSettingJSModel(field.Name, isVisible, isReadonly, isRequired));
            }

            return ret;
        }        

        private Tuple<IList<ProductArea>, IList<ProductAreaChild>> ProductAreaTreeTranslation(IList<ProductArea> productAreas)
        {
            var paChildren = new List<ProductAreaChild>();

            foreach (var pa in productAreas)
            {
                pa.Name = Translation.Get(pa.Name, Enums.TranslationSource.TextTranslation);
                if (pa.SubProductAreas.Where(sp=> sp.IsActive != 0 && sp.ShowOnExternalPage != 0).Any())
                {
                    paChildren.Add(new ProductAreaChild(pa.Id, true));
                    var newList = ProductAreaTreeTranslation(pa.SubProductAreas.Where(sp => sp.IsActive != 0 && sp.ShowOnExternalPage != 0).ToList());
                    pa.SubProductAreas = newList.Item1;                    
                    if (newList.Item2.Any())
                        paChildren.AddRange(newList.Item2);
                }
                else                
                    paChildren.Add(new ProductAreaChild(pa.Id, false));                
            }

            var item1 = productAreas.OrderBy(p => p.Name).ToList();
            return new Tuple<IList<ProductArea>, IList<ProductAreaChild>>(item1, paChildren);
        }

        private IList<CaseType> CaseTypeTreeTranslation(IList<CaseType> caseTypes)
        {
            foreach (var ct in caseTypes)
            {
                ct.Name = Translation.Get(ct.Name, Enums.TranslationSource.TextTranslation);
                if (ct.SubCaseTypes.Any())
                {
                    ct.SubCaseTypes = CaseTypeTreeTranslation(ct.SubCaseTypes.ToList());
                }
            }

            return caseTypes.OrderBy(p => p.Name).ToList();
        }

        private IList<CaseSearchResult> CaseSearchTranslate(IList<CaseSearchResult> cases, int customerId)
        {
            var ret = cases;
            var productareaCache = _productAreaService.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => true);
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
                                    var names = _productAreaService.GetParentPath(c.Id, customerId).Select(name => Translation.Get(name,Enums.TranslationSource.TextTranslation));
                                    c.StringValue = string.Join(" - ", names);
                                }

                                break;
                        }
                    }
                }
            }

            return ret;
        }

        public ViewResult AddCommentPopup(int casePreviewId)
        {
        
             return this.View("_AddCommentPopup");
        }
    }
}
