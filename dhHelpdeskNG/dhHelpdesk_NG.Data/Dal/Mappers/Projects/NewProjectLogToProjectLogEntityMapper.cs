namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class NewProjectLogToProjectLogEntityMapper : INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>
    {
        public ProjectLog Map(NewProjectLog businessModel)
        {
            return new ProjectLog
                       {
                           Id = businessModel.Id,
                           LogText = businessModel.LogText,
                           Project_Id = businessModel.ProjectId,
                           User_Id = businessModel.ResponsibleUserId,
                           CreatedDate = businessModel.CreatedDate
                       };
        }
    }
}