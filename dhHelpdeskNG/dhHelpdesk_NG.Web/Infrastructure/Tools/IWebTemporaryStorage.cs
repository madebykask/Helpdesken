using System.Collections.Generic;

namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    public interface IWebTemporaryStorage
    {
        void DeleteFiles(string objectId, string topic);

        void DeleteFiles(int objectId, string topic);

        bool FileExists(string fileName, string objectId, string topic, params string[] subtopics);

        List<WebTemporaryFile> GetFiles(string objectId, string topic, params string[] subtopics);

        List<WebTemporaryFile> GetFiles(int objectId, string topic, params string[] subtopics);

        void AddFile(byte[] content, string fileName, string objectId, string topic, params string[] subtopics);

        List<string> GetFileNames(string objectId, string topic, params string[] subtopics);

        void DeleteFile(string fileName, string objectId, string topic, params string[] subtopics);

        byte[] GetFileContent(string fileName, string objectId, string topic, params string[] subtopics);
    }
}