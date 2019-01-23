namespace DH.Helpdesk.Dal.Infrastructure.Concrete
{
    using System.Globalization;
    using System.IO;
    using System.Collections.Generic;

    public sealed class FilesStorage : IFilesStorage
    {
        private readonly List<string> FilteredFiles = new List<string>();

        public FilesStorage()
        {            
            FilteredFiles.Add("thumbs.db");
        }

        public string GetCaseFilePath(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = this.ComposeFilePath(topic, entityId, basePath, fileName);
            return filePath;
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

        public void MoveDirectory(string topic, string entityId, string sourceBasePath, string targetBasePath)
        {
            var fromDirPath = this.ComposeFilePath(topic, entityId, sourceBasePath, string.Empty);
            var targetDirPath = this.ComposeFilePath(topic, entityId, targetBasePath, string.Empty);

            if (Directory.Exists(fromDirPath))
            {
                if (!Directory.Exists(targetDirPath))
                    Directory.CreateDirectory(targetDirPath);
                

                if (Directory.Exists(targetDirPath))
                {
                    var files = Directory.GetFiles(fromDirPath);
                    foreach (var file in files)
                        if (!FilteredFiles.Contains(Path.GetFileName(file.ToLower())))
                            File.Move(file, file.Replace(sourceBasePath, targetBasePath));

                    var subDir1 = Path.Combine(fromDirPath, "html");
                    if (Directory.Exists(subDir1))
                    {
                        var targetSubDir1 = subDir1.Replace(sourceBasePath, targetBasePath);
                        if (!Directory.Exists(targetSubDir1))                        
                            Directory.CreateDirectory(targetSubDir1);                        

                        if (Directory.Exists(targetSubDir1))
                        {
                            var subFiles = Directory.GetFiles(subDir1);
                            foreach (var file in subFiles)
                                if (!FilteredFiles.Contains(Path.GetFileName(file.ToLower())))
                                    File.Move(file, file.Replace(sourceBasePath, targetBasePath));
                        }
                    }
                }
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

        public string ComposeFilePath(string topic, string entityId, string basePath, string fileName)
        {
            var directoryPath = this.ComposeDirectoryPath(basePath, topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string basePath, string topic, string entityId)
        {
            return Path.Combine(basePath, topic + entityId);
        }
    }
}
