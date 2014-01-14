using System.Collections.Generic;

namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    public interface IWebTemporaryStorage
    {
        bool FileExists(string topic, string temporaryId, string name);

        List<WebTemporaryFile> GetFiles(string topic, string temporaryId);

        void Save(byte[] file, string topic, string temporaryId, string name);

        List<string> GetFileNames(string topic, string temporaryId);

        void DeleteFile(string topic, string temporaryId, string name);

        byte[] GetFileContent(string topic, string temporaryId, string name);

        string GetDirectoryPath(string topic, string temporaryId);
    }
}