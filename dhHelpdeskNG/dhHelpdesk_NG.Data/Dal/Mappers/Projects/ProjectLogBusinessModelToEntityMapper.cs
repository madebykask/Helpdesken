namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class ProjectLogBusinessModelToEntityMapper : IBusinessModelToEntityMapper<NewProjectLogDto, ProjectLog>
    {
        public ProjectLog Map(NewProjectLogDto businessModel)
        {
            return new ProjectLog
                       {
                           Id = businessModel.Id,
                           LogText = businessModel.LogText,
                           Project_Id = businessModel.Id,
                           User_Id = businessModel.ResponsibleUserId
                       };
        }
    }
}