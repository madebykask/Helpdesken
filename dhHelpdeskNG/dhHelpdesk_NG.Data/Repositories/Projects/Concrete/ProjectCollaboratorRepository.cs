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

    public class ProjectCollaboratorRepository : Repository, IProjectCollaboratorRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator> newModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview> overviewMapper;

        public ProjectCollaboratorRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator> newModelMapper,
            IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public void Add(NewProjectCollaborator businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProjectCollaborators.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Add(List<NewProjectCollaborator> businessModels)
        {
            foreach (var businessModel in businessModels)
            {
                this.Add(businessModel);
            }
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.ProjectCollaborators.Find(id);
            this.DbContext.ProjectCollaborators.Remove(entity);
        }

        public void Delete(List<int> ids)
        {
            foreach (var id in ids)
            {
                this.Delete(id);
            }
        }

        public void DeleteByProjectId(int projectId)
        {
            var problemLogs = this.DbContext.ProjectCollaborators.Where(x => x.Project_Id == projectId).ToList();
            problemLogs.ForEach(x => this.DbContext.ProjectCollaborators.Remove(x));
        }

        public List<ProjectCollaboratorOverview> Find(int projectId)
        {
            var projectLogs = this.DbContext.ProjectCollaborators.Where(x => x.Project_Id == projectId);
            var projectLogDtos = projectLogs.Select(this.overviewMapper.Map).ToList();

            return projectLogDtos;
        }
    }
}