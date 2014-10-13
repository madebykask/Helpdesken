namespace DH.Helpdesk.Mobile.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Manufacturers;
    using DH.Helpdesk.Mobile.Enums;
    using DH.Helpdesk.Mobile.Infrastructure;
    using DH.Helpdesk.Mobile.Infrastructure.ActionFilters;

    public class ManufacturersController : BaseController
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
    }
}
