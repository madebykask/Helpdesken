using System;
using Ninject;
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
    using System.Net;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.Common.Tools;
    using System.Web;
       
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
                              ILogFileService logFileService
                             )
                              : base(masterDataService)
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
            this._customerService = customerService;
        }

        [HttpGet]
        public ActionResult Index(string id, int languageId = 1)        
        {
            var guid = new Guid(id);
            SessionFacade.CurrentLanguageId = languageId;
            this._userTemporaryFilesStorage.DeleteFiles(id);
            
            var currentCase = _caseService.GetCaseByGUID(guid);

            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            SessionFacade.CurrentCustomer = currentCustomer;

            var model = new SelfServiceModel(languageId);            
            var caseOverview = this.GetCaseOverviewModel(currentCase, languageId);
            var newCase = GetNewCaseModel(currentCase.Customer_Id, languageId);

            newCase.NewCase.Customer = currentCustomer;
            newCase.NewCase.Customer_Id = currentCustomer.Id;

            model.CaseOverview = caseOverview;
            model.NewCase = newCase;

            return this.View(model);
        }

        public ActionResult Search()
        {
            return this.View();
        }

        public ActionResult New()
        {
            return this.View();
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
                this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), fileName.Trim());
                
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
                                  ? this._userTemporaryFilesStorage.GetFileContent(fileName, id, "")
                                  : this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public RedirectToRouteResult SendMail(int caseId, int languageId, string extraNote)
        {
            IDictionary<string, string> errors;

            var currentCase = _caseService.GetCaseById(caseId);

            // save case history
            int caseHistoryId = this._caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, out errors, currentCase.Administrator.Email);


            // save log
            
            var caseLog = new CaseLog
                              {                                  
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  CaseHistoryId = caseHistoryId,
                                  TextExternal = extraNote,
                                  UserId = null,
                                  TextInternal = "",                                  
                                  WorkingTimeHour = 0,
                                  WorkingTimeMinute = 0,
                                  EquipmentPrice = 0,
                                  Price = 0,
                                  Charge = false,
                                  RegUser = currentCase.PersonsEmail,
                                  SendMailAboutCaseToNotifier = true,
                                  SendMailAboutLog = true,
                                  EmailRecepientsInternalLog = currentCase.Administrator.Email
                              };

            var temporaryLogFiles = this._userTemporaryFilesStorage.GetFiles(currentCase.CaseGUID.ToString(), "");                        
            caseLog.Id = this._logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);


            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
            this._logFileService.AddFiles(newLogFiles);

            // send emails
            var caseMailSetting = new CaseMailSetting();
            caseMailSetting.HelpdeskMailFromAdress = currentCase.Administrator.Email;

            this._caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, newLogFiles);    
                        
            return RedirectToAction("Index",new {id = currentCase.CaseGUID, languageId });
        }


        private CaseOverviewModel GetCaseOverviewModel(Case currentCase, int languageId)
        {            

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 || 
                                                                       c.Name == "tblLog.Text_External" || 
                                                                       c.Name == "tblLog.Text_Internal" ||                                                          
                                                                       c.Name == "CaseNumber"  ||
                                                                       c.Name == "RegTime")
                                                           .ToList();            

            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);
            // 6 is id of SelfService Info Text
            var infoText = _infoService.GetInfoText(6, currentCase.Customer_Id, languageId);

            var newLogFile = new FilesModel();

            CaseOverviewModel model = null;
            
            if (currentCase != null) 
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).ToList();
                
                model = new CaseOverviewModel
                {
                    InfoText = infoText.Name,                    
                    CasePreview = currentCase,
                    CaseFieldGroups = caseFieldGroups,
                    CaseLogs = caselogs,
                    FieldSettings = caseFieldSetting,
                    LogFilesModel = newLogFile
                };
            }
            
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

            var model = new NewCaseModel(newCase, regions, departments, caseTypes, productAreas, systems,
                                         categories, currencies, suppliers, caseFieldGroups, caseFieldSetting, caseFile);

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
