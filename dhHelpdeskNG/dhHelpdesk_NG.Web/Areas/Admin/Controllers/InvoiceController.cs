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

            // TODO: need to get latest filter from session 
            var filter = new InvoiceArticleProductAreaSelectedFilter();
            model.Rows = GetIndexRowModel(customerId, filter, allInvoiceArticles);
            
            model.InvoiceArticles = allInvoiceArticles.OrderBy(a => a.Name).ToList();
            model.ProductAreas = productAreasInRow.ToList();
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult ShowSearchResult(InvoiceArticleProductAreaFilterJSModel filter)
        {
            var selectedSearch = filter.MapToSelectedFilter();
            var model = GetIndexRowModel(selectedSearch.CustomerId, selectedSearch, null);           
            return PartialView("_ArticleProductAreaIndexRows.cshtml", model);
        }

        private InvoiceArticleProductAreaIndexRowsModel GetIndexRowModel(int customerId, 
                                                                         InvoiceArticleProductAreaSelectedFilter selectedFilter, 
                                                                         InvoiceArticle[] invoiceArticles = null)
        {            
            var model = new InvoiceArticleProductAreaIndexRowsModel();

            /*Selection modes*/
            // 0: Article(empty)  - ProductArea(empty)   
            // 1: Article(filled) - ProductArea(empty)   
            // 2: Article(empty)  - ProductArea(filled)   
            // 3: Article(filled) - ProductArea(filled)   
            var selectionMode = 0;
            if (selectedFilter.SelectedInvoiceArticles.Any())
            {
                if (selectedFilter.SelectedProductAreas.Any())
                    selectionMode = 3;
                else
                    selectionMode = 1;
            }
            else
            {
                if (selectedFilter.SelectedProductAreas.Any())
                    selectionMode = 2;             
            }

            var allInvoiceArticles = invoiceArticles == null? invoiceArticleService.GetArticles(customerId) : invoiceArticles;
            switch (selectionMode)
            {
                case 0:
                    foreach (var art in allInvoiceArticles)
                    {
                        if (art.ProductAreas.Any())
                        {
                            foreach (var prod in art.ProductAreas)
                            {
                                model.Data.Add(new InvoiceArticleProductAreaIndexRowModel
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
                    break;
                case 1:
                    foreach (var art in allInvoiceArticles.Where(a=> selectedFilter.SelectedInvoiceArticles.Contains(a.Id)).ToList())
                    {
                        if (art.ProductAreas.Any())
                        {
                            foreach (var prod in art.ProductAreas)
                            {
                                model.Data.Add(new InvoiceArticleProductAreaIndexRowModel
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
                    break;
                case 2:
                    foreach (var art in allInvoiceArticles)
                    {
                        var selectedProds = art.ProductAreas.Where(p => selectedFilter.SelectedProductAreas.Contains(p.Id)).ToList();
                        if (selectedProds.Any())
                        {
                            foreach (var prod in selectedProds)
                            {
                                model.Data.Add(new InvoiceArticleProductAreaIndexRowModel
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
                    break;
                case 3:
                    break;
            }
                                    
            return model;
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
