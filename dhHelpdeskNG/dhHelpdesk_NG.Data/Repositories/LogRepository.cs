using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region LOG

    public interface ILogRepository : IRepository<Log>
    {
        Log GetById(int id);
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public  Log GetById(int id)
        {
            return (from w in this.DataContext.Set<Log>()
                    where w.Id == id
                    select w).FirstOrDefault();
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
    }

    public class LogFileRepository : RepositoryBase<LogFile>, ILogFileRepository
    {
        public LogFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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
