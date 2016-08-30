using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    
    using DH.Helpdesk.Domain;

    public class InvoiceController : BaseAdminController
    {
        private readonly ICustomerService customerService;

        private readonly ICaseInvoiceFactory caseInvoiceFactory;

        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        public InvoiceController(
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

        [HttpGet]
        public ActionResult ArticleProductAreaIndex(int customerId)
        {
            var customer = customerService.GetCustomer(customerId);
            var model = new InvoiceArticleProductAreaIndexModel(customer);
            var productAreas = productAreaService.GetAllProductAreas(customerId);
            var productAreasInRow = productAreaService.GetChildsInRow(productAreas).ToList();
            
            var allInvoiceArticles = invoiceArticleService.GetArticles(customerId);

            foreach (var art in allInvoiceArticles)
            {
                if (art.ProductAreas.Any())
                {
                    foreach (var prod in art.ProductAreas)
                    {
                        model.Add(new InvoiceArticleProductAreaModel
                        {
                            InvoiceArticleId = art.Id,
                            InvoiceArticleName = art.Name, // add desction as well
                            InvoiceArticleNumber = art.Number,
                            ProductAreaId = prod.Id,
                            ProductAreaName = prod.ResolveFullName()
                        });
                    }
                }
            }

            model.InvoiceArticles = allInvoiceArticles.OrderBy(a => a.Name).ToList();
            model.ProductAreas = productAreasInRow.ToList();
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
