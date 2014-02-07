namespace DH.Helpdesk.Dal.Repositories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;

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