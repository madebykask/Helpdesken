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

        public CaseInvoiceController(
                IMasterDataService masterDataService, 
                ICustomerService customerService, 
                ICaseInvoiceFactory caseInvoiceFactory)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.caseInvoiceFactory = caseInvoiceFactory;
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

            var importer = this.caseInvoiceFactory.GetImporter();
            var result = importer.ImportArticles(model.ArticlesImport.File.InputStream);
            return "Import completed successfully!";
        }
    }
}
