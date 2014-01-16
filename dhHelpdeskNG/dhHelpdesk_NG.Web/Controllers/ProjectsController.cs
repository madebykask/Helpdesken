using System.Web.Mvc;

namespace dhHelpdesk_NG.Web.Controllers
{
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Concrete;
    using dhHelpdesk_NG.Web.Infrastructure;

    public class ProjectsController : BaseController
    {
        private IProjectService projectService;

        public ProjectsController(IMasterDataService masterDataService, IProjectService projectService)
            : base(masterDataService)
        {
            this.projectService = projectService;
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
