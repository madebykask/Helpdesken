namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class ProjectsController : BaseController
    {
        private readonly IProjectService projectService;

        private readonly IUserService userService;

        private readonly ICaseService caseService;

        private readonly INewProjectViewModelFactory newProjectViewModelFactory;

        private readonly IUpdatedProjectViewModelFactory updatedProjectViewModelFactory;

        private readonly INewProjectFactory newProjectFactory;

        private readonly INewProjectScheduleFactory newProjectScheduleFactory;

        private readonly INewProjectLogFactory newProjectLogFactory;

        private readonly IUpdatedProjectFactory updatedProjectFactory;

        private readonly IUpdatedProjectScheduleFactory updatedProjectScheduleFactory;

        private readonly IIndexProjectViewModelFactory indexProjectViewModelFactory;

        public ProjectsController(
            IMasterDataService masterDataService,
            IProjectService projectService,
            IUserService userService,
            ICaseService caseService,
            INewProjectViewModelFactory newProjectViewModelFactory,
            IUpdatedProjectViewModelFactory updatedProjectViewModelFactory,
            INewProjectFactory newProjectFactory,
            INewProjectScheduleFactory newProjectScheduleFactory,
            INewProjectLogFactory newProjectLogFactory,
            IUpdatedProjectFactory updatedProjectFactory,
            IUpdatedProjectScheduleFactory updatedProjectScheduleFactory,
            IIndexProjectViewModelFactory indexProjectViewModelFactory)
            : base(masterDataService)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.caseService = caseService;
            this.newProjectViewModelFactory = newProjectViewModelFactory;
            this.updatedProjectViewModelFactory = updatedProjectViewModelFactory;
            this.newProjectFactory = newProjectFactory;
            this.newProjectScheduleFactory = newProjectScheduleFactory;
            this.newProjectLogFactory = newProjectLogFactory;
            this.updatedProjectFactory = updatedProjectFactory;
            this.updatedProjectScheduleFactory = updatedProjectScheduleFactory;
            this.indexProjectViewModelFactory = indexProjectViewModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var filter = SessionFacade.GetPageFilters<ProjectFilter>(Enums.PageName.Projects) ?? new ProjectFilter();
            var projects = this.projectService.GetCustomerProjects(
                SessionFacade.CurrentCustomer.Id,
                (EntityStatus)filter.State,
                filter.ProjectManagerId,
                filter.ProjectNameLikeString);
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();

            var viewModel = this.indexProjectViewModelFactory.Create(projects, users, filter);
            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(ProjectFilter filter)
        {
            SessionFacade.SavePageFilters(Enums.PageName.Projects, filter);
            var projects = this.projectService.GetCustomerProjects(
                SessionFacade.CurrentCustomer.Id,
                (EntityStatus)filter.State,
                filter.ProjectManagerId,
                filter.ProjectNameLikeString);

            return this.PartialView("ProjectGrid", projects);
        }

        [HttpGet]
        public ActionResult EditProject(int id)
        {
            var project = this.projectService.GetProject(id);
            var projectCollaborators = this.projectService.GetProjectCollaborators(id);
            var projectSchedules = this.projectService.GetProjectSchedules(id);
            var projectLogs = this.projectService.GetProjectLogs(id);

            // todo
            var cases = this.caseService.GetCases().Where(x => x.Customer_Id == SessionFacade.CurrentCustomer.Id && x.Project_Id == id).ToList();
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();

            var viewModel = this.updatedProjectViewModelFactory.Create(
                project,
                users,
                projectCollaborators,
                projectSchedules,
                projectLogs,
                cases);

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult EditProject(UpdatedProjectViewModel projectModel, List<ProjectScheduleEditModel> projectScheduleModels)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projectBussinesModel = this.updatedProjectFactory.Create(projectModel.Project, DateTime.Now);
            //var projectschedulesBussinesModels =
            //    projectScheduleModels.Select(
            //        projectScheduleEditModel =>
            //        this.updatedProjectScheduleFactory.Create(projectScheduleEditModel, DateTime.Now))
            //        .ToList();

            this.projectService.UpdateProject(projectBussinesModel);
            //this.projectService.UpdateSchedule(projectschedulesBussinesModels);

            return this.RedirectToAction("EditProject", new { id = projectModel.Project.Id });
        }

        [HttpGet]
        public ActionResult NewProject()
        {
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();
            var viewModel = this.newProjectViewModelFactory.Create(users);

            return this.View(viewModel);
        }

        // todo
        [HttpPost]
        public ActionResult NewProject(NewProjectViewModel projectEditModel)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projectBussinesModel = this.newProjectFactory.Create(projectEditModel.Project, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.projectService.AddProject(projectBussinesModel);
            this.projectService.AddCollaborator(projectBussinesModel.Id, projectEditModel.Project.ProjectCollaboratorIds);

            return this.RedirectToAction("EditProject", new { id = projectBussinesModel.Id });
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

            var projecScheduleBussinesModel = this.newProjectScheduleFactory.Create(model, DateTime.Now);
            this.projectService.AddSchedule(projecScheduleBussinesModel);

            return this.RedirectToAction("EditProject", new { id = model.ProjectId });
        }

        [HttpGet]
        public ActionResult DeleteProjectSchedule(int projectId, int scheduleId)
        {
            this.projectService.DeleteSchedule(scheduleId);
            return this.RedirectToAction("EditProject", new { id = projectId });
        }

        [HttpPost]
        public ActionResult AddProjectLog(UpdatedProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projecLogBussinesModel = this.newProjectLogFactory.Create(model.ProjectLog, DateTime.Now);
            this.projectService.AddLog(projecLogBussinesModel);

            return this.RedirectToAction("EditProject", new { id = model.ProjectLog.ProjectId });
        }

        [HttpGet]
        public ActionResult DeleteProjectLog(int projectId, int logId)
        {
            this.projectService.DeleteLog(logId);
            return this.RedirectToAction("EditProject", new { id = projectId });
        }
    }
}
