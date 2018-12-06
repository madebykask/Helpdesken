using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Common.Tools.Files
{
    public sealed class TemporaryFilesCache : ITemporaryFilesCache
    {
        #region Fields

        private readonly string temporaryDirectory;

        private readonly string topic;

        #endregion

        #region Constructors and Destructors

        public TemporaryFilesCache(string topic)
        {
            //HttpContext.Current.Server.MapPath(@"~\App_Data");
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

        public List<string> FindFileNames(int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.FindFileNames(textId, subtopics);
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

        public void ResetCacheForObject(string objectId)
        {
            var directory = this.ComposeDirectoryPath(objectId);

            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public void ResetCacheForObject(int objectId)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            this.ResetCacheForObject(textId);
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

        public List<string> FindFileNames(string objectId, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(objectId, subtopics);

            return Directory.Exists(directory)
                       ? Directory.GetFiles(directory).Select(Path.GetFileName).ToList()
                       : new List<string>(0);
        }

        public string FindFilePath(string fileName, string objectId, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(objectId, subtopics);

            return Directory.Exists(directory)
                       ? Directory.GetFiles(directory).FirstOrDefault(f => Path.GetFileName(f) == fileName)
                       : string.Empty;
        }

        public List<CaseFileDate> FindFileNamesAndDates(string id, params string[] subtopics)
        {
            var directory = this.ComposeDirectoryPath(id, subtopics);

            var fileNames = Directory.Exists(directory)
                       ? Directory.GetFiles(directory).Select(Path.GetFileName).ToList()
                       : new List<string>(0);
            return fileNames.Select(x => new CaseFileDate
            {
                FileName = x,
                FileDate = DateTime.Now
            }).ToList();
        }

        public List<WebTemporaryFile> FindFiles(string objectId, params string[] subtopics)
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

        public List<WebTemporaryFile> FindFiles(int objectId, params string[] subtopics)
        {
            var textId = objectId.ToString(CultureInfo.InvariantCulture);
            return this.FindFiles(textId, subtopics);
        }

        #endregion

        #region Private Methods

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