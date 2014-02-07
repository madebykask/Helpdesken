namespace DH.Helpdesk.Dal.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain.Projects;

    public class ProjectLogEntityToProjectLogOverviewMapper : IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>
    {
        public ProjectLogOverview Map(ProjectLog entity)
        {
            return new ProjectLogOverview
                       {
                           Id = entity.Id,
                           LogText = entity.LogText,
                           ProjectId = entity.Project_Id,
                           ResponsibleUser = string.Format("{0} {1}", entity.User.FirstName, entity.User.SurName),
                           ChangedDate = entity.ChangeDate
                       };
        }
    }
}