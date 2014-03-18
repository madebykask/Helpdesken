namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

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