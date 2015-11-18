namespace DH.Helpdesk.Web.Controllers
{
    using System;
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

    public class InvoiceController : BaseController
    {
        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceHelper invoiceHelper;

        private readonly ICaseService caseService;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        private readonly ITemporaryFilesCache userTemporaryFilesStorage;

        private readonly ICaseFileService caseFileService;

        private readonly IMasterDataService masterDataService;

        public InvoiceController(
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext, 
            IInvoiceHelper invoiceHelper, 
            ICaseService caseService, 
            ICaseInvoiceSettingsService caseInvoiceSettingsService, 
            ICaseFileService caseFileService,
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
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
        }

        [HttpGet]
        public JsonResult Articles(int? productAreaId)
        {
            if (!productAreaId.HasValue)
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }

            var articles = this.invoiceArticleService.GetArticles(SessionFacade.CurrentCustomer.Id, productAreaId.Value);
            return this.Json(articles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InvoiceSettingsValid(int customerId)
        {
            bool SettingsValid = this.invoiceArticleService.ValidateInvoiceSettings(customerId);
            return this.Json(SettingsValid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveArticles(int caseId, string invoices)
        {
            this.invoiceArticleService.SaveCaseInvoices(this.invoiceHelper.ToCaseInvoices(invoices, null, null), caseId);
        }

        [HttpPost]
        public ActionResult DoInvoice(int customerId, int caseId, string invoices)
        {
            try
            {
                var settings = this.caseInvoiceSettingsService.GetSettings(customerId);
                if (settings == null)
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, "Articles invoice failed.");
                }

                var caseOverview = this.caseService.GetCaseOverview(caseId);
                var articles = this.invoiceArticleService.GetArticles(customerId);
                var data = this.invoiceHelper.ToCaseInvoices(invoices, caseOverview, articles);
                foreach (var invoice in data)
                {
                    var userId = 0;
                    if (SessionFacade.CurrentUser.Id != null)
                    {
                        userId = SessionFacade.CurrentUser.Id;
                    }
                    invoice.DoInvoice(userId);
                }

                var output = this.invoiceHelper.ToOutputXml(data);
                if (output == null)
                {
                    return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, "Articles invoice failed.");
                }

                if (!Directory.Exists(settings.ExportPath))
                {
                    Directory.CreateDirectory(settings.ExportPath);
                }

                var path = Path.Combine(settings.ExportPath, this.invoiceHelper.GetExportFileName());
                output.Save(path);

                this.invoiceArticleService.SaveCaseInvoices(data, caseId);    

                return new HttpStatusCodeResult((int)HttpStatusCode.OK, "Articles invoiced."); 
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, ex.Message);                
            }
        }

        [HttpGet]
        public JsonResult CaseFiles(string id)
        {
            var files = new List<CaseFileModel>();

            try
            {
                var fileNames = GuidHelper.IsGuid(id)
                                    ? this.userTemporaryFilesStorage.FindFileNames(id, ModuleName.Cases)
                                    : this.caseFileService.FindFileNamesByCaseId(int.Parse(id));

                foreach (var fileName in fileNames)
                {
                    var file = new CaseFileModel
                    {
                        FileName = fileName
                    };

                    byte[] fileContent = new byte[0];

                    try
                    {
                        if (GuidHelper.IsGuid(id))
                            fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
                        else
                        {
                            var c = this.caseService.GetCaseById(int.Parse(id));
                            var basePath = string.Empty;
                            if (c != null)
                                basePath = masterDataService.GetFilePath(c.Customer_Id);
                            fileContent = this.caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
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
            }
            catch (Exception)
            {
            }

            return this.Json(files.ToArray(), JsonRequestBehavior.AllowGet);
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

                fileContent = this.caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }
    }
}
