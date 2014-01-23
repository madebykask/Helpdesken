namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectLogRepository : Repository, IProjectLogRepository
    {
        private readonly IBusinessModelToEntityMapper<NewProjectLogDto, ProjectLog> newModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview> overviewMapper;

        public ProjectLogRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProjectLogDto, ProjectLog> newModelMapper,
            IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProjectLogDto businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProjectLogs.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DbContext.ProjectLogs.Find(id);
            this.DbContext.ProjectLogs.Remove(entity);
        }

        public void DeleteByProjectId(int projectId)
        {
            var problemLogs = this.DbContext.ProjectLogs.Where(x => x.Project_Id == projectId).ToList();
            problemLogs.ForEach(x => this.DbContext.ProjectLogs.Remove(x));
        }

        public List<ProjectLogOverview> Find(int projectId)
        {
            var projectLogs = this.DbContext.ProjectLogs.Where(x => x.Project_Id == projectId);
            var projectLogDtos = projectLogs.Select(this.overviewMapper.Map).ToList();

            return projectLogDtos;
        }
    }
}