using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Projects;
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectRepository : Repository, IProjectRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProject, Project> _newModelMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedProject, Project> _updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<Project, ProjectOverview> _overviewMapper;

        public ProjectRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProject, Project> newModelMapper,
            IBusinessModelToEntityMapper<UpdatedProject, Project> updatedModelMapper,
            IEntityToBusinessModelMapper<Project, ProjectOverview> overviewMapper)
            : base(databaseFactory)
        {
            this._newModelMapper = newModelMapper;
            this._updatedModelMapper = updatedModelMapper;
            this._overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProject businessModel)
        {
            var entity = this._newModelMapper.Map(businessModel);
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
            this._updatedModelMapper.Map(businessModel, entity);
        }

        public ProjectOverview FindById(int projectId)
        {
            var project = this.DbContext.Projects.Find(projectId);
            var projectDto = this._overviewMapper.Map(project);
            return projectDto;
        }

        public List<ProjectOverview> Find(int customerId)
        {
            // this is performance optimised implemetnation. pls do not use mapper
            //doesn't need include when using select with fields on Iquerable
            var projects =
                this.DbContext.Projects
                    .Where(p => p.Customer_Id == customerId)
                    .Select(p => new ProjectOverview
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        CustomerId = p.Customer_Id,

                        // todo 
                        StartDate = p.CreatedDate,

                        EndDate = p.EndDate,
                        IsActive = p.IsActive,
                        ProjectManagerId = p.ProjectManager,
                        ProjectManagerName = p.Manager == null ? "" : p.Manager.FirstName, // keep "" for Linq correct translation
                        ProjectManagerSurName = p.Manager == null ? "" : p.Manager.SurName // keep "" for Linq correct translation
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

            return projects;
        }



        public List<ProjectOverview> Find(
            int customerId,
            EntityStatus entityStatus,
            int? projectManagerId,
            string projectNameLike,
            SortField sortField,
            bool isFirstName
            )
        {
            var toLowerProjectNameLike = projectNameLike?.ToLower() ?? string.Empty;
            IQueryable<Project> query =
                this.DbContext.Projects.Include(x => x.Manager)
                    .Where(x => x.Customer_Id == customerId && x.Name.ToLower().Contains(toLowerProjectNameLike))
                    .OrderBy(x => x.Id);

            if (projectManagerId.HasValue)
            {
                query = query.Where(x => x.ProjectManager.Value == projectManagerId.Value);
            }

            switch (entityStatus)
            {
                case EntityStatus.Active:
                    query = query.Where(x => x.IsActive == 1);
                    break;
                case EntityStatus.Inactive:
                    query = query.Where(x => x.IsActive == 0);
                    break;
            }

            if (sortField != null)
            {
                switch (sortField.SortBy)
                {
                    case SortBy.Ascending:
                        if (sortField.Name == ProjectFields.Number)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else if (sortField.Name == ProjectFields.Name)
                        {
                            query = query.OrderBy(x => x.Name);
                        }
                        else if (sortField.Name == ProjectFields.Manager)
                        {
                            if (isFirstName)
                                query = query.OrderBy(x => x.Manager.FirstName).ThenBy(x => x.Manager.SurName);
                            else
                                query = query.OrderBy(x => x.Manager.SurName).ThenBy(x => x.Manager.FirstName);
                        }
                        else if (sortField.Name == ProjectFields.Date)
                        {
                            query = query.OrderBy(x => x.CreatedDate);
                        }
                        else if (sortField.Name == ProjectFields.ClosingDate)
                        {
                            query = query.OrderBy(x => x.EndDate);
                        }
                        else if (sortField.Name == ProjectFields.Description)
                        {
                            query = query.OrderBy(x => x.Description);
                        }

                        break;
                    case SortBy.Descending:
                        if (sortField.Name == ProjectFields.Number)
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }
                        else if (sortField.Name == ProjectFields.Name)
                        {
                            query = query.OrderByDescending(x => x.Name);
                        }
                        else if (sortField.Name == ProjectFields.Manager)
                        {
                            if (isFirstName)
                                query =
                                    query.OrderByDescending(x => x.Manager.FirstName)
                                        .ThenBy(x => x.Manager.SurName);
                            else
                                query =
                                query.OrderByDescending(x => x.Manager.SurName)
                                    .ThenBy(x => x.Manager.FirstName);                            
                        }
                        else if (sortField.Name == ProjectFields.Date)
                        {
                            query = query.OrderByDescending(x => x.CreatedDate);
                        }
                        else if (sortField.Name == ProjectFields.ClosingDate)
                        {
                            query = query.OrderByDescending(x => x.EndDate);
                        }
                        else if (sortField.Name == ProjectFields.Description)
                        {
                            query = query.OrderByDescending(x => x.Description);
                        }

                        break;
                }
            }

            //this performance optimised version
            //please do use mappers
            var projects =
                query.Select(p => new ProjectOverview
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        CustomerId = p.Customer_Id,

                        // todo 
                        StartDate = p.CreatedDate,

                        EndDate = p.EndDate,
                        IsActive = p.IsActive,
                        ProjectManagerId = p.ProjectManager,
                        ProjectManagerName = p.Manager == null ? "" : p.Manager.FirstName, // keep "" for Linq correct translation
                        ProjectManagerSurName = p.Manager == null ? "" : p.Manager.SurName // keep "" for Linq correct translation
                    })
                .ToList();

            return projects;
        }
    }
}
