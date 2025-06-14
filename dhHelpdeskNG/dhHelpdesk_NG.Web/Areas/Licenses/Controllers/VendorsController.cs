﻿namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Vendors;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public sealed class VendorsController : BaseController
    {
        private readonly IVendorsService vendorsService;

        private readonly IWorkContext workContext;

        private readonly IVendorsModelFactory vendorsModelFactory;

        public VendorsController(
                IMasterDataService masterDataService, 
                IVendorsService vendorsService, 
                IWorkContext workContext, 
                IVendorsModelFactory vendorsModelFactory)
            : base(masterDataService)
        {
            this.vendorsService = vendorsService;
            this.workContext = workContext;
            this.vendorsModelFactory = vendorsModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<VendorsFilterModel>(PageName.LicensesVendors);
            if (filters == null)
            {
                filters = VendorsFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesVendors, filters);
            }

            var model = this.vendorsModelFactory.GetIndexModel(filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Vendors(VendorsIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<VendorsFilterModel>(PageName.LicensesVendors);

            SessionFacade.SavePageFilters(PageName.LicensesVendors, filters);

            var vendors = this.vendorsService.GetVendors(this.workContext.Customer.CustomerId);

            var contentModel = this.vendorsModelFactory.GetContentModel(vendors);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult Vendor(int? vendorId)
        {
            var data = this.vendorsService.GetVendorData(
                                        this.workContext.Customer.CustomerId,
                                        vendorId);
            var model = this.vendorsModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Vendor(VendorEditModel model)
        {
            var vendor = this.vendorsModelFactory.GetBusinessModel(model);
            var vendorId = this.vendorsService.AddOrUpdate(vendor);

            return this.RedirectToAction("Vendor", new { vendorId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.vendorsService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}
