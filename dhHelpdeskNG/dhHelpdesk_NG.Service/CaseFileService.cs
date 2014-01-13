using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Enums;
using dhHelpdesk_NG.DTO.DTOs.Case;   
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseFileService
    {
        IList<CaseFile> GetCaseFiles(int caseid);
        byte[] GetFileContentByIdAndFileName(int caseId, string fileName);
        List<string> FindFileNamesByCaseId(int caseId);
        void AddFile(CaseFileDto caseFileDto);
        bool FileExists(int caseId, string fileName);
    }

    public class CaseFileService : ICaseFileService
    {
        #region Variables

        private readonly ICaseFileRepository _caseFileRepository;
        private readonly IFilesStorage _filesStorage;

        #endregion

        #region Public Methods and Functions

        public CaseFileService(
            ICaseFileRepository caseFileRepository)
        {
            _caseFileRepository = caseFileRepository;            
        }

        public IList<CaseFile> GetCaseFiles(int caseId)
        {
            return _caseFileRepository.GetCaseFiles(caseId).ToList();
        }

        public byte[] GetFileContentByIdAndFileName(int caseId, string fileName)
        {
            return _caseFileRepository.GetFileContentByIdAndFileName(caseId, fileName);  
        }

        public void AddFile(CaseFileDto caseFileDto)
        {
            var caseFile = new CaseFile
            {
                CreatedDate = caseFileDto.CreatedDate,
                Case_Id = caseFileDto.CaseId,
                FileName = caseFileDto.FileName,
            };

            _caseFileRepository.Add(caseFile);
            _caseFileRepository.Commit();
            _filesStorage.SaveFile(caseFileDto.Content, caseFileDto.FileName, Topic.Case, caseFileDto.CaseId);
        }

        public List<string> FindFileNamesByCaseId(int caseId)
        {
            //Todo anrop till repository
            return null;
        }

        public bool FileExists(int caseId, string fileName)
        {
            return _caseFileRepository.FileExists(caseId, fileName);  
        }

        #endregion

    }
}
