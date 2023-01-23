using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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

    // TODO: Move operations with file and directories to LogService. Repository should work only with db
    public interface ILogFileRepository : IRepository<LogFile>
    {
        LogFile GetDetails(int id);
        FileContentModel GetFileContentByIdAndFileName(int caseId, string basePath, string fileName, LogFileType logType);
        List<string> FindFileNamesByLogId(int logId);
        List<string> FindFileNamesByLogId(int logId, LogFileType logType);
        LogFile FindLogFileNameByLogId(int logId, string filename);
        List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId, LogFileType logFileType);
        List<LogFile> GetLogFilesByCaseId(int caseId, bool includeInternal);
        List<LogFile> GetLogFilesByLogId(int logId, bool includeInternal = true);
        List<LogFile> GetReferencedFiles(int logId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName, LogFileType logType);
        void MoveLogFiles(int caseId, string fromBasePath, string toBasePath);
        List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId);
        List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId, bool isInternalLogNote);
        bool SaveAttachedExistingLogFiles(IEnumerable<LogFileExisting> logExistingFiles);
        void DeleteByFileIdAndFileName(int fileId, string filename);
        void DeleteExistingById(int filename);
        void ClearExistingAttachedFiles(int caseId);
        void AddExistLogFiles(IEnumerable<LogFile> logExFiles);
        FileContentModel GetCaseFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        List<LogFile> GetLogFilesByCaseList(List<int> caseIds, bool includeInternal);
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

        public FileContentModel GetFileContentByIdAndFileName(int logId, string basePath, string fileName, LogFileType logType)
        {
            return _filesStorage.GetFileContent(logType.GetFolderPrefix(), logId, basePath, fileName);
        }

        public FileContentModel GetCaseFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            var caseNumber = DataContext.Cases.Single(x => x.Id == caseId).CaseNumber;
            return _filesStorage.GetFileContent(ModuleName.Cases, Convert.ToInt32(caseNumber), basePath, fileName);
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return DataContext.LogFiles.Where(f => f.Log_Id == logId).Select(f => f.FileName).ToList();
        }

        public List<string> FindFileNamesByLogId(int logId, LogFileType logType)
        {
            return DataContext.LogFiles.Where(f => f.Log_Id == logId && f.LogType == logType).Select(f => f.FileName).ToList();
        }

        public LogFile FindLogFileNameByLogId(int logId, string filename)
        {
            return DataContext.LogFiles.AsNoTracking()
                .FirstOrDefault(f => f.Log_Id == logId && f.FileName.ToLower().Equals(filename.ToLower()));
        }

        public List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId, LogFileType logFileType)
        {
            var logFiles = 
                DataContext.Logs.Where(l => l.Case_Id == caseId)
                    .SelectMany(l => l.LogFiles)
                    .Where(f => f.LogType == logFileType)
                    .Select(f => new KeyValuePair<int, string>(f.Log_Id, f.FileName))
                    .ToList();

            return logFiles;
        }

        public List<LogFile> GetLogFilesByCaseId(int caseId, bool includeInternal)
        {
            var caseFiles = DataContext.Logs.Where(l => l.Case_Id == caseId).SelectMany(l => l.LogFiles);
            return caseFiles.Where(f => includeInternal || (f.LogType == LogFileType.External && !string.IsNullOrEmpty(f.Log.Text_External))).ToList();
        }

        public List<LogFile> GetLogFilesByLogId(int logId, bool includeInternal = true)
        {
            var logFiles = DataContext.LogFiles.Where(f => f.Log_Id == logId && (includeInternal || f.LogType == LogFileType.External)).ToList();
            return logFiles;
        }

        public List<LogFile> GetReferencedFiles(int logId)
        {
            return DataContext.LogFiles.Where(l => l.ParentLog_Id == logId).ToList();
        }

        public void DeleteByLogIdAndFileName(int logId, string basePath, string fileName, LogFileType logType)
        {
            var lf = DataContext.LogFiles.Single(f => f.Log_Id == logId && f.FileName == fileName.Trim() && f.LogType == logType);
            DataContext.LogFiles.Remove(lf);
            Commit();
        }

        public void MoveLogFiles(int caseId, string fromBasePath, string toBasePath)
        {
            var logFiles = DataContext.LogFiles.Where(f => f.Log.Case_Id == caseId).Select(f => new
            {
                Log_Id = f.ParentLog_Id ?? f.Log_Id,
                LogType = f.ParentLogType ?? f.LogType
            }).Distinct().ToList();
            foreach (var logFile in logFiles)
            {
                var logSubFolder = logFile.LogType.GetFolderPrefix();
                _filesStorage.MoveDirectory(logSubFolder, logFile.Log_Id.ToString(), fromBasePath, toBasePath);
            }
        }

        public List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId)
        {
            return DataContext.LogFilesExisting.Where(f => f.Case_Id == caseId).ToList();
        }

        public List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId, bool isInternalLogNote)
        {
            return DataContext.LogFilesExisting.Where(f => f.Case_Id == caseId && f.IsInternalLogNote == isInternalLogNote).ToList();
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

        public List<LogFile> GetLogFilesByCaseList(List<int> caseIds, bool includeInternal)
        {
        

            string _connectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

            List<LogFile> logFiles = new List<LogFile>();

            if (caseIds.Count() > 0)
            {
                DataTable casesTable = new DataTable();
                casesTable.Columns.Add(new DataColumn("Id", typeof(int)));

                // populate DataTable from your List here
                foreach (var id in caseIds)
                {
                    casesTable.Rows.Add(id);
                }

                SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString)
                {
                    ConnectTimeout = 4000,
                    AsynchronousProcessing = true
                };


                using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure })
                    {
                        SqlParameter param = command.Parameters.AddWithValue("@CaseIds", casesTable);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "dbo.IdsList";
                        //param.Direction = ParameterDirection.Output;
                        command.CommandText = "sp_GetLogFilesByCaseIds";
                        var result = command.BeginExecuteReader(null, command);

                        using (SqlDataReader reader = command.EndExecuteReader(result))
                        {
                            while (reader.Read())
                            {
                                int logId, logType, parentLogIdInt, parentLogTypeInt;
                                bool isCaseFileBool;
                                
                                Int32.TryParse(reader["Log_Id"].ToString(), out logId);
                                Int32.TryParse(reader["LogType"].ToString(), out logType);
                                
                                int? parentLogId =  Int32.TryParse(reader["ParentLog_Id"].ToString(), out parentLogIdInt) ? (int?)parentLogIdInt : null;                                    
                                bool? isCaseFile = bool.TryParse(reader["IsCaseFile"].ToString(), out isCaseFileBool) ? (bool?)isCaseFileBool : null;
                                LogFileType? parentLogTypeEnum;

                                if (Int32.TryParse(reader["ParentLogType"].ToString(), out parentLogTypeInt))
                                {
                                    parentLogTypeEnum = (LogFileType)parentLogTypeInt;
                                }
                                else
                                { 
                                    parentLogTypeEnum = null;
                                }

                                logFiles.Add(new LogFile()
                                {
                                    Log_Id = logId,
                                    FileName = Convert.ToString(reader["FileName"].ToString()),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString()),
                                    ParentLog_Id = parentLogId,
                                    IsCaseFile = isCaseFile,
                                    LogType = (LogFileType)(logType),
                                    ParentLogType = parentLogTypeEnum
                                });
                            }
                        }
                    }
                }
            }
            return logFiles;
}


        //var logFiles = (from d in DataContext.Logs.AsEnumerable()
        //                join c in caseList
        //                on d.Case_Id equals c.Id
        //                select d).ToList()
        //                .SelectMany(l => l.LogFiles);


        //var caseFiles = logFiles.Where(f => includeInternal || (f.LogType == LogFileType.External && !string.IsNullOrEmpty(f.Log.Text_External)))
        //                .ToList();
        //    return caseFiles;
        
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
