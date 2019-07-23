using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums.Logs;


namespace DH.Helpdesk.Dal.Repositories
{
    #region LOG

    public interface ILogRepository : IRepository<Log>
    {
        IQueryable<Log> GetCaseLogs(int caseId);
        Log GetLastLog(int caseId);
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IQueryable<Log> GetCaseLogs(int caseId)
        {
            var query = Table.Where(l => l.Case_Id == caseId).OrderByDescending(l => l.LogDate);
            return query;
        }

        public Log GetLastLog(int caseId)
        {
            return Table.Where(l => l.Case_Id == caseId).OrderByDescending(l => l.LogDate).FirstOrDefault();
        }
    }

    #endregion

    #region LOGPROGRAM

    public interface ILogProgramRepository : IRepository<LogProgramEntity>
    {
        void UpdateUserLogin(LogProgram logProgram);
    }

    public class LogProgramRepository : RepositoryBase<LogProgramEntity>, ILogProgramRepository
    {
        public LogProgramRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {}

        public void UpdateUserLogin(LogProgram logProgram)
        {
            var logProgramEntity = new LogProgramEntity()
            {
                Case_Id = logProgram.CaseId,
                Customer_Id = logProgram.CustomerId,
                Log_Type = logProgram.LogType,
                LogText = logProgram.LogText,
                New_Performer_user_Id = logProgram.New_Performer_user_Id,
                Old_Performer_User_Id = logProgram.Old_Performer_User_Id,
                RegTime = logProgram.RegTime,
                User_Id = logProgram.UserId,
                ServerNameIP = logProgram.ServerNameIP,
                NumberOfUsers = logProgram.NumberOfUsers
            };

            this.DataContext.LogPrograms.Add(logProgramEntity);
            this.Commit();            
        }        
    }

    #endregion

    #region LOGFILE

