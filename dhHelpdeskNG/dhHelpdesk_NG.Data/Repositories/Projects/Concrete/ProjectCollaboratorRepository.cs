namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectCollaboratorRepository : RepositoryBase<ProjectCollaborator>, IProjectCollaboratorRepository
    {
        public ProjectCollaboratorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public void Add(ProjectCollaboratorDto newProjectCollaborator)
        {
            var projectCollaborator = this.MapFromDto(newProjectCollaborator);
            this.DataContext.ProjectCollaborators.Add(projectCollaborator);
            this.InitializeAfterCommit(newProjectCollaborator, projectCollaborator);
        }

        public void Delete(int projectId)
        {
            var projectCollaborator = this.DataContext.ProjectCollaborators.Find(projectId);
            this.DataContext.ProjectCollaborators.Remove(projectCollaborator);
        }

        public List<ProjectCollaboratorOverview> Find(int projectId)
        {
            throw new global::System.NotImplementedException();
        }

        private ProjectCollaborator MapFromDto(ProjectCollaboratorDto newProjectCollaborator)
        {
            throw new global::System.NotImplementedException();
        }
    }

    public abstract class R2<TDomain, TDto> : RepositoryBase<TDomain>
        where TDomain : Entity
        where TDto : INewEntity
    {
        protected R2(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public abstract DbSet<TDomain> GetDbSet();

        public abstract TDomain MapFromDto(TDto dto);

        public virtual void Add(TDto dto)
        {
            var entity = this.MapFromDto(dto);
            this.GetDbSet().Add(entity);
            this.InitializeAfterCommit(dto, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.GetDbSet().Find(id);
            this.GetDbSet().Remove(entity);
        }
    }
}