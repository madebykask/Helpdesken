using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.MapperData.Logs;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.LogProgram;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;
    using Log = DH.Helpdesk.Domain.Log;

    #region LOG

    public interface ILogRepository : IRepository<Log>
    {
        Log GetLogById(int id);
        IEnumerable<Log> GetLogForCase(int caseId);

        /// <summary>
        /// The get case log overviews.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The result />.
        /// </returns>
        IEnumerable<BusinessData.Models.Logs.Output.LogOverview> GetCaseLogOverviews(int caseId);
        IEnumerable<Log> GetCaseLogs(DateTime? fromDate, DateTime? toDate);

        Log GetLastLog(int caseId);
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        private readonly IEntityToBusinessModelMapper<LogMapperData, LogOverview> _logToLogOverviewMapper;

        public LogRepository(IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<LogMapperData, LogOverview> logToLogOverviewMapper) 
            : base(databaseFactory)
        {
            _logToLogOverviewMapper = logToLogOverviewMapper;
        }

        public Log GetLogById(int id)
        {
            return (from l in this.DataContext.Logs
                    where l.Id == id
                    select l).FirstOrDefault();
        }

        public IEnumerable<Log> GetLogForCase(int caseId)
        {
            var q = (
                    from l in this.DataContext.Logs
                    where l.Case_Id == caseId
                    select l
                     );
            return q.OrderByDescending(l => l.LogDate);

            //// todo join with log from problem module
            //var q2 =
            //        (
            //        from p in this.DataContext.Problems
            //        join c in DataContext.Cases on p.Id equals c.Problem_Id
            //        join pl in DataContext.ProblemLogs on p.Id equals pl.Problem_Id
            //        join u in DataContext.Users on p.ChangedByUser_Id equals u.Id
            //        where c.Id == caseId && pl.ShowOnCase != 0
            //        select new Log { Id = pl.Id, LogGUID = pl.ProblemLogGUID, Case_Id = caseId, Text_External = pl.LogText, LogDate = pl.CreatedDate, RegUser = u.FirstName + " " + u.SurName, InformCustomer = 0, WorkingTime = 0, EquipmentPrice = 0, Export = 0, Charge = 0, LogType = 0 }
            //        );
            //var combined = q.Concat(q2);
            //return combined.OrderByDescending(l => l.LogDate);
        }


        /// <summary>
        /// The get case log overviews.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The result />.
        /// </returns>
        public IEnumerable<LogOverview> GetCaseLogOverviews(int caseId)
        {
            var query = from l in Table
                        where l.Case_Id == caseId
                        orderby l.LogDate descending
                        select new LogMapperData
                        {
                            Log = l,

                            User = new UserMapperData
                            {
                                Id = l.User.Id,
                                FirstName = l.User.FirstName,
                                SurName = l.User.SurName
                            },
                          
                            EmailLogs = l.CaseHistory.Emaillogs.DefaultIfEmpty()
                                            .Select(t => new EmailLogMapperData
                                            {
                                                Id = t.Id,
                                                MailId = t.MailId,
                                                EmailAddress = t.EmailAddress
                                            }),

                            LogFiles = l.LogFiles.DefaultIfEmpty()
                                        .Select(t => 
                                            new LogFileMapperData
                                            {
                                                Id = t.Id,
                                                FileName = t.FileName,
                                                LogId = t.ParentLog_Id,
                                                CaseId = t.IsCaseFile.HasValue && t.IsCaseFile.Value ? t.Log.Case_Id : (int?)null
                                            })
                        };

            var items = query.ToList();

            var result = items.Select(_logToLogOverviewMapper.Map).ToList();
            return result;
        }

        public IEnumerable<Log> GetCaseLogs(DateTime? fromDate, DateTime? toDate)
        {
            var ret = new List<Log>();
            if (fromDate.HasValue && toDate.HasValue)
            {
                var fDate = fromDate.Value.AddDays(-1);
                var tDate = toDate.Value.AddMonths(1);                
                ret =  this.Table.Where(l => l.LogDate >= fDate && l.LogDate <= tDate).ToList();
            }
            else
            {
                ret =  this.Table.ToList();
            }            

            return ret;
        }

        public Log GetLastLog(int caseId)
        {
            return this.Table
                    .Where(l => l.Case_Id == caseId)
                    .OrderByDescending(l => l.LogDate)
                    .FirstOrDefault();
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
        byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        List<KeyValuePair<int, string>> FindFileNamesByCaseId(int caseId);
        List<LogFile> GetLogFilesByCaseId(int caseId);
        List<LogFile> GetLogFilesByLogId(int logId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
        void MoveLogFiles(int caseId, string fromBasePath, string toBasePath);
        List<LogFileExisting> GetExistingFileNamesByCaseId(int caseId);
        bool SaveAttachedExistingLogFiles(IEnumerable<LogFileExisting> logExistingFiles);
        void DeleteByFileIdAndFileName(int fileId, string filename);
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
            this._filesStorage = fileStorage;
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

        public List<string> FindFileNamesByLogId(int logId)
        {
//            return this.DataContext.LogFiles.Where(f => f.Log_Id == logId && !f.ParentLog_Id.HasValue && !(f.IsCaseFile.HasValue && f.IsCaseFile.Value)).Select(f => f.FileName).ToList();
            return this.DataContext.LogFiles.Where(f => f.Log_Id == logId).Select(f => f.FileName).ToList();
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
//            var files = logExistingFiles.ToList();
//            if (files.Any())
//            {
//                var caseId = files.Select(x => x.Case_Id).FirstOrDefault();
//                var dupFiles = DataContext.LogFilesExisting.Where(x => x.FileName.Equals(x.FileName) && x.Case_Id == caseId);
//                DataContext.LogFilesExisting.RemoveRange(oldFiles);
//            }
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

        public void ClearExistingAttachedFiles(int caseId)
        {
            var files = DataContext.LogFilesExisting.Where(x => x.Case_Id == caseId);
            DataContext.LogFilesExisting.RemoveRange(files);
            DataContext.SaveChanges();

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