    public interface ILogFileRepository : IRepository<LogFile>
    {
        LogFile GetDetails(int id);
        LogFileContent GetFileContent(int logFileId, string basePath);
        byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        List<string> FindFileNamesByLogId(int logId, LogFileType logType = LogFileType.External);
        List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId);
        List<LogFile> GetLogFilesByCaseId(int caseId);
        List<LogFile> GetLogFilesByLogId(int logId);
        List<LogFile> GetReferencedFiles(int logId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
        void MoveLogFiles(int caseId, string fromBasePath, string toBasePath);
        List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId);
        bool SaveAttachedExistingLogFiles(IEnumerable<LogFileExisting> logExistingFiles);
        void DeleteByFileIdAndFileName(int fileId, string filename);
        void DeleteExistingById(int filename);
        void ClearExistingAttachedFiles(int caseId);
        void AddExistLogFiles(IEnumerable<LogFile> logExFiles);
        byte[] GetCaseFileContentByIdAndFileName(int caseId, string basePath, string fileName);
    }

    public class LogFileRepository : RepositoryBase<LogFile>, ILogFileRepository
    {
        private readonly IFilesStorage _filesStorage;

        public LogFileRepository(IDatabaseFactory databaseFactory, IFilesStorage fileStorage)
            : base(databaseFactory)
        {
            _filesStorage = fileStorage;
        }

        public LogFile GetDetails(int id)
        {
            return Table.FirstOrDefault(f => f.Id == id);
        }

    public LogFileContent GetFileContent(int logFileId, string basePath)
        {
            var logFile = Table.FirstOrDefault(f => f.Id == logFileId);
            if (logFile == null)
                return null;

            var content = _filesStorage.GetFileContent(ModuleName.Log, logFile.Log_Id, basePath, logFile.FileName);
            if (content == null)
                return null;

            return new LogFileContent()
            {
                Id = logFile.Id,
                LogId = logFile.Log_Id,
                FileName = logFile.FileName,
                Content = content
            };
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string basePath, string fileName)
        {
            return this._filesStorage.GetFileContent(ModuleName.Log, logId, basePath, fileName);
        }

        public byte[] GetCaseFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            var caseNumber = DataContext.Cases.Single(x => x.Id == caseId).CaseNumber;
            return this._filesStorage.GetFileContent(ModuleName.Cases, Convert.ToInt32(caseNumber), basePath, fileName);
        }

        public List<string> FindFileNamesByLogId(int logId, LogFileType logType = LogFileType.External)
        {
            return DataContext.LogFiles.Where(f => f.Log_Id == logId).Select(f => f.FileName).ToList();
        }

        public List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId)
        {
            var ret = new List<KeyValuePair<int, string>>();
            var logs = this.DataContext.LogFiles.Where(f => f.Log.Case_Id == caseId).ToList();

            foreach (var log in logs)
            {
                ret.Add(new KeyValuePair<int, string>(log.Log_Id, log.FileName));
            }

            return ret;            
        }

        public List<LogFile> GetLogFilesByCaseId(int caseId)
        {
            return (from f in this.DataContext.LogFiles
                    join l in this.DataContext.Logs on f.Log_Id equals l.Id
                    where l.Case_Id == caseId
                    select f).ToList();             
        }

        public List<LogFile> GetLogFilesByLogId(int logId)
        {
            return (from f in this.DataContext.LogFiles
                    where f.Log_Id == logId
                    select f).ToList();
        }

        public List<LogFile> GetReferencedFiles(int logId)
        {
            return DataContext.LogFiles.Where(l => l.ParentLog_Id == logId).ToList();
        }

        public void DeleteByLogIdAndFileName(int logId, string basePath, string fileName)
        {
            var lf = this.DataContext.LogFiles.Single(f => f.Log_Id == logId && f.FileName == fileName.Trim());
            this.DataContext.LogFiles.Remove(lf);
            this.Commit();
            this._filesStorage.DeleteFile(ModuleName.Log, logId, basePath, fileName);
        }

        public void MoveLogFiles(int caseId, string fromBasePath, string toBasePath)
        {
            var logFiles = this.DataContext.LogFiles.Where(f => f.Log.Case_Id == caseId).Select(f=> f.Log_Id).Distinct().ToList();
            foreach (var logId in logFiles)
                _filesStorage.MoveDirectory(ModuleName.Log, logId.ToString(), fromBasePath, toBasePath);
        }

        public List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId)
        {
            return DataContext.LogFilesExisting.Where(f => f.Case_Id == caseId).ToList();
        }

        public bool SaveAttachedExistingLogFiles(IEnumerable<LogFileExisting> logExistingFiles)
        {
            DataContext.LogFilesExisting.AddRange(logExistingFiles);
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteByFileIdAndFileName(int fileId, string filename)
        {
            var file = DataContext.LogFiles.FirstOrDefault(x => x.Id == fileId && x.FileName.Equals(filename));
            if (file != null)
            {
                DataContext.LogFiles.Remove(file);
                DataContext.SaveChanges();
            }

        }

        public void DeleteExistingById(int logId)
        {
            var file = DataContext.LogFilesExisting.FirstOrDefault(x => x.Id == logId);
            if (file != null)
            {
                DataContext.LogFilesExisting.Remove(file);
                DataContext.SaveChanges();
            }
        }        
        public void ClearExistingAttachedFiles(int caseId)
        {
            var files = DataContext.LogFilesExisting.Where(x => x.Case_Id == caseId).ToList();
            if (files.Any())
            {
                DataContext.LogFilesExisting.RemoveRange(files);
                DataContext.SaveChanges();
            }
        }

        public void AddExistLogFiles(IEnumerable<LogFile> logExFiles)
        {
            DataContext.LogFiles.AddRange(logExFiles);
        }
    }

    #endregion

    #region LOGSYNC

    public interface ILogSyncRepository : IRepository<LogSync>
    {
    }

    public class LogSyncRepository : RepositoryBase<LogSync>, ILogSyncRepository
    {
        public LogSyncRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
