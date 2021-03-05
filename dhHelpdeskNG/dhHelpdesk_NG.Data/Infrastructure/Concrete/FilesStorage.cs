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

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
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

            var targetInfo = new DirectoryInfo(targetDirPath);
            if (targetInfo.Exists == false)
                Directory.CreateDirectory(targetDirPath);

            var sourceInfo = new DirectoryInfo(fromDirPath);
            if (sourceInfo.Exists)
            {
                var sourceSubDirs = sourceInfo.GetDirectories();
                var files = Directory.GetFiles(fromDirPath);
                MoveFiles(targetDirPath, files);

                foreach (var subdir in sourceSubDirs)
                {
                    var targetSubDirPath = Path.Combine(targetDirPath, subdir.Name);
                    if (!Directory.Exists(targetSubDirPath))
                    {
                        var subFiles = Directory.GetFiles(subdir.FullName);
                        MoveFiles(targetSubDirPath, subFiles);
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

        private void MoveFiles(string targetDirPath, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                if (string.IsNullOrWhiteSpace(file)) continue;

                var name = Path.GetFileName(file);
                var destFile = Path.Combine(targetDirPath, name);
                if (!FilteredFiles.Contains(Path.GetFileName(file.ToLower())))
                {
                    if (!Directory.Exists(targetDirPath))
                        Directory.CreateDirectory(targetDirPath);
                    File.Move(file, destFile);
                }
            }
        }
    }
}
