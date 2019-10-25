using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Logs;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Dal.Infrastructure.Extensions;

namespace DH.Helpdesk.Services.Services
{
    public interface ILogFileService
    {
        LogFile GetFileDetails(int logFileId);
        LogFileContent GetFileContentById(int logFileId, string basePath, LogFileType logFileType);
        FileContentModel GetFileContentByIdAndFileName(int logId, string basePath, string fileName, LogFileType logType);
        List<string> FindFileNamesByLogId(int logId);
        List<string> FindFileNamesByLogId(int logId, LogFileType logType);
        List<KeyValuePair<int,string>> FindFileNamesByCaseId(int caseId, LogFileType logType);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
        void AddFile(CaseLogFileDto fileDto, ref string path);
        void AddFiles(List<CaseLogFileDto> fileDtos, List<KeyValuePair<CaseLogFileDto, string>> paths, List<LogExistingFileModel> temporaryExLogFiles = null, int? currentLogId = null);
        void MoveLogFiles(int caseId, string fromBasePath, string toBasePath);
        List<LogExistingFileModel> GetExistingFileNamesByCaseId(int caseId, bool isInternalLog);
        List<LogExistingFileModel> GetExistingFileNamesByCaseId(int caseId);
        bool SaveAttachedExistingLogFiles(IEnumerable<LogExistingFileModel> allFiles, int caseId, bool isInternalLog);
        void DeleteByFileIdAndFileName(int fileId, string trim);
        void DeleteExistingById(int logId);
        void ClearExistingAttachedFiles(int caseId);
        List<LogFileModel> GetLogFileNamesByLogId(int logId, bool includeInternal);
        List<LogFileModel> GetLogFilesByCaseId(int caseId, bool includeInternal);
		List<LogFile> GetLogFilesByNameAndId(string fileName, int logId);
        List<LogFileModel> GetLogFilesById(int logId, LogFileType? type = null);
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

        public LogFileContent GetFileContentById(int logFileId, string basePath, LogFileType logFileType)
        {
            var logFile = _logFileRepository.Get(x => x.Id == logFileId);
            if (logFile == null)
                return null;

            var content = _filesStorage.GetFileContent(logFileType.GetFolderPrefix(),
                logFile.ParentLog_Id ?? logFile.Log_Id, basePath, logFile.FileName);
            if (content == null)
                return null;

            return new LogFileContent()
            {
                Id = logFile.Id,
                LogId = logFile.Log_Id,
                FileName = logFile.FileName,
				Path = content.FilePath,
                Content = content.Content
            };
        }

        public FileContentModel GetFileContentByIdAndFileName(int logId, string basePath, string fileName, LogFileType logType)
        {
            return _logFileRepository.GetFileContentByIdAndFileName(logId, basePath, fileName, logType);
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return _logFileRepository.FindFileNamesByLogId(logId);
        }

        public List<string> FindFileNamesByLogId(int logId, LogFileType logType)
        {
            return _logFileRepository.FindFileNamesByLogId(logId, logType);
        }

        public List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId, LogFileType logType)
        {
            return _logFileRepository.FindFileNamesByCaseId(caseId, logType);
        }

        public void DeleteByLogIdAndFileName(int logId, string basePath, string fileName)
        {
            var logFile = _logFileRepository.FindLogFileNameByLogId(logId, fileName);
            _logFileRepository.DeleteByLogIdAndFileName(logId, basePath, fileName, logFile.LogType);

            if (!logFile.ParentLog_Id.HasValue) //don't delete file if it is reference logfile
            {
                var logSubFolder = logFile.GetFolderPrefix();
                _filesStorage.DeleteFile(logSubFolder, logId, basePath, fileName);
            }
        }

