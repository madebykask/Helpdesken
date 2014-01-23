namespace dhHelpdesk_NG.Data.Dal.Mappers.Projects
{
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;

    public class NewProjectFileToProjectFileEntityMapper : INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>
    {
        public ProjectFile Map(NewProjectFile businessModel)
        {
            return new ProjectFile
                       {
                           CreatedDate = businessModel.CreatedDate,
                           Project_Id = businessModel.ProjectId,
                           FileName = businessModel.Name,
                       };
        }
    }
}