namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.Enums.Changes;

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