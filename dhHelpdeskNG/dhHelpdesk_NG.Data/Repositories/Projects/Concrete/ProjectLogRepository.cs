namespace DH.Helpdesk.Dal.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Dal.Mappers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectLogRepository : Repository, IProjectLogRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog> newModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview> overviewMapper;

        public ProjectLogRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog> newModelMapper,
            IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProjectLog businessModel)
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