namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class InvoiceController : BaseController
    {
        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceHelper invoiceHelper;

        private readonly ICaseService caseService;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        public InvoiceController(
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext, 
            IInvoiceHelper invoiceHelper, 
            ICaseService caseService, 
            ICaseInvoiceSettingsService caseInvoiceSettingsService)
            : base(masterDataService)
        {
            this.invoiceArticleService = invoiceArticleService;
            this.workContext = workContext;
            this.invoiceHelper = invoiceHelper;
            this.caseService = caseService;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
        }

        [HttpGet]
        public JsonResult Articles(int? productAreaId)
        {
            if (!productAreaId.HasValue)
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }

            var articles = this.invoiceArticleService.GetArticles(this.workContext.Customer.CustomerId, productAreaId.Value);
            return this.Json(articles, JsonRequestBehavior.AllowGet);
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
    }
}
