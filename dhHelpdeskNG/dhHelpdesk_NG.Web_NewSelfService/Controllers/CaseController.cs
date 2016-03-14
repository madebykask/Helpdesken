namespace DH.Helpdesk.NewSelfService.Controllers
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
    using DH.Helpdesk.NewSelfService.Infrastructure;
    using DH.Helpdesk.NewSelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.NewSelfService.Infrastructure.Extensions;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools;
    using DH.Helpdesk.NewSelfService.Models;
    using DH.Helpdesk.NewSelfService.Models.Case;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Services.utils;
    using DH.Helpdesk.NewSelfService.Infrastructure;

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
        private readonly IWorkContext workContext;
        private readonly IEmailService _emailService;        
        private readonly IMasterDataService _masterDataService;

        private const string ParentPathDefaultValue = "--";
        private const string EnterMarkup = "<br />";
        private readonly IOrganizationService _orgService;
        private readonly OrganizationJsonService _orgJsonService;


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
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ISupplierService supplierService,
            ICustomerService customerService,
            ISettingService settingService,
            IComputerService computerService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseSearchService caseSearchService,
            IWorkContext workContext, 
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IStateSecondaryService stateSecondaryService,
            ILogFileService logFileService,
            ICaseSolutionService caseSolutionService,
            IOrganizationService orgService,            
            OrganizationJsonService orgJsonService,            
            IEmailService emailService)
            : base(masterDataService, caseSolutionService)
        {
            this._masterDataService = masterDataService;
            this._caseService = caseService;
            this._logService = logService;
            this._caseFieldSettingService = caseFieldSettingService;
            this._infoService = infoService;
            this._userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create("Case");
            this._caseFileService = caseFileService;
            this._regionService = regionService;
            this._departmentService = departmentService;
            this._caseTypeService = caseTypeService;
            this._productAreaService = productAreaService;
            this._currencyService = currencyService;
            this._logFileService = logFileService;
            this._categoryService = categoryService;
            this._supplierService = supplierService;
            this._systemService = systemService;
            this._settingService = settingService;
            this._computerService = computerService;
            this._customerUserService = customerUserService;
            this._customerService = customerService;
            this._caseSettingService = caseSettingService;
            this._caseSearchService = caseSearchService;
            this._workingGroupService = workingGroupService;
            this._userService = userService;
            this._stateSecondaryService = stateSecondaryService;
            this._caseSolutionService = caseSolutionService;
            this.workContext = workContext;
            this._orgService = orgService;
            this._orgJsonService = orgJsonService;
            this._emailService = emailService;            
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
                currentCase = this._caseService.GetCaseById(Int32.Parse(id));

                if (currentCase == null)
                {
                    ErrorGenerator.MakeError("Case not found!");
                    return RedirectToAction("Index", "Error");
                }

                bool userHasAccessToCase = false;
                var currentApplicationType = ConfigurationManager.AppSettings[AppSettingsKey.CurrentApplicationType].ToString().ToLower();
                var curUserId = SessionFacade.CurrentUserIdentity.UserId;
                if (currentApplicationType == ApplicationTypes.LineManager)
                {
                    var coWorkers = SessionFacade.CurrentCoWorkers != null ?
                                        SessionFacade.CurrentCoWorkers.Select(c => c.EmployeeNumber).ToList() : 
                                        new List<string>();

                    var caseListCondition = ConfigurationManager.AppSettings[AppSettingsKey.CaseList].ToString().ToLower().Split(',');
                    if (caseListCondition.Contains(CaseListTypes.ManagerCases))
                    {
                        if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                        {
                            if (currentCase.RegUserId.ToLower() == curUserId.ToLower() || coWorkers.Contains(currentCase.ReportedBy))
                                userHasAccessToCase = true;               
                        }
                        else
                        {
                            if (currentCase.RegUserId.ToLower() == curUserId.ToLower() || 
                                currentCase.ReportedBy == SessionFacade.CurrentUserIdentity.EmployeeNumber)
                                userHasAccessToCase = true;                                                                       
                        }                        
                    }
                    else
                    if (caseListCondition.Contains(CaseListTypes.CoWorkerCases))
                    {
                        if (coWorkers.Contains(currentCase.ReportedBy) || 
                            (string.IsNullOrEmpty(currentCase.ReportedBy) && currentCase.RegUserId.ToLower() == curUserId.ToLower()))
                            userHasAccessToCase = true;                        
                    }                                    
                }
                else
                {
                    if (currentCase.RegUserId.ToLower() == curUserId.ToLower())
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
            var caseReceipt = this.GetCaseReceiptModel(currentCase, languageId);

            caseReceipt.ShowRegistringMessage = showRegistrationMessage;
            caseReceipt.ExLogFileGuid = currentCase.CaseGUID.ToString();
            caseReceipt.MailGuid = id;

            this._userTemporaryFilesStorage.DeleteFiles(caseReceipt.ExLogFileGuid);
            this._userTemporaryFilesStorage.DeleteFiles(id);
                        
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

            return this.View(caseReceipt);
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

            var model = this.GetNewCaseModel(currentCustomer.Id, languageId);
            model.ExLogFileGuid = Guid.NewGuid().ToString();


            if(SessionFacade.CurrentUserIdentity != null)
            {
                var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

                model.NewCase = this._caseService.InitCase(
                    currentCustomer.Id,
                    0,
                    SessionFacade.CurrentLanguageId,
                    this.Request.GetIpAddress(),
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
                var caseTemplate = this._caseSolutionService.GetCaseSolution(caseTemplateId.Value);
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

                    model.NewCase.ReportedBy = caseTemplate.ReportedBy;
                    model.NewCase.Department_Id = caseTemplate.Department_Id;
                    model.NewCase.ProductArea_Id = caseTemplate.ProductArea_Id;
                    model.NewCase.Caption = caseTemplate.Caption;
                    model.NewCase.Description = caseTemplate.Description;
                    model.NewCase.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
                    model.NewCase.Priority_Id = caseTemplate.Priority_Id;
                    model.NewCase.Project_Id = caseTemplate.Project_Id;                    
                }

                if(model.NewCase.ProductArea_Id.HasValue)
                {
                    var p = this._productAreaService.GetProductArea(model.NewCase.ProductArea_Id.GetValueOrDefault());
                    if(p != null)
                    {
                        model.ProductAreaParantPath = string.Join(" - ", this._productAreaService.GetParentPath(p.Id, currentCustomer.Id));
                    }
                }

                if(model.NewCase.CaseType_Id > 0)
                {
                    var c = this._caseTypeService.GetCaseType(model.NewCase.CaseType_Id);
                    if(c != null)
                    {
                        model.CaseTypeParantPath = c.getCaseTypeParentPath();
                    }
                }
            } // Load Case Template

            return this.View("NewCase", model);
        }

        public ActionResult GetDepartmentsByRegion(int? id, int customerId, int departmentFilterFormat)
        {
            var list = this._orgJsonService.GetActiveDepartmentForRegion(id, customerId, departmentFilterFormat);
            return this.Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrgUnitsByDepartments(int? id, int customerId)
        {
            var list = this._orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return this.Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserCases(int customerId, string progressId)
        {
            var currentCustomer = default(Customer);
            if (SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }                

            var languageId = SessionFacade.CurrentLanguageId;

            UserCasesModel model = null;

            if (progressId != CaseProgressFilter.ClosedCases && progressId != CaseProgressFilter.CasesInProgress)
            {
                ErrorGenerator.MakeError("Process is not valid!", 202);
                return RedirectToAction("Index", "Error");                
            }

            if(SessionFacade.CurrentUserIdentity != null)
            {

                model = this.GetUserCasesModel(currentCustomer.Id, languageId, SessionFacade.CurrentUserIdentity.UserId, "", 20, progressId);

            }
            else
            {
                ErrorGenerator.MakeError("You don't have access to cases, please login again.", 203);
                return RedirectToAction("Index", "Error");                
            }

            return this.View("CaseOverview", model);
        }

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
               fileContent = this._userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = this._masterDataService.GetFilePath(c.Customer_Id);

                fileContent = this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadNewCaseFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent =  this._userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = this._masterDataService.GetFilePath(c.Customer_Id);

                fileContent = this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }
            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(this._userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        this._userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void NewCaseUploadFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(this._userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        this._userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void DeleteFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var log = this._logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = this._caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpPost]
        public void DeleteNewCaseFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpGet]
        public JsonResult Files(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? this._userTemporaryFilesStorage.GetFileNames(id)
                                : this._logFileService.FindFileNamesByLogId(int.Parse(id));

            return this.Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult NewCaseFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? this._userTemporaryFilesStorage.GetFileNames(id)
                                : this._caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return this.Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult DownloadLogFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent = this._userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log);
            else
            {
                var log = this._logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = this._caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                } 

                fileContent = this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ActionResult _CaseLogNote(int caseId, string note, string logFileGuid)
        {
            SaveExternalMessage(caseId, note, logFileGuid);            
            CaseLogModel model = new CaseLogModel()
                  {
                      CaseId = caseId,
                      CaseLogs = this._logService.GetLogsByCaseId(caseId).OrderByDescending(l=> l.RegTime).ToList()                      
                  };            
            return this.PartialView(model);
        }

        private void SaveExternalMessage(int caseId, string extraNote, string logFileGuid) 
        {
            IDictionary<string, string> errors;            
            var currentCase = _caseService.GetCaseById(caseId);
            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);            
            // save case history            
            int caseHistoryId = this._caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, out errors, SessionFacade.CurrentUserIdentity.UserId);            
            // save log
            var caseLog = new CaseLog
                              {
                                  CaseHistoryId = caseHistoryId,
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  TextExternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseExternalLogNote? extraNote : string.Empty),
                                  UserId = null,
                                  TextInternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseInternalLogNote? extraNote : string.Empty),
                                  WorkingTimeHour = 0,
                                  WorkingTimeMinute = 0,
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
                temporaryLogFiles = this._userTemporaryFilesStorage.GetFiles(logFileGuid, ModuleName.Log);
                caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                var basePath = this._masterDataService.GetFilePath(currentCase.Customer_Id);
                // save log files
                var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
                this._logFileService.AddFiles(newLogFiles);
                // send emails
               
                this._caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, basePath, newLogFiles);
                this._userTemporaryFilesStorage.DeleteFiles(logFileGuid);
            }
            else
            {
                caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                this._caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, string.Empty, null);
            }
        }

        [HttpPost]
        public RedirectToRouteResult NewCase(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
        {
            int caseId = Save(newCase, caseMailSetting, caseFileKey);
            return this.RedirectToAction("Index", "case", new { id = newCase.Id, showRegistrationMessage = true });
        }

        [HttpPost]
        public ActionResult SearchUser(string query, int customerId)
        {
            var result = this._computerService.SearchComputerUsers(customerId, query);
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult SearchComputer(string query, int customerId)
        {
            var result = this._computerService.SearchComputer(customerId, query);
            return this.Json(result);
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
                var ascending = frm.ReturnFormValue("hidSortByAsc").convertStringToBool();
                var id = frm.ReturnFormValue("MailGuid");

                if (progressId != CaseProgressFilter.ClosedCases && progressId != CaseProgressFilter.CasesInProgress)
                {                    
                    ErrorGenerator.MakeError("Process is not valid!", 202);
                    return RedirectToAction("Index", "Error");                                 
                }

                var model = GetUserCasesModel(customerId, languageId, userId,
                                              pharasSearch, maxRecords, progressId,
                                              sortBy, ascending);

                return this.PartialView("CaseOverview", model);
            }
            catch(Exception e)
            {
                ErrorGenerator.MakeError("Not able to load user cases.", 204);
                return RedirectToAction("Index", "Error");                
            }
        }

        public List<CaseSolution> GetCaseSolutions(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t => t.ShowInSelfService).ToList();
        }   

        [HttpPost]
        public void UploadLogFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this._userTemporaryFilesStorage.FileExists(name, id, ModuleName.Log))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                this._userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Log);
            }
        }

        [HttpGet]
        public JsonResult GetLogFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? this._userTemporaryFilesStorage.GetFileNames(id, ModuleName.Log)
                                : this._logFileService.FindFileNamesByLogId(int.Parse(id));

            return this.Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);            
        }

        private int Save(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
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

            var basePath = this._masterDataService.GetFilePath(newCase.Customer_Id);

            int caseHistoryId = this._caseService.SaveCase(newCase, null, caseMailSetting, 0, SessionFacade.CurrentUserIdentity.UserId, out errors);
            
            // save case files            
            var temporaryFiles = this._userTemporaryFilesStorage.GetFiles(caseFileKey, ModuleName.Cases);
            var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, newCase.Id)).ToList();
            this._caseFileService.AddFiles(newCaseFiles);

            // delete temp folders                
            this._userTemporaryFilesStorage.DeleteFiles(caseFileKey);
            var oldCase = new Case();            
            // send emails
            this._caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath, oldCase);

            return newCase.Id;
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

            var regions = _regionService.GetRegions(currentCase.Customer_Id);
            var suppliers = _supplierService.GetSuppliers(currentCase.Customer_Id);
            var systems = _systemService.GetSystems(currentCase.Customer_Id);
            if (currentCase.CaseType != null)
                currentCase.CaseType.Name = _caseTypeService.GetCaseTypeFullName(currentCase.CaseType_Id);

            if (currentCase.ProductArea_Id.HasValue && currentCase.ProductArea != null)
                currentCase.ProductArea.Name = string.Join(" - ",  _productAreaService.GetParentPath(currentCase.ProductArea_Id.Value, currentCase.Customer_Id));

            var newLogFile = new FilesModel();

            CaseOverviewModel model = null;

            if(currentCase != null)
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).OrderByDescending(l => l.LogDate).ToList();

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
                    LogFileGuid = Guid.NewGuid().ToString()
                };
            }
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

            if (currentApplicationType == ApplicationTypes.LineManager)
            {
                var caseListCondition = ConfigurationManager.AppSettings[AppSettingsKey.CaseList].ToString().ToLower().Split(',');

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
                cs.RegUserId = curUser;
                cs.CaseListType = CaseListTypes.UserCases;
            }


            cs.ReportedBy = cs.ReportedBy;

            search.SortBy = sortBy;
            search.Ascending = ascending;

            sm.Search = search;
            sm.caseSearchFilter = cs;

            // 1: User in Customer Setting
            srm.CaseSettings = this._caseSettingService.GetCaseSettingsByUserGroup(cusId, 1);

            if (caseListType == string.Empty && currentApplicationType == ApplicationTypes.LineManager)
                srm.Cases = null;
            else
            {
                var workTimeCalc = TimeZoneInfo.Local;
                var showRemainingTime = false;
                CaseRemainingTimeData remainingTimeData;
                CaseAggregateData aggregateData;
                var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(cusId).ToArray();
                srm.Cases = this._caseSearchService.Search(
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
                    out aggregateData).ToList(); // Take(maxRecords)

                if (currentApplicationType == ApplicationTypes.LineManager)
                {
                    var dynamicCases = _caseService.GetAllDynamicCases();
                    model.DynamicCases = dynamicCases;
                }

            }
            
            model.CaseSearchResult = srm;
            SessionFacade.CurrentCaseSearch = sm;

            return model;
        }

        private NewCaseModel GetNewCaseModel(int customerId, int languageId)
        {
            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId)
                                                           .Where(c => c.ShowExternal == 1)
                                                           .ToList();

            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);

            var newCase = new Case { Customer_Id = customerId };
            var caseFile = new FilesModel { Id = Guid.NewGuid().ToString() };

            //Region list            
            var regions = this._regionService.GetRegions(customerId);

            //Department list
            var departments = this._departmentService.GetDepartments(customerId);

            //Organization unit list
            var orgUnits = this._orgService.GetOUs(customerId).ToList();

            //Case Type tree            
            var caseTypes = this._caseTypeService.GetCaseTypes(customerId).Where(c=> c.ShowOnExternalPage != 0).ToList();

            //Product Area tree            
            var productAreas = this._productAreaService.GetTopProductAreas(customerId).Where(p=> p.ShowOnExternalPage != 0).ToList();

            //System list            
            var systems = this._systemService.GetSystems(customerId);

            //Category List
            var categories = this._categoryService.GetCategories(customerId);

            //Currency List
            var currencies = this._currencyService.GetCurrencies();

            //Country list
            var suppliers = this._supplierService.GetSuppliers(customerId);

            //Field Settings
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            var cs = this._settingService.GetCustomerSetting(customerId);
            
            var model = new NewCaseModel(
                newCase, 
                regions, 
                departments,
                orgUnits,
                caseTypes, 
                productAreas, 
                systems,
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
                    });

            model.CaseTypeParantPath = "--";
            model.ProductAreaParantPath = "--";
            model.CaseFileKey = Guid.NewGuid().ToString();

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
                            GlobalEnums.TranslationCaseFields.UserCode.ToString()
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
                            GlobalEnums.TranslationCaseFields.Filename.ToString()                                                                                
                        };

            string[] otherGroup = new string[] 
                        { 
                            GlobalEnums.TranslationCaseFields.PlanDate.ToString(),
                            GlobalEnums.TranslationCaseFields.WatchDate.ToString(),
                            GlobalEnums.TranslationCaseFields.Verified.ToString(),
                            GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                            GlobalEnums.TranslationCaseFields.SolutionRate.ToString(),
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

    }
}
