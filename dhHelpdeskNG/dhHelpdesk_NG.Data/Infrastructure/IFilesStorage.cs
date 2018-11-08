namespace DH.Helpdesk.Dal.Infrastructure
{
    public interface IFilesStorage
    {
        void SaveFile(byte[] content, string basePath, string fileName, string topic, int entityId);

        void DeleteFile(string topic, int entityId, string basePath, string fileName);

        byte[] GetFileContent(string topic, int entityId, string basePath, string fileName);

        string ComposeFilePath(string topic, int entityId, string basePath, string fileName);

        void MoveDirectory(string topic, string entityId, string sourceBasePath, string targetBasePath);

        string GetCaseFilePath(string topic, int entityId, string basePath, string fileName);
    }
}
