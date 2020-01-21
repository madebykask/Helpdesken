using DH.Helpdesk.BusinessData.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DH.Helpdesk.Dal.Infrastructure.Concrete
{
    public sealed class FilesStorage : IFilesStorage
    {
        private readonly List<string> FilteredFiles = new List<string>();

        public FilesStorage()
        {            
            FilteredFiles.Add("thumbs.db");
        }

        public string GetCaseFilePath(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = ComposeFilePath(topic, entityId, basePath, fileName);
            return filePath;
        }

        public FileContentModel GetFileContent(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = ComposeFilePath(topic, entityId, basePath,  fileName);  
			var content = File.ReadAllBytes(filePath);
			var model = new FileContentModel
			{
				FilePath = filePath,
				Content = content
			};
			return model;
        }

        public string SaveFile(byte[] content, string basePath, string fileName, string topic, int entityId)
        {
            //var saveDirectory = ComposeDirectoryPath(basePath, topic, entityId); 
            var savePath = ComposeFilePath(topic, entityId, basePath,  fileName);
            var directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }

			return savePath;
        }

        public void DeleteFile(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = ComposeFilePath(topic, entityId, basePath,  fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void MoveDirectory(string topic, string entityId, string sourceBasePath, string targetBasePath)
        {
            var fromDirPath = ComposeFilePath(topic, entityId, sourceBasePath, string.Empty);
            var targetDirPath = ComposeFilePath(topic, entityId, targetBasePath, string.Empty);

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
            var directoryPath = ComposeDirectoryPath(basePath, topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string basePath, string topic, int entityId)
        {            
            return Path.Combine(basePath, topic + entityId.ToString(CultureInfo.InvariantCulture));
        }

        public string ComposeFilePath(string topic, string entityId, string basePath, string fileName)
        {
            var directoryPath = ComposeDirectoryPath(basePath, topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string basePath, string topic, string entityId)
        {
            return Path.Combine(basePath, topic + entityId);
        }
    }
}
