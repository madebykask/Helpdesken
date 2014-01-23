namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectFileRepository : INewRepository
    {
        bool FileExists(int projectId, string fileName);

        List<string> FindFileNames(int projectId);

        List<ProjectFileOverview> FindFileOverviews(List<int> projectIds);

        byte[] GetFileContent(int projectId, string fileName);

        void Add(NewProjectFileDto businessModel);

        void AddFiles(List<NewProjectFileDto> businessModels);

        void Delete(int projectId);

        void Delete(int projectId, string fileName);
    }
}