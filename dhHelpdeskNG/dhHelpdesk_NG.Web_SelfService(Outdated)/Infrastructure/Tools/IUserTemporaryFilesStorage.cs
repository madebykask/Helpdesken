namespace DH.Helpdesk.SelfService.Infrastructure.Tools
{
    using System.Collections.Generic;

    public interface IUserTemporaryFilesStorage
    {
        void DeleteFiles(string objectId);

        void DeleteFiles(int objectId);

        bool FileExists(string fileName, string objectId, params string[] subtopics);

        bool FileExists(string fileName, int objectId, params string[] subtopics);

        List<WebTemporaryFile> GetFiles(string objectId, params string[] subtopics);

        List<WebTemporaryFile> GetFiles(int objectId, params string[] subtopics);

        void AddFile(byte[] content, string fileName, string objectId, params string[] subtopics);

        void AddFile(byte[] content, string fileName, int objectId, params string[] subtopics);

        List<string> GetFileNames(string objectId, params string[] subtopics);

        List<string> GetFileNames(int objectId, params string[] subtopics);

        void DeleteFile(string fileName, string objectId, params string[] subtopics);

        void DeleteFile(string fileName, int objectId, params string[] subtopics);

        byte[] GetFileContent(string fileName, string objectId, params string[] subtopics);

        byte[] GetFileContent(string fileName, int objectId, params string[] subtopics);
    }
}