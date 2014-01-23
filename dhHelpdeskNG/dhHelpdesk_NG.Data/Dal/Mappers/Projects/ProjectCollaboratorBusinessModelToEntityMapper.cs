namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class ProjectCollaboratorBusinessModelToEntityMapper : IBusinessModelToEntityMapper<NewProjectCollaboratorDto, ProjectCollaborator>
    {
        public ProjectCollaborator Map(NewProjectCollaboratorDto businessModel)
        {
            return new ProjectCollaborator
                       {
                           Id = businessModel.Id,
                           Project_Id = businessModel.ProjectId,
                           User_Id = businessModel.UserId,
                       };
        }
    }
}