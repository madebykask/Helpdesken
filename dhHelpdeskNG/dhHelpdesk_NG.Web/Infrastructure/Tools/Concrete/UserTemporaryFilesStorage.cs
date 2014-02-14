namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;

    public sealed class UserTemporaryFilesStorage : IUserTemporaryFilesStorage
    {
        #region Fields

        private readonly string temporaryDirectory;

        private readonly string topic;

        #endregion

        #region Constructors and Destructors

        public UserTemporaryFilesStorage(string topic)
        {
            this.temporaryDirectory = HttpContext.Current.Server.MapPath(@"~\App_Data");
            this.topic = topic;
        }

        #endregion

        #region Public Methods and Operators

        public void AddFile(byte[] content, string fileName, string objectId, params string[] subtopics)
        {
            var saveDirectory = this.ComposeDirectoryPath(objectId, subtopics);
            Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(fileName, objectId, subtopics);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public void AddFile(byte[] content, string fileName, int objectId, params string[] subtopics)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetFileNames(int objectId, params string[] subtopics)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            File.Delete(filePath);
        }

        public void DeleteFile(string fileName, int objectId, params string[] subtopics)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFiles(string objectId)
        {
            var directoryPath = this.ComposeDirectoryPath(objectId);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public void DeleteFiles(int objectId)
        {
            this.DeleteFiles(objectId.ToString(CultureInfo.InvariantCulture));
        }

        public bool FileExists(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.Exists(filePath);
        }

        public bool FileExists(string fileName, int objectId, params string[] subtopics)
        {
            throw new System.NotImplementedException();
        }

        public byte[] GetFileContent(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.ReadAllBytes(filePath);
        }

        public byte[] GetFileContent(string fileName, int objectId, params string[] subtopics)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetFileNames(string objectId, params string[] subtopics)
        {
            var filesDirectory = this.ComposeDirectoryPath(objectId, subtopics);

            return Directory.Exists(filesDirectory)
                ? Directory.GetFiles(filesDirectory).Select(Path.GetFileName).ToList()
                : new List<string>(0);
        }

        public List<WebTemporaryFile> GetFiles(string objectId, params string[] subtopics)
        {
            var filesDirectory = this.ComposeDirectoryPath(objectId, subtopics);

            if (!Directory.Exists(filesDirectory))
            {
                return new List<WebTemporaryFile>(0);
            }

            var files = Directory.GetFiles(filesDirectory);

            var webFiles =
                files.Select(f => new WebTemporaryFile(File.ReadAllBytes(f), Path.GetFileName(f))).ToList();

            return webFiles;
        }

        public List<WebTemporaryFile> GetFiles(int objectId, params string[] subtopics)
        {
            return this.GetFiles(objectId.ToString(CultureInfo.InvariantCulture), subtopics);
        }

        #endregion

        #region Methods

        private string ComposeDirectoryPath(string objectId, params string[] subtopics)
        {
            var composedPath = Path.Combine(this.temporaryDirectory, "Uploaded Files", this.topic, objectId);

            foreach (var subtopic in subtopics)
            {
                composedPath = Path.Combine(composedPath, subtopic);
            }

            return composedPath;
        }

        private string ComposeFilePath(string fileName, string objectId, params string[] subtopics)
        {
            var directoryPath = this.ComposeDirectoryPath(objectId, subtopics);
            return Path.Combine(directoryPath, fileName);
        }

        #endregion
    }
}