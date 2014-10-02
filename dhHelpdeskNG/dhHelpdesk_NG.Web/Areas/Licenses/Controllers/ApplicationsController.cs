namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

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
            var model = this.applicationsModelFactory.GetIndexModel();
            return this.View(model);
        }
    }
}
