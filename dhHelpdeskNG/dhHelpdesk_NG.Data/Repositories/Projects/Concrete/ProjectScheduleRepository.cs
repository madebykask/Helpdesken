namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class ProjectScheduleRepository : RepositoryBase<ProjectSchedule>, IProjectScheduleRepository
    {
        public ProjectScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(NewProjectScheduleDto newProject)
        {
            throw new global::System.NotImplementedException();
        }

        public void Delete(int projectId)
        {
            throw new global::System.NotImplementedException();
        }

        public void Update(NewProjectScheduleDto existingProject)
        {
            throw new global::System.NotImplementedException();
        }

        public List<NewProjectScheduleDto> Find(int projectId)
        {
            throw new global::System.NotImplementedException();
        }
    }
}