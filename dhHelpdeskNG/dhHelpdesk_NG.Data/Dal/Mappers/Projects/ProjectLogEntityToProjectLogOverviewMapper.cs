namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

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