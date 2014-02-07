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

    public class ProjectScheduleRepository : Repository, IProjectScheduleRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule> newModelMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule> updatedModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview> overviewMapper;

        public ProjectScheduleRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule> newModelMapper,
            IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule> updatedModelMapper,
            IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview> overviewMapper)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.overviewMapper = overviewMapper;
        }

        public virtual void Add(NewProjectSchedule businessModel)
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

        public void Update(UpdatedProjectSchedule businessModel)
        {
            var entity = this.DbContext.ProjectSchedules.Find(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        public void Update(List<UpdatedProjectSchedule> businessModels)
        {
            foreach (var businessModel in businessModels)
            {
                this.Update(businessModel);
            }
        }

        public List<ProjectScheduleOverview> Find(int projectId)
        {
            var projectshedules = this.DbContext.ProjectSchedules.Where(x => x.Project_Id == projectId).Select(this.overviewMapper.Map).ToList();
            return projectshedules;
        }
    }
}