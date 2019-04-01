using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Logs;

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
        LogFile GetFileDetails(int logFileId);
        LogFileContent GetFileContentById(int logFileId, string basePath);
        byte[] GetFileContentByIdAndFileName(int logId, string basePath, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        List<KeyValuePair<int,string>> FindFileNamesByCaseId(int caseId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
        void AddFile(CaseFileDto fileDto);
        void AddFiles(List<CaseFileDto> fileDtos, List<LogExistingFileModel> temporaryExLogFiles = null, int? currentLogId = null);
        void MoveLogFiles(int caseId, string fromBasePath, string toBasePath);
        List<LogExistingFileModel> GetExistingFileNamesByCaseId(int caseId);
        bool SaveAttachedExistingLogFiles(IEnumerable<LogExistingFileModel> allFiles, int caseId);
        void DeleteByFileIdAndFileName(int fileId, string trim);
        void DeleteByFileName(string trim);
        void ClearExistingAttachedFiles(int caseId);
        List<LogFileModel> GetLogFileNamesByLogId(int logId);
        List<LogFileModel> GetLogFilesByCaseId(int caseId);
        byte[] GetCaseFileContentByIdAndFileName(int caseId, string basePath, string name);
    }

    public class LogFileService : ILogFileService
    {
        private readonly ILogFileRepository _logFileRepository;
        private readonly IFilesStorage _filesStorage;

        public LogFileService(ILogFileRepository logFileRepository, IFilesStorage filesStorage)
        {
            _logFileRepository = logFileRepository;
            _filesStorage = filesStorage; 
        }

        public LogFile GetFileDetails(int logFileId)
        {
            var logFile = _logFileRepository.GetDetails(logFileId);
            return logFile;
        }

        public LogFileContent GetFileContentById(int logFileId, string basePath)
        {
            return _logFileRepository.GetFileContent(logFileId, basePath);
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string basePath, string fileName)
        {
            return this._logFileRepository.GetFileContentByIdAndFileName(logId, basePath, fileName);
        }

        public byte[] GetCaseFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            return this._logFileRepository.GetCaseFileContentByIdAndFileName(caseId, basePath, fileName);
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return this._logFileRepository.FindFileNamesByLogId(logId);  
        }

        public List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId)
        {
            return this._logFileRepository.FindFileNamesByCaseId(caseId);
        }

        public void DeleteByLogIdAndFileName(int logId, string basePath, string fileName)
        {
            this._logFileRepository.DeleteByLogIdAndFileName(logId, basePath, fileName);
        }

        public void AddFiles(List<CaseFileDto> fileDtos, List<LogExistingFileModel> exFiles = null, int? currentLogId = null)
        {
            foreach (var f in fileDtos)
            {
                this.AddFile(f);
            }
            if (exFiles != null && exFiles.Any() && currentLogId.HasValue)
            {
                var logExFiles = exFiles.Select(x => new LogFile
                {
                    CreatedDate = DateTime.Now,
                    FileName = x.Name,
                    Log_Id = currentLogId.Value,
                    IsCaseFile = x.IsExistCaseFile,
                    ParentLog_Id = x.LogId
                }).ToList();
                _logFileRepository.AddExistLogFiles(logExFiles);
            }
        }

        public void MoveLogFiles(int caseId, string fromBasePath, string toBasePath)
        {
            _logFileRepository.MoveLogFiles(caseId, fromBasePath, toBasePath);
        }

        public List<LogExistingFileModel> GetExistingFileNamesByCaseId(int caseId)
        {
            var files = _logFileRepository.GetExistingFileNamesByCaseId(caseId);
            var caseFiles = files.Where(x => !x.Log_Id.HasValue).Select(x => new LogExistingFileModel
            {
                Name = x.FileName,
                CaseId = x.Case_Id,
                IsExistCaseFile = true
            }).ToList();
            var logFiles = files.Where(x => x.Log_Id.HasValue).Select(x => new LogExistingFileModel
            {
                Name = x.FileName,
                CaseId = x.Case_Id,
                IsExistLogFile = true,
                LogId = x.Log_Id
            }).ToList();
            var allFiles = caseFiles;
            allFiles.AddRange(logFiles);
            return allFiles;
        }

        public bool SaveAttachedExistingLogFiles(IEnumerable<LogExistingFileModel> allFiles, int caseId)
        {
            var logExistingFiles = allFiles.Select(x => new LogFileExisting
            {
                Case_Id = x.CaseId,
                CreatedDate = DateTime.Now,
                FileName = x.Name,
                Log_Id = x.LogId
            }).ToList();
            var exFiles = _logFileRepository.GetExistingFileNamesByCaseId(caseId).Select(x => x.FileName).ToList();
            var filesToAdd = logExistingFiles.Where(x => !exFiles.Contains(x.FileName)).ToList();
            
            return _logFileRepository.SaveAttachedExistingLogFiles(filesToAdd);
        }

        public void DeleteByFileIdAndFileName(int fileId, string filename)
        {
            _logFileRepository.DeleteByFileIdAndFileName(fileId, filename);
        }

        public void DeleteByFileName(string filename)
        {
            _logFileRepository.DeleteByFileName(filename);
        }

        public void ClearExistingAttachedFiles(int caseId)
        {
            _logFileRepository.ClearExistingAttachedFiles(caseId);
        }

        public List<LogFileModel> GetLogFileNamesByLogId(int logId)
        {
            var files = _logFileRepository.GetLogFilesByLogId(logId);
            var exFiles = files.Select(x => new LogFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                IsExistCaseFile = x.IsCaseFile.GetValueOrDefault(false),
                IsExistLogFile = x.ParentLog_Id.HasValue,
                ObjId = x.IsCaseFile.GetValueOrDefault(false) ? x.Log.Case_Id : x.ParentLog_Id
            }).ToList();
            return exFiles;
        }

        public List<LogFileModel> GetLogFilesByCaseId(int caseId)
        {
            var files = _logFileRepository.GetLogFilesByCaseId(caseId);
            return files.Where(x => !x.IsCaseFile.HasValue && !x.ParentLog_Id.HasValue).Select(x => new LogFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                ObjId = x.Log_Id
            }).ToList();
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
            if (fileDto.IsCaseFile)
            {
                
            }
            this._filesStorage.SaveFile(fileDto.Content, fileDto.BasePath, fileDto.FileName, ModuleName.Log, fileDto.ReferenceId);
        }

    }
}
