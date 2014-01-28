using System.Collections.Generic;
using System.Linq;

using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Enums;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region LOG

    public interface ILogRepository : IRepository<Log>
    {
        Log GetLogById(int id);
        IEnumerable<Log> GetLogForCase(int caseId);
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
            //todo tblProblem ska också hämtas, union i gammal hd
            var q = (
                    from l in this.DataContext.Logs
                    where l.Case_Id == caseId
                    select l
                     );
                //.Concat
                //    (
                //    from p in this.DataContext.Problems
                //    join c in DataContext.Cases on p.Id equals c.Problem_Id
                //    join pl in DataContext.ProblemLogs on p.Id equals pl.Problem_Id
                //    join u in DataContext.Users on p.ChangedByUser_Id equals u.Id
                //    where c.Id == caseId && pl.ShowOnCase == 1
                //    select new Log { Case_Id = caseId, Text_External = pl.LogText, LogDate = pl.CreatedDate, RegUser = u.FirstName + " " + u.SurName }
                //    );

            return q.OrderByDescending(l => l.LogDate);
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
        byte[] GetFileContentByIdAndFileName(int caseId, string fileName);
        List<string> FindFileNamesByLogId(int logId);
        void DeleteByLogIdAndFileName(int logId, string fileName);
    }

    public class LogFileRepository : RepositoryBase<LogFile>, ILogFileRepository
    {
        private readonly IFilesStorage _filesStorage;

        public LogFileRepository(IDatabaseFactory databaseFactory, IFilesStorage fileStorage)
            : base(databaseFactory)
        {
            _filesStorage = fileStorage;
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string fileName)
        {
            return _filesStorage.GetFileContent(TopicName.Log, logId, fileName);
        }

        public List<string> FindFileNamesByLogId(int logId)
        {
            return this.DataContext.LogFiles.Where(f => f.Log_Id == logId).Select(f => f.FileName).ToList();
        }

        public void DeleteByLogIdAndFileName(int logId, string fileName)
        {
            var lf = this.DataContext.LogFiles.Single(f => f.Log_Id == logId && f.FileName == fileName);
            this.DataContext.LogFiles.Remove(lf);
            this.Commit();
            _filesStorage.DeleteFile(fileName, TopicName.Log, logId);
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
