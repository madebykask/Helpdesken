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
        private readonly IInfoService _infoService;
        private readonly ICaseService _caseService;
        private readonly ILogService _logService;        
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserTemporaryFilesStorage _userTemporaryFilesStorage;
        private readonly ICaseFileService _caseFileService;
        private readonly ILogFileService _logFileService;

        

        public CaseController(ICaseService caseService,
                              ICaseFieldSettingService caseFieldSettingService,
                              IMasterDataService masterDataService,
                              ILogService logService,
                              IInfoService infoService,
                              IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
                              ICaseFileService caseFileService,
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
            this._logFileService = logFileService;
        }

        [HttpGet]
        public ActionResult Index(string id, int languageId = 1)        
        {
            var guid = new Guid(id);
            SessionFacade.CurrentLanguageId = languageId;

            this._userTemporaryFilesStorage.DeleteFiles(id);
            
            var model = GetCaseOverview(guid, languageId);

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
       

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
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
                //this.case.Commit();
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
        public FileContentResult DownloadLogFile(string id, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(id)
                                  ? this._userTemporaryFilesStorage.GetFileContent(fileName, id, "")
                                  : this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), fileName);

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        private CaseOverviewModel GetCaseOverview(Guid GUID, int languageId)
        {
            var currentCase = _caseService.GetCaseByGUID(GUID);

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 || 
                                                                       c.Name == "tblLog.Text_External" || 
                                                                       c.Name == "tblLog.Text_Internal" ||                                                          
                                                                       c.Name == "CaseNumber"  ||
                                                                       c.Name == "RegTime")
                                                           .ToList();            

            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);

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
