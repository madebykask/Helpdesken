namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Infrastructure.ModelFactories;
    using DH.Helpdesk.Mobile.Areas.Admin.Models.Invoice;
    using DH.Helpdesk.Mobile.Infrastructure.ActionFilters;

    public class CaseInvoiceController : BaseAdminController
    {
        private readonly ICustomerService customerService;

        private readonly ICaseInvoiceFactory caseInvoiceFactory;

        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        public CaseInvoiceController(
                IMasterDataService masterDataService, 
                ICustomerService customerService, 
                ICaseInvoiceFactory caseInvoiceFactory, 
                IProductAreaService productAreaService, 
                IInvoiceArticleService invoiceArticleService, 
                ICaseInvoiceSettingsService caseInvoiceSettingsService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.caseInvoiceFactory = caseInvoiceFactory;
            this.productAreaService = productAreaService;
            this.invoiceArticleService = invoiceArticleService;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
        }

        [HttpGet]
        public ActionResult Index(int customerId)
        {
            var customer = this.customerService.GetCustomer(customerId);
            var settings = this.caseInvoiceSettingsService.GetSettings(customerId);
            if (settings == null)
            {
                settings = new CaseInvoiceSettings(customerId);
            }

            var model = this.caseInvoiceFactory.GetSettingsModel(customer, settings);
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

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult SaveSettings(CaseInvoiceSettingsModel model)
        {
            this.caseInvoiceSettingsService.SaveSettings(model.Settings);
            return this.RedirectToAction("Index", new { customerId = model.Settings.CustomerId });
        }
    }
}
