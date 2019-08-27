using System;
using System.Collections.Generic;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public interface IEmailLogRepository : IRepository<EmailLog>
    {
        IList<EmailLog> GetEmailLogsByLogId(int logId);

        List<EmailLog> GetEmailLogsByCaseId(int caseId);

        List<EmailLog> GetEmailLogsByCaseHistoryId(int caseHistoryId);

        EmailLog GetEmailLogsByGuid(Guid Id);

        void DeleteByLogId(int logId);

    }

    public interface IEmailLogAttemptRepository : IRepository<EmailLogAttempt>
    {
        void DeleteLogAttempts(int logId);        
    }
}