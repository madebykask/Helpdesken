using System.Collections.Generic;
using System.Web;

namespace dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete
{
    using System.IO;
    using System.Linq;

    public sealed class WebTemporaryStorage : IWebTemporaryStorage
    {
        private readonly string temporaryDirectory;

        public WebTemporaryStorage()
        {
            this.temporaryDirectory = HttpContext.Current.Server.MapPath(@"~\App_Data");
        }

        public bool FileExists(string topic, string temporaryId, string name)
        {
            var filePath = GetFilePath(topic, temporaryId, name);
            return File.Exists(filePath);
        }

        public List<WebTemporaryFile> GetFiles(string topic, string temporaryId)
        {
            var filesDirectory = GetDirectoryPath(topic, temporaryId);
            
            if (!Directory.Exists(filesDirectory))
            {
                return new List<WebTemporaryFile>(0);
            }
            
            var files = Directory.GetFiles(filesDirectory);
            var webTemporaryFiles = new List<WebTemporaryFile>(files.Count());

            foreach (var file in files)
            {
                var webTemporaryFile = new WebTemporaryFile(File.ReadAllBytes(file), Path.GetFileName(file));
                webTemporaryFiles.Add(webTemporaryFile);
            }

            return webTemporaryFiles;
        }

        public void Save(byte[] file, string topic, string temporaryId, string name)
        {
            var saveDirectory = GetDirectoryPath(topic, temporaryId);
            Directory.CreateDirectory(saveDirectory);
            var savePath = GetFilePath(topic, temporaryId, name);
            
            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(file, 0, file.Length);
            }
        }

        public List<string> GetFileNames(string topic, string temporaryId)
        {
            var filesDirectory = GetDirectoryPath(topic, temporaryId);
            return Directory.GetFiles(filesDirectory).Select(Path.GetFileName).ToList();
        }

        public void DeleteFile(string topic, string temporaryId, string name)
        {
            var filePath = GetFilePath(topic, temporaryId, name);
            File.Delete(filePath);
        }

        public byte[] GetFileContent(string topic, string temporaryId, string name)
        {
            var filePath = GetFilePath(topic, temporaryId, name);
            return File.ReadAllBytes(filePath);
        }

        public string GetDirectoryPath(string topic, string temporaryId)
        {
            return Path.Combine(this.temporaryDirectory, topic, temporaryId);
        }

        private string GetFilePath(string topic, string temporaryId, string fileName)
        {
            return Path.Combine(GetDirectoryPath(topic, temporaryId), fileName);
        }

    }
}