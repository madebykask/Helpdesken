using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<EmailLog> GetEmailLogsByLogId(int logId)
        {
            return DataContext.EmailLogs.Where(l => l.Log_Id == logId).ToList();
        }

        public List<EmailLog> GetEmailLogsByCaseId(int caseId)  
        {
            return (from l in this.DataContext.EmailLogs
                    join h in this.DataContext.CaseHistories on l.CaseHistory_Id equals h.Id
                    where h.Case_Id == caseId
                    select l).ToList(); 
        }

        public List<EmailLog> GetEmailLogsByCaseHistoryId(int caseHistoryId)
        {            
            return (from l in this.DataContext.EmailLogs                    
                    where l.CaseHistory_Id == caseHistoryId
                    select l).ToList(); 

        }

        public EmailLog  GetEmailLogsByGuid(Guid Id)
        {
            return (from l in this.DataContext.EmailLogs
                    where l.EmailLogGUID == Id
                    select l).FirstOrDefault();

        }

        public void DeleteByLogId(int logId)
        {
            var mails = Table.Where(m => m.Log_Id == logId);
            if (mails.Any())
            {
                DataContext.EmailLogs.RemoveRange(mails);
            }

        }
    }

    public class EmailLogAttemptRepository : RepositoryBase<EmailLogAttempt>, IEmailLogAttemptRepository
    {
        public EmailLogAttemptRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public void DeleteLogAttempts(int logId)
        {
            Delete(la => la.EmailLog_Id == logId);
            Commit();
        }
       
    }
}