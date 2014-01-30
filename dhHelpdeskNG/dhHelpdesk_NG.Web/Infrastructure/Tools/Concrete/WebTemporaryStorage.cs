namespace dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;

    public sealed class WebTemporaryStorage : IWebTemporaryStorage
    {
        #region Fields

        private readonly string temporaryDirectory;

        #endregion

        #region Constructors and Destructors

        public WebTemporaryStorage()
        {
            this.temporaryDirectory = HttpContext.Current.Server.MapPath(@"~\App_Data");
        }

        #endregion

        #region Public Methods and Operators

        public void AddFile(byte[] content, string fileName, string objectId, string topic, params string[] subtopics)
        {
            var saveDirectory = this.ComposeDirectoryPath(objectId, topic, subtopics);
            Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(fileName, objectId, topic, subtopics);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public void DeleteFile(string fileName, string objectId, string topic, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, topic, subtopics);
            File.Delete(filePath);
        }

        public void DeleteFiles(string objectId, string topic)
        {
            var directoryPath = this.ComposeDirectoryPath(objectId, topic);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public void DeleteFiles(int objectId, string topic)
        {
            this.DeleteFiles(objectId.ToString(CultureInfo.InvariantCulture), topic);
        }

        public bool FileExists(string fileName, string objectId, string topic, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, topic, subtopics);
            return File.Exists(filePath);
        }

        public byte[] GetFileContent(string fileName, string objectId, string topic, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, topic, subtopics);
            return File.ReadAllBytes(filePath);
        }

        public List<string> GetFileNames(string objectId, string topic, params string[] subtopics)
        {
            var filesDirectory = this.ComposeDirectoryPath(objectId, topic, subtopics);

            return Directory.Exists(filesDirectory)
                ? Directory.GetFiles(filesDirectory).Select(Path.GetFileName).ToList()
                : new List<string>(0);
        }

        public List<WebTemporaryFile> GetFiles(string objectId, string topic, params string[] subtopics)
        {
            var filesDirectory = this.ComposeDirectoryPath(objectId, topic, subtopics);

            if (!Directory.Exists(filesDirectory))
            {
                return new List<WebTemporaryFile>(0);
            }

            var files = Directory.GetFiles(filesDirectory);

            var webFiles =
                files.Select(f => new WebTemporaryFile(File.ReadAllBytes(f), Path.GetFileName(f))).ToList();

            return webFiles;
        }

        public List<WebTemporaryFile> GetFiles(int objectId, string topic, params string[] subtopics)
        {
            return this.GetFiles(objectId.ToString(CultureInfo.InvariantCulture), topic, subtopics);
        }

        #endregion

        #region Methods

        private string ComposeDirectoryPath(string objectId, string topic, params string[] subtopics)
        {
            var composedPath = Path.Combine(this.temporaryDirectory, "Uploaded Files", topic, objectId);

            foreach (var subtopic in subtopics)
            {
                composedPath = Path.Combine(composedPath, subtopic);
            }

            return composedPath;
        }

        private string ComposeFilePath(string fileName, string objectId, string topic, params string[] subtopics)
        {
            var directoryPath = this.ComposeDirectoryPath(objectId, topic, subtopics);
            return Path.Combine(directoryPath, fileName);
        }

        #endregion
    }
}