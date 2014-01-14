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
            var filePath = GetFilePath(topic, entityId, fileName);  
            return File.ReadAllBytes(filePath);
        }

        public void SaveFile(byte[] file, string name, string topic, int entityId)
        {
            var saveDirectory = GetDirectoryPath(topic, entityId); 

            Directory.CreateDirectory(saveDirectory);
            var savePath = Path.Combine(saveDirectory, name);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(file, 0, file.Length);
            }
        }

        public void DeleteFile(string filename, string topic, int entityId)
        {
            var filepath = GetFilePath(topic, entityId, filename); 
            if (File.Exists(filepath)) 
                File.Delete(filepath);  
        }

        private string GetFilePath(string topic, int entityId, string fileName)
        {
            return Path.Combine(GetDirectoryPath(topic, entityId), fileName);
        }

        private string GetDirectoryPath(string topic, int entityId)
        {
            return Path.Combine(this.filesDirectory, topic + entityId.ToString(CultureInfo.InvariantCulture));
        }
    }

}
