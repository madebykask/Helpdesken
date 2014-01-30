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

        List<string> FindFileNamesExcludeSpecified(int projectId, List<string> excludeFiles);

        byte[] GetFileContent(int projectId, string fileName);

        void Add(NewProjectFile businessModel);

        void AddFiles(List<NewProjectFile> businessModels);

        void Delete(int projectId);

        void Delete(int projectId, string fileName);

        void DeleteFiles(int projectId, List<string> fileNames);
    }
}