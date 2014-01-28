using System.Collections.Generic;

namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    public interface IWebTemporaryStorage
    {
        bool FileExists(string temporaryId, string fileName, params string[] topics);

        List<WebTemporaryFile> GetFiles(string temporaryId, params string[] topics);

        void Save(byte[] file, string temporaryId, string name, params string[] topics);

        List<string> GetFileNames(string temporaryId, params string[] topics); 

        void DeleteFile(string temporaryId, string fileName, params string[] topics);

        void DeleteFolder(string temporaryId, params string[] topics);

        byte[] GetFileContent(string temporaryId, string fileName, params string[] topics);
    }
}