using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Domain;
using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Infrastructure
{
    public interface IFilesStorage
    {
        string SaveFile(byte[] content, string basePath, string fileName, string topic, int entityId);

        void DeleteFile(string topic, int entityId, string basePath, string fileName);

        FileContentModel GetFileContent(string topic, int entityId, string basePath, string fileName);

        byte[] GetFileByteContent(string pathToFile);

        string ComposeFilePath(string topic, int entityId, string basePath, string fileName);

        void MoveDirectory(string topic, string entityId, string sourceBasePath, string targetBasePath);

        string GetCaseFilePath(string topic, int entityId, string basePath, string fileName);

        void DeleteFilesInFolders(List<Case> cases, List<CaseFile> caseFiles, List<LogFile> logFiles, string basePath);
    }
}
