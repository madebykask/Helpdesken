namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectLogService : IProjectLogService
    {
        private readonly IProjectLogRepository projectLogRepository;

        public ProjectLogService(IProjectLogRepository projectLogRepository)
        {
            this.projectLogRepository = projectLogRepository;
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

        public IList<ProjectLogOverview> GetProjectLogs(int projectId)
        {
            var logs = this.projectLogRepository.Find(projectId);
            return logs;
        }
    }
}