namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Projects;

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;

        private readonly IProjectLogRepository projectLogRepository;

        private readonly IProjectScheduleRepository projectScheduleRepository;

        private readonly IProjectFileRepository projectFileRepository;

        private readonly IProjectCollaboratorRepository projectCollaboratorRepository;

        private readonly ICaseRepository caseRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectLogRepository projectLogRepository,
            IProjectScheduleRepository projectScheduleRepository,
            IProjectFileRepository projectFileRepository,
            IProjectCollaboratorRepository projectCollaboratorRepository,
            ICaseRepository caseRepository)
        {
            this.projectRepository = projectRepository;
            this.projectLogRepository = projectLogRepository;
            this.projectScheduleRepository = projectScheduleRepository;
            this.projectFileRepository = projectFileRepository;
            this.projectCollaboratorRepository = projectCollaboratorRepository;
            this.caseRepository = caseRepository;
        }

        public void AddProject(NewProject project)
        {
            this.projectRepository.Add(project);
            this.projectRepository.Commit();
        }

        public void DeleteProject(int id)
        {
            this.caseRepository.SetNullProblemByProblemId(id);
            this.caseRepository.Commit();

            this.projectLogRepository.DeleteByProjectId(id);
            this.projectLogRepository.Commit();

            this.projectScheduleRepository.DeleteByProjectId(id);
            this.projectScheduleRepository.Commit();

            this.projectFileRepository.Delete(id);
            this.projectFileRepository.Commit();

            this.projectCollaboratorRepository.DeleteByProjectId(id);
            this.projectCollaboratorRepository.Commit();

            this.projectRepository.Delete(id);
            this.projectRepository.Commit();
        }

        public void UpdateProject(UpdatedProject project)
        {
            this.projectRepository.Update(project);
            this.projectRepository.Commit();
        }

        public ProjectOverview GetProject(int id)
        {
            var project = this.projectRepository.FindById(id);
            return project;
        }

        public List<ProjectOverview> GetCustomerProjects(int customerId)
        {
            var projects = this.projectRepository.Find(customerId);
            return projects;
        }

        public List<ProjectOverview> GetCustomerProjects(int customerId, EntityStatus entityStatus, int? projectManagerId, 
                                                         string projectNameLike, SortField sortField, bool isFirstName)
        {
            var projects = this.projectRepository.Find(customerId, entityStatus, projectManagerId, projectNameLike, sortField, isFirstName);
            return projects;
        }

        public void AddSchedule(NewProjectSchedule schedule)
        {
            this.projectScheduleRepository.Add(schedule);
            this.projectScheduleRepository.Commit();
        }

        public void DeleteSchedule(int id)
        {
            this.projectScheduleRepository.Delete(id);
            this.projectScheduleRepository.Commit();
        }

        public void UpdateSchedule(UpdatedProjectSchedule schedule)
        {
            this.projectScheduleRepository.Update(schedule);
            this.projectScheduleRepository.Commit();
        }

        public void UpdateSchedule(List<UpdatedProjectSchedule> schedules)
        {
            this.projectScheduleRepository.Update(schedules);
            this.projectScheduleRepository.Commit();
        }

        public List<ProjectScheduleOverview> GetProjectSchedules(int projectId)
        {
            var schedules = this.projectScheduleRepository.Find(projectId).OrderBy(x => x.Position).ToList();
            return schedules;
        }

        public void AddLog(NewProjectLog log)
        {
            this.projectLogRepository.Add(log);
            this.projectLogRepository.Commit();
        }

        public void DeleteLog(int id)
        {
            this.projectLogRepository.Delete(id);
            this.projectLogRepository.Commit();
        }

        public List<ProjectLogOverview> GetProjectLogs(int projectId)
        {
            var logs = this.projectLogRepository.Find(projectId);
            return logs;
        }

        public void AddCollaborator(NewProjectCollaborator collaborator)
        {
            throw new System.NotImplementedException();
        }

        public void AddCollaborator(List<NewProjectCollaborator> collaborators)
        {
            this.projectCollaboratorRepository.Add(collaborators);
            this.projectCollaboratorRepository.Commit();
        }

        public void AddCollaborator(int projectId, List<int> collaboratorIds)
        {
            this.projectCollaboratorRepository.Add(projectId, collaboratorIds);
            this.projectCollaboratorRepository.Commit();
        }

        public void DeleteCollaborator(int collaboratorId)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCollaborator(List<int> collaboratorIds)
        {
            this.projectCollaboratorRepository.Delete(collaboratorIds);
            this.projectCollaboratorRepository.Commit();
        }

        public List<ProjectCollaboratorOverview> GetProjectCollaborators(int projectId)
        {
            var collaborators = this.projectCollaboratorRepository.Find(projectId);
            return collaborators;
        }

        public void AddFiles(List<NewProjectFile> files)
        {
            this.projectFileRepository.AddFiles(files);
            this.projectFileRepository.Commit();
        }

        public void DeleteFiles(int projectId, string basePath, List<string> deletedRegistrationFiles)
        {
            this.projectFileRepository.DeleteFiles(projectId, basePath, deletedRegistrationFiles);
            this.projectFileRepository.Commit();
        }

        public byte[] GetFileContent(int id,string basePath, string fileName)
        {
            var model = this.projectFileRepository.GetFileContent(id, basePath, fileName);
            return model.Content;
        }

        public bool FileExists(int id, string fileName)
        {
            var exist = this.projectFileRepository.FileExists(id, fileName);

            return exist;
        }

        public List<string> FindFileNamesExcludeSpecified(int id, List<string> deletedFileNames)
        {
            var fileContent = this.projectFileRepository.FindFileNamesExcludeSpecified(id, deletedFileNames);
            return fileContent;
        }
    }
}
