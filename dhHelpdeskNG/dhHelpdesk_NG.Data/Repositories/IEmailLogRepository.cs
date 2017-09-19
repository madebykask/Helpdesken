namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;

    public interface IEmailLogRepository : IRepository<EmailLog>
    {
        List<EmailLog> GetEmailLogsByCaseId(int caseId);

        List<EmailLog> GetEmailLogsByCaseHistoryId(int caseHistoryId);

        EmailLog GetEmailLogsByGuid(Guid Id);
        
    }

    public interface IEmailLogAttemptRepository : IRepository<EmailLogAttempt>
    {
        void DeleteLogAttempts(int logId);        
    }
}