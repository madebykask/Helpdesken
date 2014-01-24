namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class ProjectsController : BaseController
    {
        private readonly IProjectService projectService;

        private readonly IUserService userService;

        private readonly INewProjectViewModelFactory newProjectViewModelFactory;

        private readonly IUpdatedProjectViewModelFactory updatedProjectViewModelFactory;

        public ProjectsController(
            IMasterDataService masterDataService,
            IProjectService projectService,
            IUserService userService,
            INewProjectViewModelFactory newProjectViewModelFactory,
            IUpdatedProjectViewModelFactory updatedProjectViewModelFactory)
            : base(masterDataService)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.newProjectViewModelFactory = newProjectViewModelFactory;
            this.updatedProjectViewModelFactory = updatedProjectViewModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ChildActionOnly]
        public ActionResult Search()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult EditProject(int id)
        {
            var project = this.projectService.GetProject(id);
            var projectCollaborators = this.projectService.GetProjectCollaborators(id);
            var projectSchedules = this.projectService.GetProjectSchedules(id);
            var projectLogs = this.projectService.GetProjectLogs(id);
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();

            var viewModel = this.updatedProjectViewModelFactory.Create(
                project,
                users,
                projectCollaborators,
                projectSchedules,
                projectLogs,
                new List<Case>());

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult EditProject(ProjectEditModel projectModel, List<ProjectScheduleEditModel> projectScheduleModels)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            return this.RedirectToAction("EditProject", new { id = projectModel.Id });
        }

        [HttpGet]
        public ActionResult NewProject()
        {
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();
            var viewModel = this.newProjectViewModelFactory.Create(users);

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult NewProject(ProjectEditModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            this.projectService.DeleteProject(id);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddProjectSchedule(ProjectScheduleEditModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            return this.RedirectToAction("EditProject", new { id = model.ProjectId });
        }

        [HttpGet]
        public ActionResult DeleteProjectSchedule(int projectId, int scheduleId)
        {
            this.projectService.DeleteSchedule(scheduleId);
            return this.RedirectToAction("EditProject", new { id = projectId });
        }

        [HttpPost]
        public ActionResult AddProjectLog(ProjectLogEditModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            return this.RedirectToAction("EditProject", new { id = model.ProjectId });
        }

        [HttpGet]
        public ActionResult DeleteProjectLog(int projectId, int logId)
        {
            this.projectService.DeleteLog(logId);
            return this.RedirectToAction("EditProject", new { id = projectId });
        }
    }
}
