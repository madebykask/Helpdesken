﻿namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Products;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public sealed class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        private readonly IWorkContext workContext;

        private readonly IProductsModelFactory productsModelFactory;

        public ProductsController(
                IMasterDataService masterDataService, 
                IProductsService productsService, 
                IWorkContext workContext, 
                IProductsModelFactory productsModelFactory)
            : base(masterDataService)
        {
            this.productsService = productsService;
            this.workContext = workContext;
            this.productsModelFactory = productsModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<ProductsFilterModel>(PageName.LicensesProducts);
            if (filters == null)
            {
                filters = ProductsFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesProducts, filters);
            }

            var data = this.productsService.GetProductsFilterData(this.workContext.Customer.CustomerId);

            var model = this.productsModelFactory.GetIndexModel(data, filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Products(ProductsIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<ProductsFilterModel>(PageName.LicensesProducts);

            SessionFacade.SavePageFilters(PageName.LicensesProducts, filters);

            var products = this.productsService.GetProducts(
                                    this.workContext.Customer.CustomerId,
                                    filters.RegionIds,
                                    filters.DepartmentIds,
                                    filters.ProductIds);

            var contentModel = this.productsModelFactory.GetContentModel(products);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult Product(int? productId)
        {
            var data = this.productsService.GetProductData(
                                            this.workContext.Customer.CustomerId,
                                            productId);
            var model = this.productsModelFactory.GetEditModel(data);
            return this.View(model);
        }   

        [HttpPost]
        public RedirectToRouteResult Product(ProductEditModel model, int[] selectedApplications)
        {
            model.SelectedApplications = selectedApplications != null ?
                selectedApplications.Select(a => new SelectListItem { Value = a.ToString(CultureInfo.InvariantCulture) }).ToList() :
                new List<SelectListItem>();
            var product = this.productsModelFactory.GetBusinessModel(model);
            var productId = this.productsService.AddOrUpdate(product);

            return this.RedirectToAction("Product", new { productId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.productsService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}
