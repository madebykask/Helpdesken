using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Common.Enums;
using ExtendedCase.Dal.Data;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Models.Files;
using System.Web;

namespace ExtendedCase.Logic.Services
{
    public interface ICaseFileService
    {
        bool FileExists(int caseId, string fileName);
        int AddFile(CaseFileDto caseFileDto, int caseNumber);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName, int caseNumber);

        CaseFileContent GetCaseFile(int customerId, int caseId, int fileId, string caseNumber,
            bool embedImmges = false);
    }

    public class CaseFileService: ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly IFilesStorageService _filesStorage;
        private readonly ISettingsRepository _settingsRepository;

        public CaseFileService(ICaseFileRepository caseFileRepository, 
            IFilesStorageService filesStorage, 
            ISettingsRepository settingsRepository)
        {
            _caseFileRepository = caseFileRepository;
            _filesStorage = filesStorage;
            _settingsRepository = settingsRepository;
        }

        public bool FileExists(int caseId, string fileName)
        {
            return _caseFileRepository.FileExists(caseId, fileName);
        }

        //public CaseFileModel GetCaseFile(int caseId, int fileId)
        //{
        //    var fileInfo = _caseFileRepository.GetCaseFileInfo(caseId, fileId);
        //    return fileInfo;
        //}

        public void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName, int caseNumber)
        {
            _caseFileRepository.DeleteByCaseIdAndFileName(caseId, basePath, fileName);
            _filesStorage.DeleteFile(ModuleName.Cases, caseNumber, basePath, fileName);
        }

        public int AddFile(CaseFileDto caseFileDto, int caseNumber)
        {
            var caseFile = new CaseFile
            {
                CreatedDate = caseFileDto.CreatedDate,
                Case_Id = caseFileDto.ReferenceId,
                FileName = caseFileDto.FileName,
                UserId = caseFileDto.UserId
            };
            var id = _caseFileRepository.SaveCaseFile(caseFile);

            var path =_filesStorage.SaveFile(caseFileDto.Content, caseFileDto.BasePath, caseFileDto.FileName, ModuleName.Cases, caseNumber);

            //if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            //{
            //	_fileViewLogService.Log(caseId, userId, caseFileDto.FileName, path, FileViewLogFileSource.WebApi, FileViewLogOperation.Add);
            //}

            return id;
        }

        public CaseFileContent GetCaseFile(int customerId, int caseId, int fileId, string caseNumber, bool embedImmges = false)
        {
            var basePath = _settingsRepository.GetFilePath(customerId);
            var caseFileInfo = _caseFileRepository.GetFileInfo(caseId, fileId);

            var iCaseNumber = Convert.ToInt32(caseNumber);
            var content = 
                _filesStorage.GetFileContent(ModuleName.Cases, iCaseNumber, basePath, caseFileInfo.FileName);

            var res = new CaseFileContent()
            {
                Id = fileId,
                CaseNumber = iCaseNumber,
                FileName = Path.GetFileName(caseFileInfo.FileName),
                Content = content.Content
            };

            var caseFilePath = _filesStorage.GetCaseFilePath(ModuleName.Cases, Convert.ToInt32(iCaseNumber), basePath, caseFileInfo.FileName)
                .Replace("/", "\\") ;
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

    }
}
