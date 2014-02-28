namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IEmailLogRepository : IRepository<EmailLog>
    {
        List<EmailLog> GetEmailLogsByCaseId(int caseId);
    }
}