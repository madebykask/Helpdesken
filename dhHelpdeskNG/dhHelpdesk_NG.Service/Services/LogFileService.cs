namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ILogFileService
    {
        byte[] GetFileContentByIdAndFileName(int logId, string basePath, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
        void AddFile(CaseFileDto fileDto);
        void AddFiles(List<CaseFileDto> fileDtos);
    }

    public class LogFileService : ILogFileService
    {
        private readonly ILogFileRepository _logFileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilesStorage _filesStorage;

        public LogFileService(
            ILogFileRepository logFileRepository
            , IUnitOfWork unitOfWork
            , IFilesStorage filesStorage)
        {
            this._logFileRepository = logFileRepository;
            this._unitOfWork = unitOfWork;
            this._filesStorage = filesStorage; 
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string basePath, string fileName)
        {
            return this._logFileRepository.GetFileContentByIdAndFileName(logId, basePath, fileName);  
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return this._logFileRepository.FindFileNamesByLogId(logId);  
        }

        public void DeleteByLogIdAndFileName(int logId, string basePath, string fileName)
        {
            this._logFileRepository.DeleteByLogIdAndFileName(logId, basePath, fileName);
        }

        public void AddFiles(List<CaseFileDto> fileDtos)
        {
            foreach (var f in fileDtos)
            {
                this.AddFile(f);
            }
        }

        public void AddFile(CaseFileDto fileDto)
        {
            var file = new LogFile 
            {
                CreatedDate = fileDto.CreatedDate,
                Log_Id = fileDto.ReferenceId,
                FileName = fileDto.FileName,
            };

            this._logFileRepository.Add(file);
            this._logFileRepository.Commit();
            this._filesStorage.SaveFile(fileDto.Content, fileDto.BasePath, fileDto.FileName, ModuleName.Log, fileDto.ReferenceId);
        }

    }
}
