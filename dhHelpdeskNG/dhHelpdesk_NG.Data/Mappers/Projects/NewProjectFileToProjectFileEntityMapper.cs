namespace DH.Helpdesk.Dal.Mappers.Projects
{
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.Domain.Projects;

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