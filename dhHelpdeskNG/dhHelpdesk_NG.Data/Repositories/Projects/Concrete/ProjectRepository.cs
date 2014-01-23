namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectRepository : Repository, IProjectRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProject, Project> newModelMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedProject, Project> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<Project, ProjectOverview> overviewMapper;

        public ProjectRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProject, Project> newModelMapper,
            IBusinessModelToEntityMapper<UpdatedProject, Project> updatedModelMapper,
            IEntityToBusinessModelMapper<Project, ProjectOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProject businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.Projects.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DbContext.Projects.Find(id);
            this.DbContext.Projects.Remove(entity);
        }

        public void Update(UpdatedProject businessModel)
        {
            var entity = this.DbContext.Projects.Find(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        public ProjectOverview FindById(int projectId)
        {
            var project = this.DbContext.Projects.Find(projectId);
            var projectDto = this.overviewMapper.Map(project);
            return projectDto;
        }

        public List<ProjectOverview> Find(int customerId)
        {
            var projects = this.DbContext.Projects.Where(x => x.Customer_Id == customerId).Select(this.overviewMapper.Map).ToList();
            return projects;
        }

        public List<ProjectOverview> Find(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike)
        {
            var toLowerProjectNameLike = projectNameLike.ToLower();
            var projects = this.DbContext.Projects.Where(x => x.Customer_Id == customerId && x.ProjectManager == projectManagerId && x.Name.ToLower().Contains(toLowerProjectNameLike));

            switch (entityStatus)
            {
                case EntityStatus.Active:
                    projects = projects.Where(x => x.IsActive == 1);
                    break;
                case EntityStatus.Inactive:
                    projects = projects.Where(x => x.IsActive == 0);
                    break;
            }

            var projectDtos = projects.OrderBy(x => x.Name)
                                      .Select(this.overviewMapper.Map)
                                      .ToList();
            return projectDtos;
        }
    }
}
