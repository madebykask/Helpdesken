using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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

        Log GetLastLog(int caseId);
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {

        public LogRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
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
        public IEnumerable<BusinessData.Models.Logs.Output.LogOverview> GetCaseLogOverviews(int caseId)
        {
                var entities = this.Table                    
                    .Where(l => l.Case_Id == caseId)  
                    .OrderByDescending(l => l.LogDate)
                    .ToList();

                return entities.Select(l => new BusinessData.Models.Logs.Output.LogOverview()
                        {
                            CaseHistoryId = l.CaseHistory_Id,
                            CaseId = l.Case_Id,
                            ChangeTime = l.ChangeTime,
                            Charge = l.Charge,
                            EquipmentPrice = l.EquipmentPrice,
                            Export = l.Export,
                            ExportDate = l.ExportDate,
                            FinishingDate = l.FinishingDate,
                            FinishingType = l.FinishingType,
                            Id = l.Id,
                            InformCustomer = l.InformCustomer,
                            LogDate = l.LogDate,
                            LogGuid = l.LogGUID,
                            LogType = l.LogType,
                            Price = l.Price,
                            RegTime = l.RegTime,
                            RegUser = l.RegUser,
                            TextExternal = l.Text_External,
                            TextInternal = l.Text_Internal,
                            UserId = l.User_Id,
                            WorkingTime = l.WorkingTime,
                            CaseHistory = l.CaseHistory,
                            LogFiles = l.LogFiles,
                            User = l.User
                        });
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

    public interface ILogProgramRepository : IRepository<LogProgram>
    {
    }

    public class LogProgramRepository : RepositoryBase<LogProgram>, ILogProgramRepository
    {
        public LogProgramRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region LOGFILE

    public interface ILogFileRepository : IRepository<LogFile>
    {
        byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        List<LogFile> GetLogFilesByCaseId(int caseId);
        List<LogFile> GetLogFilesByLogId(int logId);
        void DeleteByLogIdAndFileName(int logId, string basePath, string fileName);
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

        public List<string> FindFileNamesByLogId(int logId)
        {
            return this.DataContext.LogFiles.Where(f => f.Log_Id == logId).Select(f => f.FileName).ToList();
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
