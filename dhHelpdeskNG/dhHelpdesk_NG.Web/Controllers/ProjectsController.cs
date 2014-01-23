using System.Web.Mvc;

namespace dhHelpdesk_NG.Web.Controllers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Service;
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
            var m1 = new MyClass { Id = 1, Name = "Name1", Description = "Desc1" };
            var m2 = new MyClass { Id = 1, Name = "Name2", Description = "Desc2" };

            var list = new List<MyClass> { m1, m2 };

            return this.View(list);
        }
    }

    public class MyClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
