using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;   

namespace dhHelpdesk_NG.Data.Repositories
{
    #region LOG

    public interface ILogRepository : IRepository<Log>
    {
        Log GetLogById(int id);
        IEnumerable<CaseLog> GetLogByCaseId(int caseId);
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
            return null;
        }

        public IEnumerable<CaseLog> GetLogByCaseId(int caseId)
        {
            var query =
                from l in DataContext.Logs
                //join u in DataContext.Users on l.User_Id equals u.Id into res
                //from usr in res.DefaultIfEmpty()
                where l.Case_Id == caseId 
                select new CaseLog
                {
                    CaseId = l.Case_Id, 
                    Charge = l.Charge,  
                    EquipmentPrice = l.EquipmentPrice, 
                    FinishingDate = l.FinishingDate, 
                    FinishingType = l.FinishingType, 
                    Id = l.Id, 
                    InformCustomer = l.InformCustomer,  
                    LogDate = l.LogDate,
                    LogType = l.LogType, 
                    Price = l.Price,  
                    RegUser = l.RegUser,
                    TextExternal = l.Text_External, 
                    TextInternal = l.Text_Internal,  
                    UserId = l.User_Id,  
                    //UserName = usr.FirstName.FirstOrDefault() + " " +  usr.SurName.FirstOrDefault(), TODO get UserName
                    WorkingTimeHour = l.WorkingTime,  
                    WorkingTimeMinute = l.WorkingTime 
                };

            return query.OrderByDescending(x => x.Id);
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
