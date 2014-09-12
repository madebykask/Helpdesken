namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class CaseInvoiceController : BaseAdminController
    {
        private readonly ICustomerService customerService;

        private readonly ICaseInvoiceFactory caseInvoiceFactory;

        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        public CaseInvoiceController(
                IMasterDataService masterDataService, 
                ICustomerService customerService, 
                ICaseInvoiceFactory caseInvoiceFactory, 
                IProductAreaService productAreaService, 
                IInvoiceArticleService invoiceArticleService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.caseInvoiceFactory = caseInvoiceFactory;
            this.productAreaService = productAreaService;
            this.invoiceArticleService = invoiceArticleService;
        }

        [HttpGet]
        public ActionResult CaseInvoiceSettings(int customerId)
        {
            var customer = this.customerService.GetCustomer(customerId);
            var model = this.caseInvoiceFactory.GetSettingsModel(customer);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [ValidateAntiForgeryToken]
        public string ArticlesImport(CaseInvoiceSettingsModel model)
        {
            if (model.ArticlesImport == null ||
                model.ArticlesImport.File == null || 
                model.ArticlesImport.File.ContentLength == 0)
            {
                return "File is empty!";
            }

            var importer = this.caseInvoiceFactory.GetImporter(this.productAreaService, this.invoiceArticleService);
            var result = importer.ImportArticles(model.ArticlesImport.File.InputStream);
            importer.SaveImportedArticles(result, model.ArticlesImport.CustomerId);
            return "Import completed successfully!";
        }
    }
}
