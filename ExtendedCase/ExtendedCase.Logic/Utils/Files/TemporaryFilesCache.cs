using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Common.Enums;

namespace ExtendedCase.Logic.Utils.Files
{
    public interface ITemporaryFilesCache
    {
        bool FileExists(string fileName, string objectId, params string[] subtopics);
        void AddFile(byte[] content, string fileName, string objectId, params string[] subtopics);
        void DeleteFile(string fileName, string objectId, params string[] subtopics);
        byte[] GetFileContent(string fileName, string objectId, params string[] subtopics);
    }

    public class TemporaryFilesCache : ITemporaryFilesCache
    {
        private readonly string _temporaryDirectory;
        private readonly string _topic;

        public TemporaryFilesCache(string topic)
        {
            this._temporaryDirectory = ConfigurationManager.AppSettings[AppSettingsKey.FilesDirectory]; ;
            this._topic = topic;
        }

        public bool FileExists(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.Exists(filePath);
        }

        public void AddFile(byte[] content, string fileName, string objectId, params string[] subtopics)
        {
            var saveDirectory = this.ComposeDirectoryPath(objectId, subtopics);
            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);
            var savePath = this.ComposeFilePath(fileName, objectId, subtopics);

            using (var fileStream = new FileStream(savePath, FileMode.CreateNew))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public byte[] GetFileContent(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            return File.ReadAllBytes(filePath);
        }

        public void DeleteFile(string fileName, string objectId, params string[] subtopics)
        {
            var filePath = this.ComposeFilePath(fileName, objectId, subtopics);
            File.Delete(filePath);
        }

        private string ComposeDirectoryPath(string objectId, params string[] subtopics)
        {
            var composedPath = Path.Combine(this._temporaryDirectory, "Temporary", this._topic, objectId);

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
    }
}
