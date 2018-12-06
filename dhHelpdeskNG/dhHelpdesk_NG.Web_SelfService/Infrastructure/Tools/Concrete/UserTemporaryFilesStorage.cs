namespace DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete
{
    using DH.Helpdesk.Common.Enums;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public sealed class UserTemporaryFilesStorage : IUserTemporaryFilesStorage
    {
        #region Fields

        private readonly string temporaryDirectory;

        private readonly string topic;

        #endregion

        #region Constructors and Destructors

        public UserTemporaryFilesStorage(string topic)
        {
            //this.temporaryDirectory = HttpContext.Current.Server.MapPath(@"~\App_Data");
            this.temporaryDirectory = ConfigurationManager.AppSettings[AppSettingsKey.FilesDirectory]; 
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
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            this.AddFile(content, fileName, textId, subtopics);
        }

        public List<string> GetFileNames(int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.GetFileNames(textId, subtopics);
        }

        public void DeleteFile(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            File.Delete(filePath);
        }

        public void DeleteFile(string fileName, int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            this.DeleteFile(fileName, textId, subtopics);
        }

        public void DeleteFiles(string objectId)
        {
            var directory = this.ComposeDirectoryPath(objectId);

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public void DeleteFiles(int objectId)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            this.DeleteFiles(textId);
        }

        public bool FileExists(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.Exists(filePath);
        }

        public bool FileExists(string fileName, int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.FileExists(fileName, textId, subtopics);
        }

        public byte[] GetFileContent(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.ReadAllBytes(filePath);
        }

        public byte[] GetFileContent(string fileName, int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.GetFileContent(fileName, textId, subtopics);
        }

        public List<string> GetFileNames(string objectId, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(objectId, subtopics);

            return Directory.Exists(directory)
                       ? Directory.GetFiles(directory).Select(Path.GetFileName).ToList()
                       : new List<string>(0);
        }

        public List<WebTemporaryFile> GetFiles(string objectId, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(objectId, subtopics);

            if (!Directory.Exists(directory))
            {
                return new List<WebTemporaryFile>(0);
            }

            var files = Directory.GetFiles(directory);
            var webFiles = files.Select(f => new WebTemporaryFile(File.ReadAllBytes(f), Path.GetFileName(f))).ToList();

            return webFiles;
        }

        public List<WebTemporaryFile> GetFiles(int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.GetFiles(textId, subtopics);
        }

        #endregion

        #region Methods

        private string ComposeDirectoryPath(string objectId, params string[] subtopics)
        {
            var composedPath = Path.Combine(this.temporaryDirectory, "Temporary", this.topic, objectId);

            foreach (var subtopic in subtopics)
            {
                composedPath = Path.Combine(composedPath, subtopic);
            }

            return composedPath;
        }

        private string ComposeFilePath(string fileName, string objectId, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(objectId, subtopics);
            return Path.Combine(directory, fileName);
        }

        #endregion
    }
}