using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Models.Files;

namespace ExtendedCase.Logic.Services
{
    public interface IFilesStorageService
    {
        void DeleteFile(string topic, int entityId, string basePath, string fileName);
        string SaveFile(byte[] content, string basePath, string fileName, string topic, int entityId);
        FileContentModel GetFileContent(string topic, int entityId, string basePath, string fileName);
        string GetCaseFilePath(string topic, int entityId, string basePath, string fileName);
    }

    public class FilesStorageService: IFilesStorageService
    {
        public void DeleteFile(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = ComposeFilePath(topic, entityId, basePath,  fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
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

        private string ComposeFilePath(string topic, int entityId, string basePath, string fileName)
        {
            var directoryPath = ComposeDirectoryPath(basePath, topic, entityId);
            return Path.Combine(directoryPath, fileName.Trim());
        }

        private string ComposeDirectoryPath(string basePath, string topic, int entityId)
        {            
            return Path.Combine(basePath, topic + entityId.ToString(CultureInfo.InvariantCulture));
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

        public string GetCaseFilePath(string topic, int entityId, string basePath, string fileName)
        {
            var filePath = ComposeFilePath(topic, entityId, basePath, fileName);
            return filePath;
        }
    }
}
