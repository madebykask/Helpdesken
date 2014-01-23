namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

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

        public IList<ProjectOverview> GetCustomerProjects(int customerId)
        {
            var projects = this.projectRepository.Find(customerId);
            return projects;
        }

        public IList<ProjectOverview> GetCustomerProjects(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike)
        {
            var projects = this.projectRepository.Find(customerId, entityStatus, projectManagerId, projectNameLike);
            return projects;
        }
    }
}
