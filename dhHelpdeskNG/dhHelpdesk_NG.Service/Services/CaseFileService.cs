using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services
{
    public interface ICaseFileService
    {
        CaseFileContent GetCaseFile(int customerId, int caseId, int fileId);
        byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);

        IList<string> FindFileNamesByCaseId(int caseId);
        void AddFile(CaseFileDto caseFileDto);
        void AddFiles(IList<CaseFileDto> caseFileDtos);
        void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath);
        bool FileExists(int caseId, string fileName);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName);

        IList<CaseFileModel> GetCaseFiles(int caseId, bool canDelete);
        IList<CaseFileDate> FindFileNamesAndDatesByCaseId(int caseId);
    }

    public class CaseFileService : ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly ISettingRepository _settingRepository;

        #region ctor()

        public CaseFileService(ISettingRepository settingRepository,
            IGlobalSettingRepository globalSettingRepository,
            ICaseFileRepository caseFileRepository)
        {
            _settingRepository = settingRepository;
            _globalSettingRepository = globalSettingRepository;
            _caseFileRepository = caseFileRepository;
        }

        #endregion

        #region Public Methods

        public CaseFileContent GetCaseFile(int customerId, int caseId, int fileId)
        {
            var basePath = GetFileAttachFolderPath(customerId);
            var res = _caseFileRepository.GetCaseFileContent(caseId, fileId, basePath);
            return res;
        }
        
        public byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            return _caseFileRepository.GetFileContentByIdAndFileName(caseId, basePath, fileName);
        }

        public void AddFiles(IList<CaseFileDto> caseFileDtos)
        {
            caseFileDtos?.ForEach(AddFile);
        }

        public void AddFile(CaseFileDto caseFileDto)
        {
            _caseFileRepository.SaveCaseFile(caseFileDto);
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
            var customerFilePath = _settingRepository.GetMany(s => s.Customer_Id == customerId).Single().PhysicalFilePath;
            if (string.IsNullOrEmpty(customerFilePath))
            {
                var globalSetting = _globalSettingRepository.Get();
                if (globalSetting != null)
                    customerFilePath = globalSetting.AttachedFileFolder;
            }

            return customerFilePath ?? string.Empty;
        }

        #endregion
    }
}