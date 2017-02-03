﻿using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure;

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
            var productAreas = productAreaService.GetAll(customerId);
            var lastLevels = new List<ProductArea>();
        
            foreach (var p in productAreas)
            {
                if (p.SubProductAreas.Count == 0)
                {
                    p.Name = p.ResolveFullName();
                    lastLevels.Add(p);
                }
            }
            
            var allInvoiceArticles = invoiceArticleService.GetArticles(customerId);

            // TODO: need to get latest filter from session 
            InvoiceArticleProductAreaSelectedFilter CS = new InvoiceArticleProductAreaSelectedFilter();
            if (SessionFacade.CurrentInvoiceArticleProductAreaSearch != null)
            {
                CS = SessionFacade.CurrentInvoiceArticleProductAreaSearch;
                
            }

            model.IAPSearch_Filter = CS;
            model.InvoiceArticles = allInvoiceArticles.OrderBy(a => a.Number).ToList();
            model.ProductAreas = lastLevels.OrderBy(l=> l.Name).ToList();
            return this.View(model);
        }

        [HttpGet]
        public ActionResult New(int customerId)
        {
            var customer = this.customerService.GetCustomer(customerId);
            

            //var invoicArticleProdArea = new InvoiceArticleProductArea {  };

            var model = this.CreateInputViewModel(customer);

            return this.View(model);
        }

        [HttpPost]
        public JsonResult Save(InvoiceArticleProductAreaFilterJSModel filter)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            var selectedItems = filter.MapToSelectedFilter();

            if (selectedItems.SelectedInvoiceArticles.Any() && selectedItems.SelectedProductAreas.Any())            
                this.invoiceArticleService.SaveArticleProductArea(selectedItems);
            else
                return Json(new { res = "error", data = Translation.GetCoreTextTranslation("Du måste välja både artikel och produktområde!") });

            if (errors.Count != 0)
                return Json(new { res = "error", data = string.Join(" - " ,errors.Values)});

            var target = Url.Action("ArticleProductAreaIndex", "Invoice",
                        new { customerId = selectedItems.CustomerId, area ="admin" }, Request.Url.Scheme);
                                
            return Json(new { res = "sucess", data = target});
        }

        [HttpPost]
        public ActionResult DeleteArticleProductArea(int articleid, int productareaid, int customerid)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this.invoiceArticleService.DeleteArticleProductArea(articleid, productareaid);

            if (errors.Count == 0)
                return this.RedirectToAction("ArticleProductAreaIndex", "invoice", new { customerId = customerid, area = "admin" });

            var customer = customerService.GetCustomer(customerid);
            var model = this.CreateInputViewModel(customer);

            return this.View(model);
        }

        private InvoiceArticleProductAreaInputViewModel CreateInputViewModel(Customer customer)
        {
            var allInvoiceArticles = invoiceArticleService.GetArticles(customer.Id).OrderBy(a => a.Number);
            var productAreas = productAreaService.GetAll(customer.Id);
            var lastLevels = new List<ProductArea>();

            foreach (var p in productAreas)
            {
                if (p.SubProductAreas.Count == 0)
                {
                    p.Name = p.ResolveFullName();
                    lastLevels.Add(p);
                }
            }            

            var model = new InvoiceArticleProductAreaInputViewModel
            {
                Customer = customer,
                ProductAreas = lastLevels.OrderBy(p=> p.Name).ToList(),
                Articles = allInvoiceArticles.ToList()
            };

            return model;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [ValidateAntiForgeryToken]
        public JsonResult ArticlesImport(CaseInvoiceSettingsModel model)
        {
            if (model.ArticlesImport == null ||
                model.ArticlesImport.File == null || 
                model.ArticlesImport.File.ContentLength == 0)
            {
                var answer = new
                {
                    Success = false,
                    Status = Translation.GetCoreTextTranslation("Filen är tom")
                };
                return Json(answer, JsonRequestBehavior.AllowGet);
            }
            var lastSyncDate = DateTime.UtcNow;
            var importer = this.caseInvoiceFactory.GetImporter(this.productAreaService, this.invoiceArticleService);
            var result = importer.ImportArticles(model.ArticlesImport.File.InputStream, lastSyncDate);
            if (result.Errors.Any())
            {
                var error = new StringBuilder();
                foreach (var resError in result.Errors)
                {
                    error.AppendLine(Translation.GetCoreTextTranslation(resError));
                }
                var answer = new
                {
                    Success = false,
                    Status = error.ToString()
                };
                return Json(answer, JsonRequestBehavior.AllowGet);
            }
            else
            {
                importer.SaveImportedArticles(result, model.ArticlesImport.CustomerId, lastSyncDate);
                var answer = new
                {
                    Success = true,
                    Status = Translation.GetCoreTextTranslation("Import slutförts")
                };
                return Json(answer, JsonRequestBehavior.AllowGet);
            }
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
