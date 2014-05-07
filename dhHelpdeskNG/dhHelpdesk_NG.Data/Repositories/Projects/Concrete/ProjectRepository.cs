namespace DH.Helpdesk.Dal.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Projects;

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
            var projects = this.DbContext.Projects.Where(x => x.Customer_Id == customerId).Select(this.overviewMapper.Map).OrderBy(x => x.Name).ToList();
            return projects;
        }

        public List<ProjectOverview> Find(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike)
        {
            var toLowerProjectNameLike = projectNameLike == null ? string.Empty : projectNameLike.ToLower();
            var projects = this.DbContext.Projects.Where(x => x.Customer_Id == customerId && x.Name.ToLower().Contains(toLowerProjectNameLike));

            if (projectManagerId.HasValue)
            {
                projects = projects.Where(x => x.ProjectManager.Value == projectManagerId.Value);
            }

            switch (entityStatus)
            {
                case EntityStatus.Active:
                    projects = projects.Where(x => x.IsActive == 1);
                    break;
                case EntityStatus.Inactive:
                    projects = projects.Where(x => x.IsActive == 0);
                    break;
            }

            var projectDtos = projects.OrderBy(x => x.Name);

            return projectDtos.Select(this.overviewMapper.Map)
                                      .ToList();
        }
    }
}
