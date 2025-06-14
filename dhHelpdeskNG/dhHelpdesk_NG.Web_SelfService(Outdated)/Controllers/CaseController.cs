﻿using System;

using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.BusinessData.Models.SelfService.Case;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Common;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;

using DH.Helpdesk.Dal.Enums;

namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Diagnostics.Eventing.Reader;
    using System.EnterpriseServices;
    using System.Net;
    using System.Web.WebPages;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.Extensions;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.Common.Tools;
    using System.Web;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.SelfService.Models;

    using Microsoft.SqlServer.Server;
    using DH.Helpdesk.SelfService.Models.SelfService;
    using System.Configuration;
    using DH.Helpdesk.Common.Enums;

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
        private readonly IMasterDataService _masterDataService;                


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
                              ILogFileService logFileService
                             )
                              : base(masterDataService)
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
        }

        [HttpGet]
        public ActionResult Index(int customerId = 0, string id="", int languageId = 0)
        {
            // just for first version 
            if (!id.Is<Guid>())
            {
                return null;
            }

            Case currentCase = null;
            var config = new SelfServiceConfigurationModel
            {
                ShowBulletinBoard = false,
                ShowDashboard = false,
                ShowFAQ = false,
                ShowOrderAccount = false,
                ShowNewCase = false,
                ShowUserCases = false,
                ViewCaseMode = 0,
                IsReceipt = true
            };

            if (id != string.Empty)
            {
                if (id.Is<Guid>())
                {
                    var guid = new Guid(id);
                    currentCase = _caseService.GetCaseByEMailGUID(guid);                    
                    config.ViewCaseMode = 0;
                    config.IsReceipt = false;
                }
                else
                {
                    currentCase = this._caseService.GetCaseById(Int32.Parse(id));                    
                    config.ViewCaseMode = 1;                    
                }

                if (currentCase != null)
                    customerId = currentCase.Customer_Id;

                this._userTemporaryFilesStorage.DeleteFiles(id);
            }

            if (customerId == 0)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);                
            }
            
            var currentCustomer = this._customerService.GetCustomer(customerId);

            if (currentCustomer == null)
                return null;
            else
            {
                if (languageId == 0)
                    languageId = currentCustomer.Language_Id;

                var model = new SelfServiceModel(customerId, languageId);

                SessionFacade.CurrentCustomer = currentCustomer;
                SessionFacade.CurrentLanguageId = languageId;

                config.ShowBulletinBoard = currentCustomer.ShowBulletinBoardOnExtPage.convertIntToBool();
                config.ShowDashboard = currentCustomer.ShowDashboardOnExternalPage.convertIntToBool();
                config.ShowFAQ = currentCustomer.ShowFAQOnExternalPage.convertIntToBool();

                model.IsEmptyCase = 1;
                model.ExLogFileGuid = Guid.NewGuid().ToString();

                if (id != string.Empty)
                {
                    if (currentCase != null)
                    {
                        model.IsEmptyCase = 0;
                        var caseOverview = this.GetCaseOverviewModel(currentCase, languageId);
                        caseOverview.CasePreview.Description = caseOverview.CasePreview.Description.Replace("\n", "<br />");
                        model.CaseOverview = caseOverview;
                        
                        if (currentCase.StateSecondary_Id.HasValue && model.CaseOverview.CasePreview.FinishingDate == null)
                        {
                            var stateSecondary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);
                            if (stateSecondary.NoMailToNotifier == 1)
                                model.CaseOverview.CasePreview.FinishingDate = DateTime.UtcNow; //model.CaseOverview.CasePreview.ChangeTime;
                        }
                        model.ExLogFileGuid = currentCase.CaseGUID.ToString();
                        if (config.IsReceipt)
                        {
                            //model.CaseOverview.InfoText = Translation.Get("Tack", Enums.TranslationSource.TextTranslation);
                            model.CaseOverview.ReceiptFooterMessage = currentCustomer.RegistrationMessage;
                        }
                    }
                }

                this._userTemporaryFilesStorage.DeleteFiles(model.ExLogFileGuid);

                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (identity != null)
                {
                    // *** New Case *** 
                    var newCase = this.GetNewCaseModel(currentCustomer.Id, languageId);
                    var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

                    newCase.NewCase = this._caseService.InitCase(
                        currentCustomer.Id,
                        0,
                        SessionFacade.CurrentLanguageId,
                        this.Request.GetIpAddress(),
                        CaseRegistrationSource.SelfService,
                        cs,
                        identity.Name);

                    newCase.NewCase.Customer = currentCustomer;
                    newCase.CaseMailSetting = new CaseMailSetting(
                        currentCustomer.NewCaseEmailList,
                        currentCustomer.HelpdeskEmail,
                        ConfigurationManager.AppSettings["dh_helpdeskaddress"].ToString(),
                        cs.DontConnectUserToWorkingGroup);
                    model.NewCase = newCase;

                    // *** User Cases *** 
                    string adUser = identity.Name;
                    string regUser = adUser.GetUserFromAdPath();
                    if (regUser != string.Empty)
                    {
                        model.AUser = regUser;
                        if (id.Is<Guid>())
                        {
                            model.MailGuid = id.ToString();
                        }
                        else
                        {
                            model.UserCases = this.GetUserCasesModel(currentCustomer.Id, languageId, regUser, "", 20);
                            if (!string.IsNullOrEmpty(id) && model.CaseOverview != null)
                                if (model.CaseOverview.CasePreview.RegUserId != regUser)
                                {
                                    model.IsEmptyCase = 1;
                                    model.CaseOverview.CasePreview = null;
                                }
                            config.ShowNewCase = true;
                            config.ShowUserCases = true;
                            config.ViewCaseMode = 1;
                        }
                    }
                    else
                    {
                        model.AUser = "";
                        model.UserCases = null;
                        config.ViewCaseMode = 0;
                    }
                }

                model.Configuration = config;
                return this.View(model);
            } // if exist Customer

            
            
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

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            if (uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if (GuidHelper.IsGuid(id))
                {
                    if (this._userTemporaryFilesStorage.FileExists(name, id))
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
            if (uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if (GuidHelper.IsGuid(id))
                {
                    if (this._userTemporaryFilesStorage.FileExists(name, id))
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
            if (GuidHelper.IsGuid(id))
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
            if (GuidHelper.IsGuid(id))
            {
                this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = this._masterDataService.GetFilePath(c.Customer_Id);

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
        public RedirectToRouteResult SendMail(int caseId, int languageId, string extraNote, string curGUID)
        {
            IDictionary<string, string> errors;

            var currentCase = _caseService.GetCaseById(caseId);
            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

            // save case history
            // may be need change PersonsEmail
            int caseHistoryId = this._caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, out errors, currentCase.RegUserId); 

            var guid = new Guid(curGUID);
            var emailLog = _caseService.GetEMailLogByGUID(guid);

            // save log
            var caseLog = new CaseLog
                              {
                                  CaseHistoryId = caseHistoryId,
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  TextExternal = extraNote,
                                  UserId = null,
                                  TextInternal = "",
                                  WorkingTimeHour = 0,
                                  WorkingTimeMinute = 0,
                                  EquipmentPrice = 0,
                                  Price = 0,
                                  Charge = false,
                                  RegUser = emailLog.EmailAddress,
                                  SendMailAboutCaseToNotifier = true,
                                  SendMailAboutLog = true
                              };
            

            if (currentCase.WorkingGroup_Id != null)
            {
                var curWorkingGroup = _workingGroupService.GetWorkingGroup(currentCase.WorkingGroup_Id.Value);
                if (curWorkingGroup.SendExternalEmailToWGUsers.HasValue && curWorkingGroup.SendExternalEmailToWGUsers.Value == 1) // Send To Users Working Group EMail
                {
                    if (currentCase.Performer_User_Id == 0) // Send Mail to all working Group users
                    {
                        var emails = _userService.GetUsersForWorkingGroup(currentCase.Customer_Id, currentCase.WorkingGroup_Id.Value).Select(u => u.Email).ToList();
                        if (emails != null && emails.Count > 0)
                        {
                            caseLog.EmailRecepientsExternalLog = string.Join(Environment.NewLine, emails);
                        }
                    }
                    else
                    {
                        if (currentCase.Performer_User_Id.HasValue)
                        {
                            var userEmail = _userService.GetUser(currentCase.Performer_User_Id.Value).Email;
                            caseLog.EmailRecepientsExternalLog = userEmail;
                        }
                    }
                }
                else // Send Mail to Working Group Email  
                {
                    caseLog.EmailRecepientsExternalLog = curWorkingGroup.EMail;
                }                
            }            
                
            
            var temporaryLogFiles = this._userTemporaryFilesStorage.GetFiles(currentCase.CaseGUID.ToString(), "");                        
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);

            var basePath = this._masterDataService.GetFilePath(currentCase.Customer_Id);
            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            // send emails
            var caseMailSetting = new CaseMailSetting(            
                                                       currentCustomer.NewCaseEmailList,
                                                       currentCustomer.HelpdeskEmail,
                                                       ConfigurationManager.AppSettings["dh_helpdeskaddress"].ToString(),
                                                       cs.DontConnectUserToWorkingGroup
                                                       );

            this._caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, basePath, newLogFiles);

            return RedirectToAction("Index", new { id = curGUID });
        }

        [HttpPost]
        public RedirectToRouteResult NewCase(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
        {            
            int caseId = Save(newCase, caseMailSetting, caseFileKey);
            return this.RedirectToAction("Index", "case", new {id = newCase.Id});
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

                var model = GetUserCasesModel(customerId, languageId, userId,
                                              pharasSearch, maxRecords, progressId,
                                              sortBy, ascending);

                return this.PartialView("_UserCases", model);
            }
            catch (Exception e)
            {
                throw new HttpException((int)HttpStatusCode.PartialContent, e.Message);                
            }
        }

        private int Save(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey)
        {           
            IDictionary<string, string> errors;
            
            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(newCase, null, caseMailSetting, 0, this.User.Identity.Name, out errors);

            var basePath = this._masterDataService.GetFilePath(newCase.Customer_Id);

            // save case files            
            var temporaryFiles = this._userTemporaryFilesStorage.GetFiles(caseFileKey, ModuleName.Cases);
            var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, newCase.Id)).ToList();
            this._caseFileService.AddFiles(newCaseFiles);
            
            // delete temp folders                
            this._userTemporaryFilesStorage.DeleteFiles(caseFileKey);

            
            // send emails
            this._caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath);

            return newCase.Id;
        }

        private CaseOverviewModel GetCaseOverviewModel(Case currentCase, int languageId)
        {            

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 || 
                                                                       c.Name == "tblLog.Text_External" ||                                                                        
                                                                       c.Name == "CaseNumber"  ||
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
            
            if (currentCase != null) 
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).OrderByDescending(l=> l.LogDate).ToList();
                
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
            if (string.IsNullOrEmpty(progressId)) progressId = "1,2";
                        
            var cusId = customerId;
       
                
            var model = new UserCasesModel
                    {
                        CustomerId = cusId,
                        LanguageId = languageId,
                        UserId = curUser,
                        MaxRecords = maxRecords,
                        PharasSearch = pharasSearch
                    };
            
            var srm = new CaseSearchResultModel();
            var sm = new CaseSearchModel();
            var cs = new CaseSearchFilter();
            var search = new Search();

            if (string.IsNullOrEmpty(sortBy)) sortBy = "Casenumber";
            
            cs.CustomerId = cusId;            
            cs.FreeTextSearch = pharasSearch;
            cs.CaseProgress = progressId;
            
            search.SortBy = sortBy;
            search.Ascending = ascending;

            sm.Search = search;
            sm.caseSearchFilter = cs;

            // 1: User in Customer Setting
            srm.CaseSettings = this._caseSettingService.GetCaseSettingsByUserGroup(cusId, 1);
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
                1,
                1,
                null,
                ApplicationTypes.SelfService).Take(maxRecords).ToList();
            
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
            var productAreas = this._productAreaService.GetTopProductAreas(customerId);

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
         

            string[] userInformationGroup = new string [] {"ReportedBy", "Persons_Name", "Persons_EMail",
                                                           "Persons_Phone", "Persons_CellPhone", "Customer_Id", "Region_Id",
                                                           "Department_Id" , "OU_Id", "Place", "UserCode"};

            string[] computerInformationGroup = new string[] { "InventoryNumber", "ComputerType_Id", "InventoryLocation" };

            string[] caseInfoGroup = new string[] { "CaseNumber", "RegTime", "ChangeTime",  "CaseType_Id", "ProductArea_Id", "System_Id", "Urgency_Id", "Impact_Id",
                                                    "Category_Id", "Supplier_Id" , "InvoiceNumber", "ReferenceNumber", "Caption", "Description" ,
                                                    "Miscellaneous", "ContactBeforeAction", "SMS", "AgreedDate" ,  "Available" , "Cost" ,"Filename"};

            string[] otherGroup = new string[] { "PlanDate", "WatchDate", "Verified", "VerifiedDescription", "SolutionRate" };

            string[] caseLogGroup = new string[] { "tblLog.Text_External", "tblLog.Filename", "FinishingDescription", "FinishingDate" };

            foreach (var field in fieldList)
            {
                if (userInformationGroup.Contains(field.Name))                                    
                    ret.Add("UserInformation");
                

                if (computerInformationGroup.Contains(field.Name))                                    
                    ret.Add("ComputerInformation");                

                if (caseInfoGroup.Contains(field.Name))                                    
                    ret.Add("CaseInfo");                

                if (otherGroup.Contains(field.Name))                
                    ret.Add("Other");               

                if (caseLogGroup.Contains(field.Name))                
                    ret.Add("CaseLog");                         
            }
            
            return ret;
        }

    }
}
