using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Projects;
    using DH.Helpdesk.Web.Infrastructure.Filters.Projects;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Projects;
    

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

        private readonly ITemporaryFilesCache userTemporaryFilesStorage;

        private readonly IEditorStateCache userEditorValuesStorage;

        private readonly IMasterDataService masterDataService;

        private readonly ISettingService settingService;

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
            IEditorStateCacheFactory userEditorValuesStorageFactory,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            ISettingService settingService)
            : base(masterDataService)
        {
            this.masterDataService = masterDataService;
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
            this.settingService = settingService;

            this.userEditorValuesStorage = userEditorValuesStorageFactory.CreateForModule(ModuleName.Project);
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Project);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var filter = SessionFacade.FindPageFilters<ProjectFilter>(PageName.Projects) ?? new ProjectFilter();
            SortField sortField = ExtractSortField(filter);

            var cs = this.settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var isFirstName = (cs.IsUserFirstLastNameRepresentation ==1);
            var projects = this.projectService.GetCustomerProjects(
                SessionFacade.CurrentCustomer.Id,
                (EntityStatus)filter.State,
                filter.ProjectManagerId,
                filter.ProjectNameLikeString,
                sortField,
                isFirstName
                );

            projects = SetNameOriation(projects, isFirstName);
            
            var users = this.userService.GetCustomerUsers(SessionFacade.CurrentCustomer.Id).MapToSelectList(cs);
           
            var viewModel = this.indexProjectViewModelFactory.Create(projects, users, filter);
            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(ProjectFilter filter)
        {
            SessionFacade.SavePageFilters(PageName.Projects, filter);

            SortField sortField = ExtractSortField(filter);
            var cs = this.settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var isFirstName = (cs.IsUserFirstLastNameRepresentation == 1);

            List<ProjectOverview> projects = this.projectService.GetCustomerProjects(
                SessionFacade.CurrentCustomer.Id,
                (EntityStatus)filter.State,
                filter.ProjectManagerId,
                filter.ProjectNameLikeString,
                sortField,
                isFirstName);

            projects = SetNameOriation(projects, isFirstName);
            var viewModel = new ProjectOverviewSorting(projects, filter.SortField);

            return this.PartialView("ProjectGrid", viewModel);
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

        /// <summary>
        /// The edit project.
        /// </summary>
        /// <param name="projectEditModel">
        /// The project edit model.
        /// </param>
        /// <param name="projectScheduleEditModels">
        /// The project schedule edit models.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult EditProject(ProjectEditModel projectEditModel, List<ProjectScheduleEditModel> projectScheduleEditModels)
        {
            if (!this.ModelState.IsValid)
            {
                var model = this.CreateEditProjectViewModel(projectEditModel.Id);
                model.ProjectEditModel = projectEditModel;
                return this.View(model);
            }

            var projectBussinesModel = this.updatedProjectFactory.Create(projectEditModel, DateTime.Now);

            this.projectService.UpdateProject(projectBussinesModel);
            this.projectService.AddCollaborator(projectBussinesModel.Id, projectEditModel.ProjectCollaboratorIds);

            // todo need to use mappers
            var pr = this.projectService.GetProject(projectBussinesModel.Id);
            var basePath = string.Empty;
            if (pr != null)
                basePath = masterDataService.GetFilePath(pr.CustomerId);            
            var newRegistrationFiles = this.userTemporaryFilesStorage.FindFiles(projectEditModel.Id).Select(x => new NewProjectFile(projectBussinesModel.Id, x.Content, basePath, x.Name, DateTime.Now)).ToList();
            var deletedRegistrationFiles = this.userEditorValuesStorage.FindDeletedFileNames(projectEditModel.Id);
            this.projectService.DeleteFiles(projectEditModel.Id, basePath, deletedRegistrationFiles);
            this.projectService.AddFiles(newRegistrationFiles);

            this.userTemporaryFilesStorage.ResetCacheForObject(projectEditModel.Id);
            this.userEditorValuesStorage.ClearObjectDeletedFiles(projectEditModel.Id);

            if (projectScheduleEditModels != null)
            {
                var projecScheduleBussinesModels = projectScheduleEditModels.Select(x => this.updatedProjectScheduleFactory.Create(x, DateTime.Now)).ToList();
                this.projectService.UpdateSchedule(projecScheduleBussinesModels);
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult NewProject()
        {
            var cs = this.settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var users = this.userService.GetCustomerUsers(SessionFacade.CurrentCustomer.Id);

            var viewModel = this.newProjectViewModelFactory.Create(users.MapToSelectList(cs), Guid.NewGuid().ToString());
            return this.View(viewModel);
        }

        // todo
        [HttpPost]
        public ActionResult NewProject(ProjectEditModel projectEditModel, string guid)
        {
            if (!this.ModelState.IsValid)
            {
                var cs = this.settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
                var users = this.userService.GetCustomerUsers(SessionFacade.CurrentCustomer.Id);
                var model = this.newProjectViewModelFactory.Create(users.MapToSelectList(cs), guid);
                model.ProjectEditModel = projectEditModel;
                return this.View(model);
            }

            var projectBussinesModel = this.newProjectFactory.Create(projectEditModel, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.projectService.AddProject(projectBussinesModel);
            this.projectService.AddCollaborator(projectBussinesModel.Id, projectEditModel.ProjectCollaboratorIds);

            // todo need to use mappers
            var basePath = masterDataService.GetFilePath(SessionFacade.CurrentCustomer.Id);
            var registrationFiles = this.userTemporaryFilesStorage.FindFiles(guid);
            var files = registrationFiles.Select(x => new NewProjectFile(projectBussinesModel.Id, x.Content, basePath, x.Name, DateTime.Now)).ToList();
            this.projectService.AddFiles(files);

            this.userTemporaryFilesStorage.ResetCacheForObject(guid);

            return this.RedirectToAction("EditProject", new { id = projectBussinesModel.Id });
        }

        public ActionResult DeleteProject(int id)
        {
            this.projectService.DeleteProject(id);
            this.userTemporaryFilesStorage.ResetCacheForObject(id);
            this.userEditorValuesStorage.ClearObjectDeletedFiles(id);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddProjectSchedule(ProjectScheduleEditModel projectScheduleEditModel)
        {
            if (!this.ModelState.IsValid)
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
            if (!this.ModelState.IsValid)
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
            if (!this.ModelState.IsValid)
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

        public PartialViewResult AttachedFiles(string guid)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(guid))
            {
                fileNames = this.userTemporaryFilesStorage.FindFileNames(guid);
            }
            else
            {
                var fileNamesFromTemporaryStorage = this.userTemporaryFilesStorage.FindFileNames(guid);

                var deletedFileNames = this.userEditorValuesStorage.FindDeletedFileNames(int.Parse(guid));

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

                var basePath = masterDataService.GetFilePath(SessionFacade.CurrentCustomer.Id);
                fileContent = fileInWebStorage
                    ? this.userTemporaryFilesStorage.GetFileContent(fileName, guid)
                    : this.projectService.GetFileContent(int.Parse(guid),basePath, fileName);
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
                    this.userEditorValuesStorage.AddDeletedFile(fileName, int.Parse(guid));
                }
            }

            return this.RedirectToAction("AttachedFiles", new { guid });
        }

        private UpdatedProjectViewModel CreateEditProjectViewModel(int id)
        {
            this.userTemporaryFilesStorage.ResetCacheForObject(id);
            this.userEditorValuesStorage.ClearObjectDeletedFiles(id); // todo redirect after New Project

            var project = this.projectService.GetProject(id);
            var projectCollaborators = this.projectService.GetProjectCollaborators(id).OrderBy(x => x.UserName).ToList();
            var projectSchedules = this.projectService.GetProjectSchedules(id);
            var projectLogs = this.projectService.GetProjectLogs(id);
            
            var cases = this.caseService.GetProjectCases(SessionFacade.CurrentCustomer.Id, id).ToList();            
            var cs = this.settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var users = this.userService.GetCustomerUsers(SessionFacade.CurrentCustomer.Id);
            var isFirstName = (cs.IsUserFirstLastNameRepresentation == 1);

            var viewModel = this.updatedProjectViewModelFactory.Create(
                project,
                users.MapToSelectList(cs),
                projectCollaborators,
                projectSchedules,
                projectLogs,
                cases);

            foreach (var log in viewModel.ProjectLogs)
                log.ResponsibleUser = (isFirstName ? string.Format("{0} {1}", log.ResponsibleUser, log.ResponsibleUserSurName) :
                                                     string.Format("{0} {1}", log.ResponsibleUserSurName, log.ResponsibleUser));
            return viewModel;
        }

        private static SortField ExtractSortField(ProjectFilter filter)
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(filter.SortField.Name))
            {
                sortField = new SortField(filter.SortField.Name, filter.SortField.SortBy.Value);
            }
            return sortField;
        }

        private List<ProjectOverview> SetNameOriation(List<ProjectOverview> projects, bool isFirstName)
        {
            foreach (var project in projects)
            {
                project.ProjectManagerName = (isFirstName ? string.Format("{0} {1}", project.ProjectManagerName, project.ProjectManagerSurName) :
                                                            string.Format("{0} {1}", project.ProjectManagerSurName, project.ProjectManagerName));
            }
            return projects;
        }
    }
}
