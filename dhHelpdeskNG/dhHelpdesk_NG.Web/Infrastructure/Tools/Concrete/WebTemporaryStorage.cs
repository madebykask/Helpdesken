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

        public bool FileExists(string temporaryId, string fileName, params string[] topics)
        {
            var filePath = this.ComposeFilePath(temporaryId, fileName, topics);
            return File.Exists(filePath);
        }

        public List<WebTemporaryFile> GetFiles(string temporaryId, params string[] topics)
        {
            var filesDirectory = this.ComposeDirectoryPath(temporaryId, topics);

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

        public void Save(byte[] file, string temporaryId, string name, params string[] topics)
        {
            var saveDirectory = this.ComposeDirectoryPath(temporaryId, topics);
            Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(temporaryId, name, topics);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(file, 0, file.Length);
            }
        }

        public List<string> GetFileNames(string temporaryId, params string[] topics)
        {
            var filesDirectory = this.ComposeDirectoryPath(temporaryId, topics);
            return Directory.GetFiles(filesDirectory).Select(Path.GetFileName).ToList();
        }

        public void DeleteFile(string temporaryId, string fileName, params string[] topics)
        {
            var filePath = this.ComposeFilePath(temporaryId, fileName, topics);
            File.Delete(filePath);
        }

        public void DeleteFolder(string temporaryId, params string[] topics)
        {
            var directoryPath = this.ComposeDirectoryPath(temporaryId, topics);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public byte[] GetFileContent(string temporaryId, string fileName, params string[] topics)
        {
            var filePath = this.ComposeFilePath(temporaryId, fileName, topics);
            return File.ReadAllBytes(filePath);
        }

        private string ComposeDirectoryPath(string temporaryId, params string[] topics)
        {
            var composedPath = this.temporaryDirectory;

            foreach (var topic in topics)
            {
                composedPath = Path.Combine(composedPath, topic);
            }

            return Path.Combine(composedPath, temporaryId);
        }

        private string ComposeFilePath(string temporaryId, string fileName, params string[] topics)
        {
            var directoryPath = this.ComposeDirectoryPath(temporaryId, topics);
            return Path.Combine(directoryPath, fileName);
        }
    }
}