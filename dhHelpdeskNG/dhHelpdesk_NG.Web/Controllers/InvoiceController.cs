namespace DH.Helpdesk.Web.Controllers
{
    using System;
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

        public InvoiceController(
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext, 
            IInvoiceHelper invoiceHelper, 
            ICaseService caseService)
            : base(masterDataService)
        {
            this.invoiceArticleService = invoiceArticleService;
            this.workContext = workContext;
            this.invoiceHelper = invoiceHelper;
            this.caseService = caseService;
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
            this.invoiceArticleService.SaveCaseInvoices(this.invoiceHelper.ToCaseInvoices(invoices, null, null));
        }

        [HttpPost]
        public ActionResult DoInvoice(int customerId, int caseId, string invoices)
        {
            try
            {
                var caseOverview = this.caseService.GetCaseOverview(caseId);
                var articles = this.invoiceArticleService.GetArticles(customerId);
                var data = this.invoiceHelper.ToCaseInvoices(invoices, caseOverview, articles);
                var output = this.invoiceHelper.ToOutputXml(data);
                return null;
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, ex.Message);                
            }
        }
    }
}
