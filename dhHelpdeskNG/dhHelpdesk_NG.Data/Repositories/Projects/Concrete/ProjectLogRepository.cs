namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectLogRepository : RepositoryBase<ProjectLog>, IProjectLogRepository
    {
        public ProjectLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public ProjectLog MapFromDto(NewProjectLogDto newProjectLog)
        {
            return new ProjectLog
                       {
                           Id = newProjectLog.Id,
                           LogText = newProjectLog.LogText,
                           Project_Id = newProjectLog.Id,
                           User_Id = newProjectLog.ResponsibleUserId
                       };
        }

        public NewProjectLogOverview MapToOverview(ProjectLog projectLog)
        {
            return new NewProjectLogOverview
            {
                Id = projectLog.Id,
                LogText = projectLog.LogText,
                ProjectId = projectLog.Id,
                ResponsibleUser = string.Format("{0} {1}", projectLog.User.FirstName, projectLog.User.SurName),
                ChangedDate = projectLog.ChangeDate
            };
        }

        public void Add(NewProjectLogDto newProject)
        {
            var projectLog = this.MapFromDto(newProject);
            this.DataContext.ProjectLogs.Add(projectLog);
            this.InitializeAfterCommit(newProject, projectLog);
        }

        public void Delete(int projectId)
        {
            var projectLog = this.DataContext.ProjectLogs.Find(projectId);
            this.DataContext.ProjectLogs.Remove(projectLog);
        }

        public List<NewProjectLogOverview> FindByProjectId(int projectId)
        {
            var projectLogs = this.DataContext.ProjectLogs.Where(x => x.Project_Id == projectId);
            var projectLogDtos = projectLogs.Select(this.MapToOverview).ToList();
            return projectLogDtos;
        }
    }
}