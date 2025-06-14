﻿namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public sealed class ManufacturersController : BaseController
    {
        private readonly IManufacturersService manufacturersService;

        private readonly IWorkContext workContext;

        private readonly IManufacturersModelFactory manufacturersModelFactory;

        public ManufacturersController(
                IMasterDataService masterDataService, 
                IManufacturersService manufacturersService, 
                IWorkContext workContext, 
                IManufacturersModelFactory manufacturersModelFactory)
            : base(masterDataService)
        {
            this.manufacturersService = manufacturersService;
            this.workContext = workContext;
            this.manufacturersModelFactory = manufacturersModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<ManufacturersFilterModel>(PageName.LicensesManufacturers);
            if (filters == null)
            {
                filters = ManufacturersFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesManufacturers, filters);
            }

            var model = this.manufacturersModelFactory.GetIndexModel(filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Manufacturers(ManufacturersIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<ManufacturersFilterModel>(PageName.LicensesManufacturers);

            SessionFacade.SavePageFilters(PageName.LicensesManufacturers, filters);

            var manufacturers = this.manufacturersService.GetManufacturers(this.workContext.Customer.CustomerId);

            var contentModel = this.manufacturersModelFactory.GetContentModel(manufacturers);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult Manufacturer(int? manufacturerId)
        {
            var data = this.manufacturersService.GetManufacturerData(
                                                this.workContext.Customer.CustomerId,
                                                manufacturerId);
            var model = this.manufacturersModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Manufacturer(ManufacturerEditModel model)
        {
            var manufacturer = this.manufacturersModelFactory.GetBusinessModel(model);
            var manufacturerId = this.manufacturersService.AddOrUpdate(manufacturer);

            return this.RedirectToAction("Manufacturer", new { manufacturerId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.manufacturersService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}
