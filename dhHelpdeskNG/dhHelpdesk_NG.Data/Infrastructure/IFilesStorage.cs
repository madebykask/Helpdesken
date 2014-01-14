namespace dhHelpdesk_NG.Data.Infrastructure
{
    public interface IFilesStorage
    {
        void SaveFile(byte[] file, string name, string topic, int entityId);

        void DeleteFile(string filename, string topic, int entityId);

        byte[] GetFileContent(string topic, int entityId, string fileName);
    }
}
