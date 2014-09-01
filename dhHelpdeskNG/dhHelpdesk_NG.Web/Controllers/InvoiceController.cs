namespace DH.Helpdesk.Web.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Invoice;

    public class InvoiceController : BaseController
    {
        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IWorkContext workContext;

        public InvoiceController(
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext)
            : base(masterDataService)
        {
            this.invoiceArticleService = invoiceArticleService;
            this.workContext = workContext;
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
            var toSave = !string.IsNullOrEmpty(invoices)
                   ? InvoiceHelper.ToCaseInvoices(invoices)
                   : null;
            this.invoiceArticleService.SaveCaseInvoices(toSave);
        }

        [HttpGet]
        public ViewResult IkeaArticlesImport()
        {
            return this.View();
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult IkeaArticlesImport(ArticlesImportModel model)
        {
            using (var ms = new MemoryStream())
            {
                var importer = new IkeaExcelImporter();
                var articles = importer.ImportArticles(ms);
            }

            return this.View(model);
        }
    }
}
