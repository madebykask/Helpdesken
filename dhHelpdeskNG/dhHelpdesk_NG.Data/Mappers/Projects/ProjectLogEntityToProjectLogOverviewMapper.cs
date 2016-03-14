namespace DH.Helpdesk.Dal.Mappers.Projects
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
                           ResponsibleUser = entity.User.FirstName, 
                           ResponsibleUserSurName = entity.User.SurName,
                           ChangedDate = entity.ChangeDate
                       };
        }
    }
}