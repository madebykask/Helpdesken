using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Common.Enums;
using ExtendedCase.Dal.Data;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Models.Files;

namespace ExtendedCase.Logic.Services
{
    public interface ICaseFileService
    {
        bool FileExists(int caseId, string fileName);
        int AddFile(CaseFileDto caseFileDto, int caseNumber);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName, int caseNumber);
    }

    public class CaseFileService: ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly IFilesStorageService _filesStorage;

        public CaseFileService(ICaseFileRepository caseFileRepository, 
            IFilesStorageService filesStorage)
        {
            _caseFileRepository = caseFileRepository;
            _filesStorage = filesStorage;
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
    }
}
