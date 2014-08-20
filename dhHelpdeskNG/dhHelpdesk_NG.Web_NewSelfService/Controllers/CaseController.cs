using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.NewSelfService.Models.Case;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Dal.Enums;
using System.Net;


namespace DH.Helpdesk.NewSelfService.Controllers
{
    using System.Net;
    using System.Web;
    using System.Web.WebPages;
    using System.Configuration;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.Infrastructure;
    using DH.Helpdesk.NewSelfService.Infrastructure.Extensions;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.NewSelfService.Models;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net.Http.Headers;

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


        private const string ParentPathDefaultValue = "--";


        public CaseController(ICaseService caseService,
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
                              IUserService userService,
                              IWorkingGroupService workingGroupService,
                              IStateSecondaryService stateSecondaryService,
                              ILogFileService logFileService,
                              ICaseSolutionService caseSolutionService,
                              ISSOService ssoService)
            : base(masterDataService, ssoService, caseSolutionService)
        {
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
        }


        [HttpGet]
        public ActionResult Index(string id)
        {
            if(string.IsNullOrEmpty(id))
                return null;

            int customerId;

            Case currentCase = null;

            if(id.Is<Guid>())
            {
                var guid = new Guid(id);
                currentCase = _caseService.GetCaseByEMailGUID(guid);
            }
            else
            {
                currentCase = this._caseService.GetCaseById(Int32.Parse(id));

                if(currentCase == null)
                    return RedirectToAction("Index", "Error", new { message = "Can not find the Case!" });

                if(currentCase != null && currentCase.RegUserId != SessionFacade.CurrentUserIdentity.UserId)
                    return RedirectToAction("Index", "Error", new { message = "Can not find this Case in your cases!" }); ;

            }

            if(currentCase == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Can not find the Case!" });
            }
            else
            {
                if(currentCase.FinishingDate == null)
                {
                    ViewBag.CurrentCaseId = currentCase.Id;
                }

                currentCase.Description = currentCase.Description.Replace("\n", "<br />");
                customerId = currentCase.Customer_Id;
            }

            Customer currentCustomer = default(Customer);

            if(SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
                return RedirectToAction("Index", "Error", new { message = "Customer Id: " + customerId.ToString() + " is not valid!", errorCode = 108 });

            currentCase.Customer = currentCustomer;


            var languageId = SessionFacade.CurrentLanguageId;
            var caseOverview = this.GetCaseOverviewModel(currentCase, languageId);

            caseOverview.ExLogFileGuid = currentCase.CaseGUID.ToString();
            caseOverview.MailGuid = id;

            this._userTemporaryFilesStorage.DeleteFiles(caseOverview.ExLogFileGuid);
            this._userTemporaryFilesStorage.DeleteFiles(id);

            if(id.Is<Guid>())
            {
                if(currentCase.StateSecondary_Id.HasValue && caseOverview.CasePreview.FinishingDate == null)
                {
                    var stateSecondary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);
                    if(stateSecondary.NoMailToNotifier == 1)
                        caseOverview.CasePreview.FinishingDate = DateTime.UtcNow; //model.CaseOverview.CasePreview.ChangeTime;
                }
                caseOverview.CanAddExternalNote = true;
            }
            else
            {
                caseOverview.ReceiptFooterMessage = currentCustomer.RegistrationMessage;
                caseOverview.CanAddExternalNote = false;
            }

            return this.View(caseOverview);
        }


        [HttpGet]
        public ActionResult NewCase(int customerId, int? caseTemplateId)
        {
            // *** New Case ***
            var currentCustomer = default(Customer);
            if(SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
                return RedirectToAction("Index", "Error", new { message = "Customer Id: " + customerId.ToString() + " is not valid!", errorCode = 208 });

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
                    GlobalEnums.RegistrationSource.Case,
                    cs, SessionFacade.CurrentUserIdentity.UserId);

                model.NewCase.Customer = currentCustomer;
                model.CaseMailSetting = new CaseMailSetting(
                    currentCustomer.NewCaseEmailList,
                    currentCustomer.HelpdeskEmail,
                    ConfigurationManager.AppSettings["dh_helpdeskaddress"].ToString(),
                    cs.DontConnectUserToWorkingGroup);
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
                    //m.CaseLog.TextExternal = caseTemplate.Text_External;
                    //m.CaseLog.TextInternal = caseTemplate.Text_Internal;
                    //m.CaseLog.FinishingType = caseTemplate.FinishingCause_Id;
                }

