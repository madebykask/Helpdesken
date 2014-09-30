namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Infrastructure;

    public class ApplicationsController : BaseController
    {
        private readonly IApplicationsService applicationsService;

        public ApplicationsController(
                IMasterDataService masterDataService, 
                IApplicationsService applicationsService)
            : base(masterDataService)
        {
            this.applicationsService = applicationsService;
        }
    }
}
