using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.Web.Common.Tools.Files
{
    public interface ITemporaryFilesCache
    {
        void ResetCacheForObject(string objectId);

        void ResetCacheForObject(int objectId);

        bool FileExists(string fileName, string objectId, params string[] subtopics);

        bool FileExists(string fileName, int objectId, params string[] subtopics);

        List<WebTemporaryFile> FindFiles(string objectId, params string[] subtopics);

        List<WebTemporaryFile> FindFiles(int objectId, params string[] subtopics);

        void AddFile(byte[] content, string fileName, string objectId, params string[] subtopics);

        void AddFile(byte[] content, string fileName, int objectId, params string[] subtopics);

        List<string> FindFileNames(string objectId, params string[] subtopics);

        List<string> FindFileNames(int objectId, params string[] subtopics);

        void DeleteFile(string fileName, string objectId, params string[] subtopics);

        void DeleteFile(string fileName, int objectId, params string[] subtopics);

        byte[] GetFileContent(string fileName, string objectId, params string[] subtopics);

        byte[] GetFileContent(string fileName, int objectId, params string[] subtopics);

        string FindFilePath(string fileName, string objectId, params string[] subtopics);
        List<CaseFileDate> FindFileNamesAndDates(string id, params string[] subtopics);
    }
}