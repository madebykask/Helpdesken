namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeFileRepository : INewRepository
    {
        void AddFile(NewFile newFile);

        byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName);

        void Delete(int changeId, Subtopic subtopic, string fileName);

        List<string> FindFileNamesByChangeIdAndSubtopic(int changeId, Subtopic subtopic);

        List<string> FindFileNamesExcludeSpecified(int changeId, Subtopic subtopic, List<string> excludeFiles); 

        bool FileExists(int changeId, Subtopic subtopic, string fileName);
    }
}