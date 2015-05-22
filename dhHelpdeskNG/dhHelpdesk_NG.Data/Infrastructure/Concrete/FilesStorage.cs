namespace DH.Helpdesk.Dal.Infrastructure.Concrete
{
    using System.Configuration;
    using System.Globalization;
    using System.IO;

    using DH.Helpdesk.Common.Enums;

    public sealed class FilesStorage : IFilesStorage
    {        

        public FilesStorage()
        {            
        }

        public byte[] GetFileContent(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = this.ComposeFilePath(topic, entityId, basePath,  fileName);  
            return File.ReadAllBytes(filePath);
        }

        public void SaveFile(byte[] content, string basePath, string fileName, string topic, int entityId)
        {
            var saveDirectory = this.ComposeDirectoryPath(basePath, topic, entityId); 
            Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(topic, entityId, basePath,  fileName);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public void DeleteFile(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = this.ComposeFilePath(topic, entityId, basePath,  fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string ComposeFilePath(string topic, int entityId, string basePath, string fileName)
        {
            var directoryPath = this.ComposeDirectoryPath(basePath, topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string basePath, string topic, int entityId)
        {            
            return Path.Combine(basePath, topic + entityId.ToString(CultureInfo.InvariantCulture));
        }
    }
}