        public void AddFiles(List<CaseLogFileDto> fileDtos, List<KeyValuePair<CaseLogFileDto, string>> paths, List<LogExistingFileModel> exFiles = null, int? currentLogId = null)
        {
            foreach (var f in fileDtos)
            {
				var path = "";
                AddFile(f, ref path);
				paths.Add(new KeyValuePair<CaseLogFileDto, string>(f, path));
            }

            if (exFiles != null && exFiles.Any() && currentLogId.HasValue)
            {
                var logExFiles = exFiles.Select(x => new LogFile
                {
                    CreatedDate = DateTime.UtcNow,
                    FileName = x.Name,
                    Log_Id = currentLogId.Value,
                    IsCaseFile = x.IsExistCaseFile,
                    ParentLog_Id = x.LogId,
                    LogType = x.IsInternalLogNote ? LogFileType.Internal : LogFileType.External,
                    ParentLogType = x.LogType
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
            var files = GetExistingFileNamesByCaseIdInner(caseId);
            return files;
        }

        public List<LogExistingFileModel> GetExistingFileNamesByCaseId(int caseId, bool isInternalLog)
        {
            var files = GetExistingFileNamesByCaseIdInner(caseId);
            return files.Where(f => f.IsInternalLogNote == isInternalLog).ToList();
        }

        private List<LogExistingFileModel> GetExistingFileNamesByCaseIdInner(int caseId)
        {
            var files = _logFileRepository.GetExistingFileNamesByCaseId(caseId);

            var caseFiles = files.Where(f => !f.Log_Id.HasValue).Select(x => new LogExistingFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                CaseId = x.Case_Id,
                IsExistCaseFile = true,
                IsExistLogFile = false,
                IsInternalLogNote = x.IsInternalLogNote
            }).ToList();

            var logFiles = files.Where(x => x.Log_Id.HasValue).Select(f => new LogExistingFileModel
            {
                Id = f.Id,
                Name = f.FileName,
                CaseId = f.Case_Id,
                IsExistLogFile = true,
                IsExistCaseFile = false,
                LogId = f.Log_Id,
                LogType = f.LogType,
                IsInternalLogNote = f.IsInternalLogNote
            }).ToList();

            var allFiles = caseFiles;
            allFiles.AddRange(logFiles);

            return allFiles;
        }
        
        public bool SaveAttachedExistingLogFiles(IEnumerable<LogExistingFileModel> allFiles, int caseId, bool isInternalLog)
        {
            var logExistingFiles = allFiles.Select(x => new LogFileExisting
            {
                Case_Id = x.CaseId,
                CreatedDate = DateTime.Now,
                FileName = x.Name,
                Log_Id = x.LogId,
                LogType = x.LogType,
                IsInternalLogNote = isInternalLog
            }).ToList();

            var exFiles = _logFileRepository.GetExistingFileNamesByCaseId(caseId, isInternalLog).Select(x => x.FileName).ToList();
            var filesToAdd = logExistingFiles.Where(x => !exFiles.Contains(x.FileName, StringComparer.OrdinalIgnoreCase)).ToList();
            
            return _logFileRepository.SaveAttachedExistingLogFiles(filesToAdd);
        }

        public void DeleteByFileIdAndFileName(int fileId, string filename)
        {
            _logFileRepository.DeleteByFileIdAndFileName(fileId, filename);
        }

        public void DeleteExistingById(int logId)
        {
            _logFileRepository.DeleteExistingById(logId);
        }

        public void ClearExistingAttachedFiles(int caseId)
        {
            _logFileRepository.ClearExistingAttachedFiles(caseId);
        }

        public List<LogFileModel> GetLogFileNamesByLogId(int logId, bool includeInternal = false)
        {
            var files = _logFileRepository.GetLogFilesByLogId(logId, includeInternal);
            var exFiles = files.Select(x => new LogFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                IsExistCaseFile = x.IsCaseFile.GetValueOrDefault(false),
                IsExistLogFile = x.ParentLog_Id.HasValue,
                ObjId = x.IsCaseFile.GetValueOrDefault(false) ? x.Log.Case_Id : x.ParentLog_Id,
                LogType = x.LogType,
                ParentLogType = x.ParentLogType
            }).ToList();
            return exFiles;
        }

        public List<LogFileModel> GetLogFilesByCaseId(int caseId, bool includeInternal)
        {
            var files = _logFileRepository.GetLogFilesByCaseId(caseId, includeInternal);
            return files.Where(x => !x.IsCaseFile.HasValue && !x.ParentLog_Id.HasValue).Select(x => new LogFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                ObjId = x.Log_Id,
                LogType = x.LogType,
                ParentLogType = x.ParentLogType
            }).ToList();
        }

        public List<LogFileModel> GetLogFilesById(int logId, LogFileType? type = null)
        {
            var files = _logFileRepository.GetLogFilesByLogId(logId);
            if (type.HasValue)
                files = files.Where(f => f.LogType == type.Value).ToList();
            return files.Where(x => !x.IsCaseFile.HasValue && !x.ParentLog_Id.HasValue).Select(x => new LogFileModel
            {
                Id = x.Id,
                Name = x.FileName,
                ObjId = x.Log_Id,
                LogType = x.LogType,
                ParentLogType = x.ParentLogType
            }).ToList();

        }

        public void AddFile(CaseLogFileDto fileDto, ref string path)
        {
            var file = new LogFile 
            {
                CreatedDate = fileDto.CreatedDate,
                Log_Id = fileDto.ReferenceId,
                FileName = fileDto.FileName,
                LogType = fileDto.LogType,
                ParentLogType = fileDto.ParentLogType
            };

            _logFileRepository.Add(file);
            _logFileRepository.Commit();

            if (fileDto.IsCaseFile) 
            {
            }

            path = _filesStorage.SaveFile(fileDto.Content, fileDto.BasePath, fileDto.FileName, fileDto.LogType.GetFolderPrefix(), fileDto.ReferenceId);
        }

		public List<LogFile> GetLogFilesByNameAndId(string fileName, int logId)
		{
			return _logFileRepository.GetMany(o => o.FileName == fileName && o.Log_Id == logId).ToList();
		}
	}
}
