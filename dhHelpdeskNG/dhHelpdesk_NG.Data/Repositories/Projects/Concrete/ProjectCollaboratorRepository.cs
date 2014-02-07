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

        public void Add(int projectId, List<int> collaboratorIds)
        {
            // todo
            var members = this.DbContext.ProjectCollaborators.Where(x => x.Project_Id == projectId).Select(x => x.Id).ToList();
            this.Delete(members);

            foreach (var item in collaboratorIds)
            {
                this.Add(new NewProjectCollaborator(item, projectId));
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
            var projectCollaborators = this.DbContext.ProjectCollaborators.Where(x => x.Project_Id == projectId).ToList();
            projectCollaborators.ForEach(x => this.DbContext.ProjectCollaborators.Remove(x));
        }

        public List<ProjectCollaboratorOverview> Find(int projectId)
        {
            var projectCollaborators = this.DbContext.ProjectCollaborators.Where(x => x.Project_Id == projectId);
            var projectCollaboratoDtos = projectCollaborators.Select(this.overviewMapper.Map).ToList();

            return projectCollaboratoDtos;
        }
    }
}