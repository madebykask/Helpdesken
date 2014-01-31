namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.Tools;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Models.Projects;

    using PostSharp.Aspects;

    public class ProjectsController : BaseController
    {
        public const string ProjectTabName = "#fragment-1";

        public const string LogTabName = "#fragment-2";

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

        private readonly IUserTemporaryFilesStorage userTemporaryFilesStorage;

        private readonly IUserEditorValuesStorage userEditorValuesStorage;

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
            IIndexProjectViewModelFactory indexProjectViewModelFactory,
            IUserEditorValuesStorageFactory userEditorValuesStorageFactory,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory)
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

            this.userEditorValuesStorage = userEditorValuesStorageFactory.Create(TopicName.Changes);
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create(TopicName.Changes);
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
        [CurrentTab(ProjectTabName)]
        public ActionResult EditProject(int id)
        {
            var viewModel = this.CreateEditProjectViewModel(id);

            return this.View(viewModel);
        }

        [HttpGet]
        [CurrentTab(LogTabName)]
        public ActionResult EditProjectLogActiveTab(int id)
        {
            var viewModel = this.CreateEditProjectViewModel(id);

            return this.View("EditProject", viewModel);
        }

        [HttpPost]
        public ActionResult EditProject(ProjectEditModel projectEditModel)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projectBussinesModel = this.updatedProjectFactory.Create(projectEditModel, DateTime.Now);

            this.projectService.UpdateProject(projectBussinesModel);
            this.projectService.AddCollaborator(projectBussinesModel.Id, projectEditModel.ProjectCollaboratorIds);

            // todo need to use mappers
            var newRegistrationFiles = this.userTemporaryFilesStorage.GetFiles(projectEditModel.Id).Select(x => new NewProjectFile(projectBussinesModel.Id, x.Content, x.Name, DateTime.Now)).ToList();
            var deletedRegistrationFiles = this.userEditorValuesStorage.GetDeletedFileNames(projectEditModel.Id);
            this.projectService.DeleteFiles(projectEditModel.Id, deletedRegistrationFiles);
            this.projectService.AddFiles(newRegistrationFiles);

            this.userTemporaryFilesStorage.DeleteFiles(projectEditModel.Id);
            this.userEditorValuesStorage.ClearDeletedFileNames(projectEditModel.Id);

            return this.RedirectToAction("EditProject", new { id = projectEditModel.Id });
        }

        [HttpGet]
        public ActionResult NewProject()
        {
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id).ToList();
            var viewModel = this.newProjectViewModelFactory.Create(users, Guid.NewGuid().ToString());

            return this.View(viewModel);
        }

        // todo
        [HttpPost]
        public ActionResult NewProject(ProjectEditModel projectEditModel, string guid)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projectBussinesModel = this.newProjectFactory.Create(projectEditModel, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.projectService.AddProject(projectBussinesModel);
            this.projectService.AddCollaborator(projectBussinesModel.Id, projectEditModel.ProjectCollaboratorIds);

            // todo need to use mappers
            var registrationFiles = this.userTemporaryFilesStorage.GetFiles(guid);
            var files = registrationFiles.Select(x => new NewProjectFile(projectBussinesModel.Id, x.Content, x.Name, DateTime.Now)).ToList();
            this.projectService.AddFiles(files);

            return this.RedirectToAction("EditProject", new { id = projectBussinesModel.Id });
        }

        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            this.projectService.DeleteProject(id);
            this.userTemporaryFilesStorage.DeleteFiles(id);
            this.userEditorValuesStorage.ClearDeletedFileNames(id);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddProjectSchedule(ProjectScheduleEditModel projectScheduleEditModel)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projecScheduleBussinesModel = this.newProjectScheduleFactory.Create(projectScheduleEditModel, DateTime.Now);
            this.projectService.AddSchedule(projecScheduleBussinesModel);

            return this.RedirectToAction("EditProject", new { id = projectScheduleEditModel.ProjectId });
        }

        [HttpPost]
        [CurrentTab(ProjectTabName)]
        public ActionResult EditProjectSchedules(int projectId, List<ProjectScheduleEditModel> projectScheduleEditModels)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projecScheduleBussinesModels = projectScheduleEditModels.Select(x => this.updatedProjectScheduleFactory.Create(x, DateTime.Now)).ToList();
            this.projectService.UpdateSchedule(projecScheduleBussinesModels);

            return this.RedirectToAction("EditProject", new { id = projectId });
        }

        [HttpGet]
        [CurrentTab(ProjectTabName)]
        public ActionResult DeleteProjectSchedule(int projectId, int scheduleId)
        {
            this.projectService.DeleteSchedule(scheduleId);
            return this.RedirectToAction("EditProject", new { id = projectId });
        }

        [HttpPost]
        public ActionResult AddProjectLog(ProjectLogEditModel projectLog)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var projecLogBussinesModel = this.newProjectLogFactory.Create(projectLog, DateTime.Now);
            this.projectService.AddLog(projecLogBussinesModel);

            return this.RedirectToAction("EditProjectLogActiveTab", new { id = projectLog.ProjectId });
        }

        [HttpGet]
        public ActionResult DeleteProjectLog(int projectId, int logId)
        {
            this.projectService.DeleteLog(logId);
            return this.RedirectToAction("EditProjectLogActiveTab", new { id = projectId });
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string guid)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(guid))
            {
                fileNames = this.userTemporaryFilesStorage.GetFileNames(guid);
            }
            else
            {
                var fileNamesFromTemporaryStorage = this.userTemporaryFilesStorage.GetFileNames(guid);

                var deletedFileNames = this.userEditorValuesStorage.GetDeletedFileNames(int.Parse(guid));

                var fileNamesFromService = this.projectService.FindFileNamesExcludeSpecified(
                    int.Parse(guid),
                    deletedFileNames);

                fileNames = new List<string>(fileNamesFromTemporaryStorage.Count + fileNamesFromService.Count);

                fileNames.AddRange(fileNamesFromTemporaryStorage);
                fileNames.AddRange(fileNamesFromService);
            }

            var model = new FilesModel(guid, fileNames);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string guid, string name)
        {
            var uploadedFile = this.Request.Files[0];

            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NoContent, null);
            }

            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (this.userTemporaryFilesStorage.FileExists(name, guid))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (GuidHelper.IsGuid(guid))
            {
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, guid);
            }
            else
            {
                if (this.projectService.FileExists(int.Parse(guid), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.userTemporaryFilesStorage.AddFile(uploadedData, name, guid);
            }

            return this.RedirectToAction("AttachedFiles", new { guid });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string guid, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(guid))
            {
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, guid);
            }
            else
            {
                var fileInWebStorage = this.userTemporaryFilesStorage.FileExists(
                    fileName,
                    guid);

                fileContent = fileInWebStorage
                    ? this.userTemporaryFilesStorage.GetFileContent(fileName, guid)
                    : this.projectService.GetFileContent(int.Parse(guid), fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string guid, string fileName)
        {
            if (GuidHelper.IsGuid(guid))
            {
                this.userTemporaryFilesStorage.DeleteFile(fileName, guid);
            }
            else
            {
                if (this.userTemporaryFilesStorage.FileExists(fileName, guid))
                {
                    this.userTemporaryFilesStorage.DeleteFile(fileName, guid);
                }
                else
                {
                    this.userEditorValuesStorage.AddDeletedFileName(fileName, int.Parse(guid));
                }
            }

            return this.RedirectToAction("AttachedFiles", new { guid });
        }

        private UpdatedProjectViewModel CreateEditProjectViewModel(int id)
        {
            this.userTemporaryFilesStorage.DeleteFiles(id);
            this.userEditorValuesStorage.ClearDeletedFileNames(id); // todo redirect after New Project

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

            return viewModel;
        }

        [Serializable]
        public sealed class CurrentTabAttribute : OnMethodBoundaryAspect
        {
            private readonly string tabName;

            public CurrentTabAttribute(string tabName)
            {
                this.tabName = tabName;
            }

            public override void OnEntry(MethodExecutionArgs args)
            {
                base.OnEntry(args);

                SessionFacade.ActiveTab = this.tabName;
            }
        }
    }
}
