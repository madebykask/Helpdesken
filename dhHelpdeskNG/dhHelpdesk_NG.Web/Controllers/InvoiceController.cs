namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    public class InvoiceController : BaseController
    {
        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IWorkContext workContext;

        private readonly IInvoiceHelper invoiceHelper;

        public InvoiceController(
            IMasterDataService masterDataService, 
            IInvoiceArticleService invoiceArticleService, 
            IWorkContext workContext, 
            IInvoiceHelper invoiceHelper)
            : base(masterDataService)
        {
            this.invoiceArticleService = invoiceArticleService;
            this.workContext = workContext;
            this.invoiceHelper = invoiceHelper;
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
            this.invoiceArticleService.SaveCaseInvoices(this.invoiceHelper.ToCaseInvoices(invoices));
        }

        [HttpPost]
        public ActionResult DoInvoice(string invoices)
        {
            return null;
            try
            {
                var data = this.invoiceHelper.ToCaseInvoices(invoices);
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
