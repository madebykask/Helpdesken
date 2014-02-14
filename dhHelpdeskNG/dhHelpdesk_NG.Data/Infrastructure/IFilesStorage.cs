namespace DH.Helpdesk.Dal.Infrastructure
{
    public interface IFilesStorage
    {
        void SaveFile(byte[] content, string fileName, string topic, int entityId);

        void DeleteFile(string topic, int entityId, string fileName);

        byte[] GetFileContent(string topic, int entityId, string fileName);
    }
}
