namespace DH.Helpdesk.Dal.Infrastructure.Concrete
{
    using System.Configuration;
    using System.Globalization;
    using System.IO;

    using DH.Helpdesk.Common.Enums;

    public sealed class FilesStorage : IFilesStorage
    {
        private readonly string filesDirectory;

        public FilesStorage()
        {
            this.filesDirectory = ConfigurationManager.AppSettings[AppSettingsKey.FilesDirectory];
        }

        public byte[] GetFileContent(string topic, int entityId, string fileName)
        {
            var filePath = this.ComposeFilePath(topic, entityId, fileName);  
            return File.ReadAllBytes(filePath);
        }

        public void SaveFile(byte[] content, string fileName, string topic, int entityId)
        {
            var saveDirectory = this.ComposeDirectoryPath(topic, entityId); 
            Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(topic, entityId, fileName);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public void DeleteFile(string topic, int entityId, string fileName)
        {
            var filePath = this.ComposeFilePath(topic, entityId, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string ComposeFilePath(string topic, int entityId, string fileName)
        {
            var directoryPath = this.ComposeDirectoryPath(topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string topic, int entityId)
        {
            return Path.Combine(this.filesDirectory, topic + entityId.ToString(CultureInfo.InvariantCulture));
        }
    }
}
