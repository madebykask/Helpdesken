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

    public class ProjectScheduleRepository : Repository, IProjectScheduleRepository
    {
        private readonly IBusinessModelToEntityMapper<NewProjectScheduleDto, ProjectSchedule> newModelMapper;

        private readonly IEntityChangerFromBusinessModel<NewProjectScheduleDto, ProjectSchedule> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview> overviewMapper;

        public ProjectScheduleRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProjectScheduleDto, ProjectSchedule> newModelMapper,
            IEntityChangerFromBusinessModel<NewProjectScheduleDto, ProjectSchedule> updatedModelMapper,
            IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProjectScheduleDto businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProjectSchedules.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DbContext.ProjectSchedules.Find(id);
            this.DbContext.ProjectSchedules.Remove(entity);
        }

        public void DeleteByProjectId(int projectId)
        {
            var problemLogs = this.DbContext.ProjectSchedules.Where(x => x.Project_Id == projectId).ToList();
            problemLogs.ForEach(x => this.DbContext.ProjectSchedules.Remove(x));
        }

        public void Update(NewProjectScheduleDto businessModel)
        {
            var entity = this.DbContext.ProjectSchedules.Find(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        public List<ProjectScheduleOverview> Find(int projectId)
        {
            var projectshedules = this.DbContext.ProjectSchedules.Where(x => x.Project_Id == projectId).Select(this.overviewMapper.Map).ToList();
            return projectshedules;
        }
    }
}