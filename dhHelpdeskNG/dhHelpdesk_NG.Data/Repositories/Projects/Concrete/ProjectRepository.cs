namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public Project MapFromDto(NewProjectDto newProjectDto)
        {
            return new Project
                       {
                           Id = newProjectDto.Id,
                           Name = newProjectDto.Name,
                           Description = newProjectDto.Description,
                           Customer_Id = newProjectDto.CustomerId,
                           FinishDate = newProjectDto.FinishDate,
                           IsActive = newProjectDto.IsActive,
                           ProjectManager = newProjectDto.ProjectManagerId
                       };
        }

        public NewProjectOverview MapToOverview(Project project)
        {
            return new NewProjectOverview
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CustomerId = project.Customer_Id,
                FinishDate = project.FinishDate,
                IsActive = project.IsActive,
                ProjectManagerId = project.ProjectManager
            };
        }

        public void Add(NewProjectDto newProject)
        {
            var project = this.MapFromDto(newProject);
            this.DataContext.Projects.Add(project);
            this.InitializeAfterCommit(newProject, project);
        }

        public void Delete(int projectId)
        {
            var project = this.DataContext.Projects.Find(projectId);
            this.DataContext.Projects.Remove(project);
        }

        public void Update(NewProjectDto existingProject)
        {
            var project = this.DataContext.Projects.Find(existingProject.Id);

            project.Name = existingProject.Name;
            project.Description = existingProject.Description;
            project.Customer_Id = existingProject.CustomerId;
            project.FinishDate = existingProject.FinishDate;
            project.IsActive = existingProject.IsActive;
            project.ProjectManager = existingProject.ProjectManagerId;
        }

        public NewProjectOverview FindById(int projectId)
        {
            var project = this.DataContext.Projects.Find(projectId);
            var projectDto = this.MapToOverview(project);
            return projectDto;
        }

        public List<NewProjectOverview> Find(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike)
        {
            var toLowerProjectNameLike = projectNameLike.ToLower();
            var projects = this.GetMany(x => x.Customer_Id == customerId && x.ProjectManager == projectManagerId && x.Name.ToLower().Contains(toLowerProjectNameLike));

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
                                      .Select(this.MapToOverview)
                                      .ToList();
            return projectDtos;
        }
    }
}
