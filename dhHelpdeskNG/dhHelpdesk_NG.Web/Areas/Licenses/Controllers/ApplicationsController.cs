namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Applications;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;

    public class ApplicationsController : BaseController
    {
        private readonly IApplicationsService applicationsService;

        private readonly IWorkContext workContext;

        private readonly IApplicationsModelFactory applicationsModelFactory;

        public ApplicationsController(
                IMasterDataService masterDataService, 
                IApplicationsService applicationsService, 
                IWorkContext workContext, 
                IApplicationsModelFactory applicationsModelFactory)
            : base(masterDataService)
        {
            this.applicationsService = applicationsService;
            this.workContext = workContext;
            this.applicationsModelFactory = applicationsModelFactory;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var filters = SessionFacade.FindPageFilters<ApplicationsFilterModel>(PageName.LicensesApplications);
            if (filters == null)
            {
                filters = ApplicationsFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.LicensesApplications, filters);
            }

            var model = this.applicationsModelFactory.GetIndexModel(filters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Applications(ApplicationsIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<ApplicationsFilterModel>(PageName.LicensesApplications);

            SessionFacade.SavePageFilters(PageName.LicensesApplications, filters);

            var applications = this.applicationsService.GetApplications(
                                    this.workContext.Customer.CustomerId,
                                    filters.OnlyConnected);

            var contentModel = this.applicationsModelFactory.GetContentModel(applications);
            return this.PartialView(contentModel);
        }

        [HttpGet]
        public ViewResult Application(int? applicationId)
        {
            var data = this.applicationsService.GetApplicationData(
                                                this.workContext.Customer.CustomerId,
                                                applicationId);
            var model = this.applicationsModelFactory.GetEditModel(data);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Application(ApplicationEditModel model)
        {
            var application = this.applicationsModelFactory.GetBusinessModel(model);
            var applicationId = this.applicationsService.AddOrUpdate(application);

            return this.RedirectToAction("Application", new { applicationId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.applicationsService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}
