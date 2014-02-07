namespace DH.Helpdesk.Dal.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

    public class NewProjectCollaboratorlToProjectCollaboratorEntityMapper : INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>
    {
        public ProjectCollaborator Map(NewProjectCollaborator businessModel)
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