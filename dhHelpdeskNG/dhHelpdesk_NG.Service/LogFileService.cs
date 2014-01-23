using System;
using System.Linq;
using System.Collections.Generic;

using dhHelpdesk_NG.Data.Enums;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Service
{
    public interface ILogFileService
    {
        byte[] GetFileContentByIdAndFileName(int logId, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        void DeleteByLogIdAndFileName(int logId, string fileName);
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
            _logFileRepository = logFileRepository;
            _unitOfWork = unitOfWork;
            _filesStorage = filesStorage; 
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string fileName)
        {
            return _logFileRepository.GetFileContentByIdAndFileName(logId, fileName);  
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return _logFileRepository.FindFileNamesByLogId(logId);  
        }

        public void DeleteByLogIdAndFileName(int logId, string fileName)
        {
            _logFileRepository.DeleteByLogIdAndFileName(logId, fileName);
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

            _logFileRepository.Add(file);
            _logFileRepository.Commit();
            _filesStorage.SaveFile(fileDto.Content, fileDto.FileName, Topic.Log, fileDto.ReferenceId);
        }

    }
}
