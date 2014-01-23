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

        private readonly ICaseHistoryRepository caseHistoryRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectLogRepository projectLogRepository,
            IProjectScheduleRepository projectScheduleRepository,
            IProjectFileRepository projectFileRepository,
            IProjectCollaboratorRepository projectCollaboratorRepository,
            ICaseRepository caseRepository,
            ICaseHistoryRepository caseHistoryRepository)
        {
            this.projectRepository = projectRepository;
            this.projectLogRepository = projectLogRepository;
            this.projectScheduleRepository = projectScheduleRepository;
            this.projectFileRepository = projectFileRepository;
            this.projectCollaboratorRepository = projectCollaboratorRepository;
            this.caseRepository = caseRepository;
            this.caseHistoryRepository = caseHistoryRepository;
        }

        public void AddProject(NewProjectDto project)
        {
            this.projectRepository.Add(project);
            this.projectRepository.Commit();
        }

        public void DeleteProject(int id)
        {
            this.caseHistoryRepository.SetNullProblemByProblemId(id);
            this.caseHistoryRepository.Commit();

            this.caseRepository.SetNullProblemByProblemId(id);
            this.caseRepository.Commit();

            this.projectLogRepository.Delete(id);
            this.projectLogRepository.Commit();

            this.projectScheduleRepository.Delete(id);
            this.projectScheduleRepository.Commit();

            this.projectFileRepository.Delete(id);
            this.projectFileRepository.Commit();

            this.projectCollaboratorRepository.Delete(id);
            this.projectCollaboratorRepository.Commit();

            this.projectRepository.Delete(id);
            this.projectRepository.Commit();
        }

        public void UpdateProject(NewProjectDto project)
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
