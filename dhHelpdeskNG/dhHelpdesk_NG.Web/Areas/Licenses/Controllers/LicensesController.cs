namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class LicensesController : BaseController
    {
        private readonly ILicensesService licensesService;

        private readonly IWorkContext workContext;

        private readonly ILicensesModelFactory licensesModelFactory;

        public LicensesController(
                IMasterDataService masterDataService, 
                ILicensesService licensesService, 
                IWorkContext workContext, 
                ILicensesModelFactory licensesModelFactory)
            : base(masterDataService)
        {
            this.licensesService = licensesService;
            this.workContext = workContext;
            this.licensesModelFactory = licensesModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<LicensesFilterModel>(PageName.LicensesLicenses);
            if (filters == null)
            {
                filters = LicensesFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesLicenses, filters);
            }

            var model = this.licensesModelFactory.GetIndexModel(filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Licenses(LicensesIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<LicensesFilterModel>(PageName.LicensesLicenses);

            SessionFacade.SavePageFilters(PageName.LicensesLicenses, filters);

            var licenses = this.licensesService.GetLicenses(this.workContext.Customer.CustomerId);

            var contentModel = this.licensesModelFactory.GetContentModel(licenses);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult License(int? licenseId)
        {
            var data = this.licensesService.GetLicenseData(
                                            this.workContext.Customer.CustomerId,
                                            licenseId);
            var model = this.licensesModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult License(LicenseEditModel model)
        {
            var license = this.licensesModelFactory.GetBusinessModel(model);
            var licenseId = this.licensesService.AddOrUpdate(license);

            return this.RedirectToAction("License", licenseId);
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.licensesService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}
