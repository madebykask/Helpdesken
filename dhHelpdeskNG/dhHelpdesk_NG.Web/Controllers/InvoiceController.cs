namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

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
            this.invoiceArticleService.SaveCaseInvoices(InvoiceHelper.ToCaseInvoices(invoices));
        }        
    }
}
