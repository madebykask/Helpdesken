namespace DH.Helpdesk.Mobile.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Vendors;
    using DH.Helpdesk.Mobile.Enums;
    using DH.Helpdesk.Mobile.Infrastructure;
    using DH.Helpdesk.Mobile.Infrastructure.ActionFilters;

    public class VendorsController : BaseController
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
    }
}
