using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Web.Models.Case;
    using System.Web.Script.Serialization;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public class CaseInvoiceController : BaseController
    {
        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceHelper invoiceHelper;

        private readonly ICaseService caseService;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        private readonly ITemporaryFilesCache userTemporaryFilesStorage;

        private readonly ICaseFileService caseFileService;

        private readonly ILogFileService logFileService;

        private readonly IInvoiceArticlesModelFactory invoiceArticlesModelFactory;

        private readonly IMasterDataService masterDataService;        

        public CaseInvoiceController(
            ICaseService caseService,
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext, 
            IInvoiceHelper invoiceHelper,             
            ICaseInvoiceSettingsService caseInvoiceSettingsService, 
            ICaseFileService caseFileService,
            ILogFileService logFileService,
            IInvoiceArticlesModelFactory invoiceArticlesModelFactory,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory)
            : base(masterDataService)
        {
            this.masterDataService = masterDataService;
            this.invoiceArticleService = invoiceArticleService;
            this.workContext = workContext;
            this.invoiceHelper = invoiceHelper;
            this.caseService = caseService;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
            this.caseFileService = caseFileService;
            this.logFileService = logFileService;
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
            this.invoiceArticlesModelFactory = invoiceArticlesModelFactory;            
        }

        [HttpGet]
        public JsonResult Articles(int caseId, int? productAreaId)
        {
            var hasInvoice = this.invoiceArticleService.GetCaseInvoices(caseId).Any();

            if (!productAreaId.HasValue)
            {
                return this.Json(new { CaseHasInvoice = hasInvoice, Data = "" }, JsonRequestBehavior.AllowGet);
            }

            var articles = this.invoiceArticleService.GetArticles(SessionFacade.CurrentCustomer.Id, productAreaId.Value);
            return this.Json(new { CaseHasInvoice = hasInvoice, Data = articles }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InvoiceSettingsValid(int customerId)
        {
            var invoiceArts = new List<InvoiceArticle>();
            foreach (var art in invoiceArts)
            {

            }
            var SettingsValid = this.invoiceArticleService.ValidateInvoiceSettings(customerId);
            return this.Json(SettingsValid, JsonRequestBehavior.AllowGet);
            
            
        }
    
        [HttpGet]
        public JsonResult CaseFiles(string id, string logKey)
        {
            var files = new List<CaseFileModel>();
            if (string.IsNullOrEmpty(id) || id == "undefined")
                return this.Json(files.ToArray(), JsonRequestBehavior.AllowGet);

            var basePath = string.Empty;
            if (!GuidHelper.IsGuid(id))
            {
                var c = this.caseService.GetCaseById(int.Parse(id));                        
                if (c != null)
                    basePath = masterDataService.GetFilePath(c.Customer_Id);
            }

            try
            {
                #region CaseFiles
                var fileNames = GuidHelper.IsGuid(id)
                                    ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Cases)
                                    : this.caseFileService.FindFileNamesByCaseId(int.Parse(id));

                foreach (var fileName in fileNames)
                {
                    var file = new CaseFileModel
                    {
                        FileName = fileName,
                        Category = ModuleName.Cases
                    };

                    byte[] fileContent = new byte[0];

                    try
                    {
						if (GuidHelper.IsGuid(id))
							fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
						else
						{
							var model = this.caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
							fileContent = model.Content;
						}
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    file.Size = fileContent.Length;
                    file.Type = MimeHelper.GetMimeTypeExtended(fileName);

                    files.Add(file);
                }
                #endregion

                #region LogFiles
                if (!string.IsNullOrEmpty(logKey) && logKey != "undefined")
                {
                    var tempLogfileNames = this.userTemporaryFilesStorage.FindFileNames(logKey, ModuleName.Log);

                    foreach (var fileName in tempLogfileNames)
                    {
                        var file = new CaseFileModel
                        {
                            FileName = fileName,
                            Category = ModuleName.Log
                        };

                        byte[] fileContent = new byte[0];

                        try
                        {
                            fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, logKey, ModuleName.Log);
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        file.Size = fileContent.Length;
                        file.Type = MimeHelper.GetMimeTypeExtended(fileName);

                        files.Add(file);
                    }

                    if (!GuidHelper.IsGuid(id))
                    {
                        var savedLogFiles = new List<KeyValuePair<int, string>>();
                        var caseId = int.Parse(id);
                        savedLogFiles = this.logFileService.FindFileNamesByCaseId(caseId, LogFileType.External);

                        foreach (var logFile in savedLogFiles)
                        {
                            var file = new CaseFileModel
                            {
                                FileName = logFile.Value,
                                Category = ModuleName.Log
                            };

                            byte[] fileContent = new byte[0];

                            try
                            {
								var model = this.logFileService.GetFileContentByIdAndFileName(logFile.Key, basePath, logFile.Value, LogFileType.External);
								fileContent = model.Content;
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            file.Size = fileContent.Length;
                            file.Type = MimeHelper.GetMimeTypeExtended(logFile.Value);

                            files.Add(file);
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }

            return this.Json(files.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCaseInvoice(string caseInvoiceArticle, int customerId,
                                          int caseId, string caseKey, string logKey,
                                          int? orderIdToXML)
        {
            try
            {

                if (string.IsNullOrEmpty(caseInvoiceArticle))
                    return Json(new { result = "Error", data = "Invalid Invoice to save!" });

               
                if (SessionFacade.CurrentUser == null)
                    return Json(new { result = "Error", data = "Invoice is not available, refresh the page and try it again." });

                var saveRes = DoInvoiceWork(caseInvoiceArticle, caseId, customerId,SessionFacade.CurrentUser.Id, orderIdToXML);

                if (saveRes.IsSucceed)
                {
                    var caseInvoices = this.invoiceArticleService.GetCaseInvoicesWithTimeZone(caseId, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    var invoiceArticles = this.invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                    var invoiceModel = new CaseInvoiceModel(customerId, caseId, invoiceArticles, string.Empty, caseKey, logKey);
                    var serializer = new JavaScriptSerializer();
                    var caseArticlesJson = serializer.Serialize(invoiceModel.InvoiceArticles);
                    var warningMessage = saveRes.ResultType == ProcessResult.ResultTypeEnum.WARNING ? saveRes.LastMessage : string.Empty;

                    return Json(new { result = "Success", data = caseArticlesJson, warningMessage = warningMessage });
                }
                else
                {
                    return Json(new { result = "Error", data = saveRes.LastMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", data = "Unexpected Error:" + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult IsThereNotSentOrder(int caseId)
        {
            var res = false;

            var notInvoicedOrders = this.invoiceArticleService.GetInvoiceOrders(caseId, InvoiceOrderFetchStatus.AllNotSent);
            if (notInvoicedOrders.Any())
                res = true;

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public UnicodeFileContentResult CaseFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
            else
            {
                var c = this.caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = masterDataService.GetFilePath(c.Customer_Id);

				var model = this.caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
				fileContent = model.Content;
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        private ProcessResult DoInvoiceWork(string caseInvoiceData, int caseId, int customerId,int userId, int? orderIdToXML)
        {
            var caseOverview = this.caseService.GetCaseOverview(caseId);
            var articles = this.invoiceArticleService.GetArticles(customerId);
            var Invoices = this.invoiceHelper.ToCaseInvoices(caseInvoiceData, caseOverview, articles, SessionFacade.CurrentUser.Id, orderIdToXML); //there will only be one?
            return this.invoiceArticleService.DoInvoiceWork(Invoices, caseId, caseOverview.CaseNumber, customerId, userId, orderIdToXML);
        }
    
    }
}