                if(model.NewCase.ProductArea_Id.HasValue)
                {
                    var p = this._productAreaService.GetProductArea(model.NewCase.ProductArea_Id.GetValueOrDefault());
                    if(p != null)
                    {
                        model.ProductAreaParantPath = p.getProductAreaParentPath();
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

        [HttpGet]
        public ActionResult UserCases(int customerId, string progressId)
        {

            var currentCustomer = default(Customer);
            if(SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
                return RedirectToAction("Index", "Error", new { message = "Customer Id: " + customerId.ToString() + " is not valid!", errorCode = 208 });

            var languageId = SessionFacade.CurrentLanguageId;

            UserCasesModel model = null;

            if(progressId != "1" && progressId != "2")
                return RedirectToAction("Index", "Error", new { message = "Process is not valid!", errorCode = 207 });

            if(SessionFacade.CurrentUserIdentity != null)
            {

                model = this.GetUserCasesModel(currentCustomer.Id, languageId, SessionFacade.CurrentUserIdentity.UserId, "", 20, progressId);

            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "You don't have access to user cases! please login again.", errorCode = 204 });
            }

            return this.View("UserCases", model);
        }

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this._userTemporaryFilesStorage.GetFileContent(fileName, id, "")
                                  : this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);
            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadNewCaseFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this._userTemporaryFilesStorage.GetFileContent(fileName, id, "")
                                  : this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);
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
                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), fileName.Trim());
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
                this._caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), fileName.Trim());

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
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this._userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log)
                                  : this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public RedirectToRouteResult SaveExternalMessage(int caseId, string extraNote) // , string curGUID)
        {
            IDictionary<string, string> errors;

            var currentCase = _caseService.GetCaseById(caseId);
            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

            // save case history
            // may be need change PersonsEmail
            int caseHistoryId = this._caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, out errors, currentCase.RegUserId);

            //var guid = new Guid(curGUID);
            //var emailLog = _caseService.GetEMailLogByGUID(guid);

            // save log
            var caseLog = new CaseLog
                              {
                                  CaseHistoryId = caseHistoryId,
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  TextExternal = "",
                                  UserId = null,
                                  TextInternal = extraNote,
                                  WorkingTimeHour = 0,
                                  WorkingTimeMinute = 0,
                                  EquipmentPrice = 0,
                                  Price = 0,
                                  Charge = false,
                                  RegUser = SessionFacade.CurrentSystemUser, //emailLog.EmailAddress,
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


                    //var emails = _userService.GetUsersForWorkingGroup(currentCase.WorkingGroup_Id.Value).Where(u => u.Email.Trim() != string.Empty).Select(u => u.Email).ToList();
                    //if(emails != null && emails.Count > 0)
                    //{
                    //    caseLog.EmailRecepientsExternalLog = string.Join(Environment.NewLine, emails);
                    //}

                    //if(currentCase.Performer_User_Id != 0)
                    //{
                    //    var userEmail = _userService.GetUser(currentCase.Performer_User_Id).Email;
                    //    caseLog.EmailRecepientsExternalLog = userEmail;
                    //}
                }
                //else // Send Mail to Working Group Email  
                //{
                //    caseLog.EmailRecepientsExternalLog = curWorkingGroup.EMail;
                //}
            }

            var temporaryLogFiles = this._userTemporaryFilesStorage.GetFiles(currentCase.CaseGUID.ToString(), "");
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            // send emails
            var caseMailSetting = new CaseMailSetting(
                                                       currentCustomer.NewCaseEmailList,
                                                       currentCustomer.HelpdeskEmail,
                                                       ConfigurationManager.AppSettings["dh_helpdeskaddress"].ToString(),
                                                       cs.DontConnectUserToWorkingGroup
                                                       );

            this._caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, newLogFiles);

            return RedirectToAction("Index", new { id = caseId });
        }

        [HttpPost]
        public RedirectToRouteResult NewCase(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
        {
            int caseId = Save(newCase, caseMailSetting, caseFileKey);
            return this.RedirectToAction("Index", "case", new { id = newCase.Id });
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

        public ActionResult SeachUserCase(FormCollection frm)
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

                if(progressId != "1" && progressId != "2")
                    return RedirectToAction("Index", "Error", new { message = "Process is not valid!", errorCode = 207 });

                var model = GetUserCasesModel(customerId, languageId, userId,
                                              pharasSearch, maxRecords, progressId,
                                              sortBy, ascending);

                return this.PartialView("UserCases", model);
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new { message = "Error in load user cases!", errorCode = 206 });
                //return View("Error");
            }
        }

        public List<CaseSolution> GetCaseSolutions(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t => t.ShowInSelfService).ToList();
        }

        private int Save(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
        {
            IDictionary<string, string> errors;

            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(newCase, null, caseMailSetting, 0, SessionFacade.CurrentUserIdentity.UserId, out errors);

            // save case files            
            var temporaryFiles = this._userTemporaryFilesStorage.GetFiles(caseFileKey, ModuleName.Cases);
            var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, newCase.Id)).ToList();
            this._caseFileService.AddFiles(newCaseFiles);

            // delete temp folders                
            this._userTemporaryFilesStorage.DeleteFiles(caseFileKey);

            // send emails
            this._caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId);

            return newCase.Id;
        }

        private CaseOverviewModel GetCaseOverviewModel(Case currentCase, int languageId)
        {
            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 ||
                                                                       c.Name == "tblLog.Text_External" ||
                                                                       c.Name == "CaseNumber" ||
                                                                       c.Name == "RegTime")
                                                           .ToList();

            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);
            // 6 is id of SelfService Info Text
            var infoText = _infoService.GetInfoText(6, currentCase.Customer_Id, languageId);

            var regions = _regionService.GetRegions(currentCase.Customer_Id);
            var suppliers = _supplierService.GetSuppliers(currentCase.Customer_Id);
            var systems = _systemService.GetSystems(currentCase.Customer_Id);

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
                    Systems = systems
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
            var caseListCondition = ConfigurationManager.AppSettings["CaseList"].ToString().ToLower().Split(',');


            var LMtype = "";
            if(caseListCondition.Contains("manager"))
            {
                LMtype = "1";
                cs.RegUserId = curUser;
                cs.ReportedBy = "'" + SessionFacade.CurrentUserIdentity.EmployeeNumber + "'";
            }

            if(caseListCondition.Contains("coworkers"))
            {
                LMtype = LMtype + "2";
                var employees = SessionFacade.CurrentCoWorkers;
                foreach(var emp in employees)
                {
                    if(emp.EmployeeNumber != "")
                    {
                        if(cs.ReportedBy == "")
                            cs.ReportedBy = "'" + emp.EmployeeNumber + "'";
                        else
                            cs.ReportedBy = cs.ReportedBy + "," + "'" + emp.EmployeeNumber + "'";
                    }
                }

            }


            //if (cs.ReportedBy == "")
            //  cs.ReportedBy = "-1";
            cs.LMCaseList = LMtype;

            cs.ReportedBy = cs.ReportedBy;

            search.SortBy = sortBy;
            search.Ascending = ascending;

            sm.Search = search;
            sm.caseSearchFilter = cs;

            // 1: User in Customer Setting
            srm.CaseSettings = this._caseSettingService.GetCaseSettingsByUserGroup(cusId, 1);

            if(LMtype == "")
                srm.Cases = null;
            else
            {
                srm.Cases = this._caseSearchService.Search(
                    sm.caseSearchFilter,
                    srm.CaseSettings,
                    -1,
                    curUser,
                    1,
                    1,
                    1,
                    search,
                    1,
                    1,
                    null,
                    "Line Manager").ToList(); // Take(maxRecords)

                var dynamicCases = _caseService.GetAllDynamicCases();
                model.DynamicCases = dynamicCases;

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

            //Case Type tree            
            var caseTypes = this._caseTypeService.GetCaseTypes(customerId);

            //Product Area tree            
            var productAreas = this._productAreaService.GetProductAreas(customerId);

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

            var model = new NewCaseModel(newCase, regions, departments, caseTypes, productAreas, systems,
                                         categories, currencies, suppliers, caseFieldGroups, caseFieldSetting, caseFile, caseFieldSettings);

            model.CaseTypeParantPath = "--";
            model.ProductAreaParantPath = "--";
            model.CaseFileKey = Guid.NewGuid().ToString();

            return model;
        }

        private List<string> GetVisibleFieldGroups(List<CaseListToCase> fieldList)
        {
            List<string> ret = new List<string>();

            string[] userInformationGroup = new string[] {"ReportedBy", "Persons_Name", "Persons_EMail",
                                                           "Persons_Phone", "Persons_CellPhone", "Customer_Id", "Region_Id",
                                                           "Department_Id" , "OU_Id", "Place", "UserCode"};

            string[] computerInformationGroup = new string[] { "InventoryNumber", "ComputerType_Id", "InventoryLocation" };

            string[] caseInfoGroup = new string[] { "CaseNumber", "RegTime", "ChangeTime",  "CaseType_Id", "ProductArea_Id", "System_Id", "Urgency_Id", "Impact_Id",
                                                    "Category_Id", "Supplier_Id" , "InvoiceNumber", "ReferenceNumber", "Caption", "Description" ,
                                                    "Miscellaneous", "ContactBeforeAction", "SMS", "AgreedDate" ,  "Available" , "Cost" ,"Filename"};

            string[] otherGroup = new string[] { "PlanDate", "WatchDate", "Verified", "VerifiedDescription", "SolutionRate" };

            string[] caseLogGroup = new string[] { "tblLog.Text_External", "tblLog.Filename", "FinishingDescription", "FinishingDate" };

            foreach(var field in fieldList)
            {
                if(userInformationGroup.Contains(field.Name))
                    ret.Add("UserInformation");

                if(computerInformationGroup.Contains(field.Name))
                    ret.Add("ComputerInformation");

                if(caseInfoGroup.Contains(field.Name))
                    ret.Add("CaseInfo");

                if(otherGroup.Contains(field.Name))
                    ret.Add("Other");

                if(caseLogGroup.Contains(field.Name))
                    ret.Add("CaseLog");
            }

            return ret;
        }


    }
}
