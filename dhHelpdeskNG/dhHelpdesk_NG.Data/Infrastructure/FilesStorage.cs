namespace dhHelpdesk_NG.Data.Infrastructure
{
    using System.Configuration;
    using System.Globalization;
    using System.IO;

    using dhHelpdesk_NG.Common.Enums;
    using dhHelpdesk_NG.Data.Enums;

    public sealed class FilesStorage : IFilesStorage
    {
        private readonly string filesDirectory;

        public FilesStorage()
        {
            this.filesDirectory = ConfigurationManager.AppSettings[AppSettingsKey.FilesDirectory];
        }

        public byte[] GetFileContent(string topic, int entityId, string fileName)
        {
            var filePath = GetPathForFile(topic, entityId, fileName);  
            return File.ReadAllBytes(filePath);
        }

        public void SaveFile(byte[] file, string name, string topic, int entityId)
        {
            var saveDirectory = GetFileDirectory(topic, entityId); 

            Directory.CreateDirectory(saveDirectory);
            var savePath = Path.Combine(saveDirectory, name);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(file, 0, file.Length);
            }
        }

        private string GetPathForFile(string topic, int entityId, string fileName)
        {
            return Path.Combine(GetFileDirectory(topic , entityId), fileName);
        }

        private string GetFileDirectory(string topic, int entityId)
        {
            return Path.Combine(this.filesDirectory, topic + entityId.ToString(CultureInfo.InvariantCulture));
        }
    }

}
