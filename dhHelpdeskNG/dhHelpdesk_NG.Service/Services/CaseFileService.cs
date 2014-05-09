namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICaseFileService
    {
        //IList<CaseFile> GetCaseFiles(int caseid);
        byte[] GetFileContentByIdAndFileName(int caseId, string fileName);
        List<string> FindFileNamesByCaseId(int caseId);
        void AddFile(CaseFileDto caseFileDto);
        void AddFiles(List<CaseFileDto> caseFileDtos);
        bool FileExists(int caseId, string fileName);
        void DeleteByCaseIdAndFileName(int caseId, string fileName);
    }

    public class CaseFileService : ICaseFileService
    {
        #region Variables

        private readonly ICaseFileRepository _caseFileRepository;
        private readonly IFilesStorage _filesStorage;

        #endregion

        #region Public Methods and Functions

        public CaseFileService(
            ICaseFileRepository caseFileRepository, IFilesStorage fileStorage)
        {
            this._caseFileRepository = caseFileRepository;
            this._filesStorage = fileStorage; 
        }

        public byte[] GetFileContentByIdAndFileName(int caseId, string fileName)
        {
            return this._caseFileRepository.GetFileContentByIdAndFileName(caseId, fileName);  
        }

        public void AddFiles(List<CaseFileDto> caseFileDtos)
        {
            foreach (var f in caseFileDtos)
            {
                this.AddFile(f);
            }
        }

        public void AddFile(CaseFileDto caseFileDto)
        {
            var caseFile = new CaseFile
            {
                CreatedDate = caseFileDto.CreatedDate,
                Case_Id = caseFileDto.ReferenceId,
                FileName = caseFileDto.FileName,
            };
            this._caseFileRepository.Add(caseFile);
            this._caseFileRepository.Commit();

            int caseNo = this._caseFileRepository.GetCaseNumberForUploadedFile(caseFileDto.ReferenceId);
            this._filesStorage.SaveFile(caseFileDto.Content, caseFileDto.FileName, ModuleName.Cases, caseNo);
        }

        public void DeleteByCaseIdAndFileName(int caseId, string fileName)
        {
            this._caseFileRepository.DeleteByCaseIdAndFileName(caseId, fileName);  
        }

        public List<string> FindFileNamesByCaseId(int caseId)
        {
            return this._caseFileRepository.FindFileNamesByCaseId(caseId);  
        }

        public bool FileExists(int caseId, string fileName)
        {
            return this._caseFileRepository.FileExists(caseId, fileName);  
        }

        #endregion

    }
}
