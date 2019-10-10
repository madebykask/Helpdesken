using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Services.Services
{
    public interface ICaseFileService
    {
        CaseFileContent GetCaseFile(int customerId, int caseId, int fileId, bool embedImmges = false);
        FileContentModel GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        CaseFileModel GetCaseFile(int caseId, int fileId);
        IList<string> FindFileNamesByCaseId(int caseId);
        int AddFile(CaseFileDto caseFileDto, ref string path);
        IList<int> AddFiles(IList<CaseFileDto> caseFileDtos, List<KeyValuePair<CaseFileDto, string>> paths);
        void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath);
        bool FileExists(int caseId, string fileName);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName);

        IList<CaseFileModel> GetCaseFiles(int caseId, bool canDelete);
        IList<CaseFileDate> FindFileNamesAndDatesByCaseId(int caseId);
    }

    public class CaseFileService : ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly ISettingsLogic _settingsLogic;

        #region ctor()

        public CaseFileService(ICaseFileRepository caseFileRepository, ISettingsLogic settingsLogic)
        {
            _caseFileRepository = caseFileRepository;
            _settingsLogic = settingsLogic;
        }

        #endregion

        #region Public Methods

        public CaseFileModel GetCaseFile(int caseId, int fileId)
        {
            var fileInfo = _caseFileRepository.GetCaseFileInfo(caseId, fileId);
            return fileInfo;
        }

        public CaseFileContent GetCaseFile(int customerId, int caseId, int fileId, bool embedImmges = false)
        {
            var basePath = GetFileAttachFolderPath(customerId);
            var res = _caseFileRepository.GetCaseFileContent(caseId, fileId, basePath);

            var caseFilePath = _caseFileRepository.GetCaseFilePath(caseId, fileId, basePath);
            res.FilePath = caseFilePath;
            if (embedImmges && Path.GetExtension(res.FileName ?? string.Empty).Equals(".htm", StringComparison.OrdinalIgnoreCase))
                res.Content = EmbedFilesIntoHtml(caseFilePath, res.Content);
            
            return res;
        }

        private byte[] EmbedFilesIntoHtml(string filePath, byte[] fileData)
        {
            if (fileData == null)
                return null;

            var hasChanged = false;
            var res = fileData;

            var content = System.Text.Encoding.Unicode.GetString(fileData, 0, fileData.Length);

            var dirName = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dirName))
            {
                foreach (var file in Directory.GetFiles(dirName))
                {
                    var fileName = Path.GetFileName(file);
                    if (Path.GetExtension(fileName).EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
                        continue;

                    var imgData = CreateImageSrcData(file);
                    content = content.Replace(fileName, imgData);
                    hasChanged = true;
                }
            }

            if (hasChanged)
                res = System.Text.Encoding.Unicode.GetBytes(content);

            return res;
        }

        private string CreateImageSrcData(string filename)
        {
            var mimeType = MimeMapping.GetMimeMapping(filename);
            string srcData;

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                srcData = Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
            }
            return $"data:{mimeType};base64," + srcData;
        }

        public CaseFileContent GetCaseFile(int customerId, int caseId, int fileId )
        {
            var basePath = GetFileAttachFolderPath(customerId);
            var res = _caseFileRepository.GetCaseFileContent(caseId, fileId, basePath);
            return res;
        }
        
        public FileContentModel GetFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            return _caseFileRepository.GetFileContentByIdAndFileName(caseId, basePath, fileName);
        }

        public IList<int> AddFiles(IList<CaseFileDto> caseFileDtos, List<KeyValuePair<CaseFileDto, string>> paths)
        {
            var fileIds = new List<int>();
            foreach (var fileDto in caseFileDtos)
            {
				string path = "";
                var id = AddFile(fileDto, ref path);
                fileIds.Add(id);

				paths.Add(new KeyValuePair<CaseFileDto, string>(fileDto, path));
            }
            return fileIds;
        }

        public int AddFile(CaseFileDto caseFileDto, ref string path)
        {
            return _caseFileRepository.SaveCaseFile(caseFileDto, ref path);
        }

        public void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath)
        {
            _caseFileRepository.MoveCaseFiles(caseNumber, fromBasePath, toBasePath);
        }

        public void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName)
        {
            _caseFileRepository.DeleteByCaseIdAndFileName(caseId, basePath, fileName);
        }

        public IList<CaseFileModel> GetCaseFiles(int caseId, bool canDelete)
        {
            return _caseFileRepository.GetCaseFiles(caseId, canDelete);
        }

        public IList<CaseFileDate> FindFileNamesAndDatesByCaseId(int caseId)
        {
            var files = _caseFileRepository.GetCaseFilesByCaseId(caseId);

            return files.Select(x => new CaseFileDate
            {
                FileDate = x.CreatedDate,
                FileName = x.FileName
            }).ToList();
        }

        public IList<string> FindFileNamesByCaseId(int caseId)
        {
            return _caseFileRepository.FindFileNamesByCaseId(caseId);
        }

        public bool FileExists(int caseId, string fileName)
        {
            return _caseFileRepository.FileExists(caseId, fileName);
        }

        #endregion

        #region Private Methods

        private string GetFileAttachFolderPath(int customerId)
        {
            return _settingsLogic.GetFilePath(customerId);
        }

        #endregion
    }
}